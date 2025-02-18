using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Devlooped.Web;
using Humanizer;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.Process;
using static Spectre.Console.AnsiConsole;

namespace Devlooped;

public partial class TrxCommand : Command<TrxCommand.TrxSettings>
{
    static readonly JsonSerializerOptions indentedJson = new() { WriteIndented = true };
    const string Header = "<!-- header -->";
    const string Footer = "<!-- footer -->";

    static string Author =>
        $"from [{ThisAssembly.Project.PackageId}]({ThisAssembly.Project.PackageProjectUrl}) v{ThisAssembly.Project.Version} on {RuntimeInformation.FrameworkDescription} with [:purple_heart:](https://github.com/sponsors/devlooped)";

    public enum Verbosity
    {
        [Description("Only failed tests are displayed")]
        Quiet,
        [Description("Failed and skipped tests are displayed")]
        Normal,
        [Description("Failed, skipped and passed tests are displayed")]
        Verbose,
    }

    public class TrxSettings : CommandSettings
    {
        [Description("Optional base directory for *.trx files discovery. Defaults to current directory.")]
        [CommandOption("-p|--path")]
        public string? Path { get; set; }

        [Description("Include test output")]
        [CommandOption("-o|--output")]
        [DefaultValue(false)]
        public bool Output { get; set; }

        [Description("Recursively search for *.trx files")]
        [CommandOption("-r|--recursive")]
        [DefaultValue(true)]
        public bool Recursive { get; set; } = true;

        /// <summary>
        /// Output verbosity.
        /// </summary>
        [Description(
            """
            Output display verbosity:
            - quiet: only failed tests are displayed
            - normal: failed and skipped tests are displayed
            - verbose: failed, skipped and passed tests are displayed
            """)]
        [CommandOption("-v|--verbosity <quiet|normal|verbose>")]
        [DefaultValue(Verbosity.Quiet)]
        public Verbosity Verbosity { get; set; } = Verbosity.Quiet;

        /// <summary>
        /// Whether to include skipped tests in the output.
        /// </summary>
        [Description("Include skipped tests in output")]
        [CommandOption("--skipped", IsHidden = true)]
        [DefaultValue(true)]
        public bool Skipped
        {
            get => Verbosity != Verbosity.Quiet;
            set
            {
                if (!value)
                    Verbosity = Verbosity.Quiet;
            }
        }

        /// <summary>
        /// Whether to return a -1 exit code on test failures.
        /// </summary>
        [Description("Do not return a -1 exit code on test failures")]
        [CommandOption("--no-exit-code")]
        public bool NoExitCode { get; set; }

        /// <summary>
        /// Report as GitHub PR comment.
        /// </summary>
        [Description("Report as GitHub PR comment")]
        [CommandOption("--gh-comment")]
        [DefaultValue(true)]
        public bool GitHubComment { get; set; } = true;

        /// <summary>
        /// Report as GitHub PR comment.
        /// </summary>
        [Description("Report as GitHub step summary")]
        [CommandOption("--gh-summary")]
        [DefaultValue(true)]
        public bool GitHubSummary { get; set; } = true;

        public override ValidationResult Validate()
        {
            // Validate, normalize and default path.
            var path = Path ?? Directory.GetCurrentDirectory();
            if (!System.IO.Path.IsPathFullyQualified(path))
                path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), path);

            Path = File.Exists(path) ? new FileInfo(path).DirectoryName! : System.IO.Path.GetFullPath(path);

            return base.Validate();
        }
    }

    public override int Execute(CommandContext context, TrxSettings settings)
    {
        if (Environment.GetEnvironmentVariable("RUNNER_DEBUG") == "1")
            WriteLine(JsonSerializer.Serialize(new { settings }, indentedJson));

        // We get this validated by the settings, so it's always non-null.
        var path = settings.Path!;
        var search = settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var testIds = new HashSet<string>();
        var passed = 0;
        var failed = 0;
        var skipped = 0;
        var duration = TimeSpan.Zero;
        var failures = new List<Failed>();

        // markdown details for gh comment
        var details = new StringBuilder().AppendLine(
            $"""
            <details>

            <summary>:test_tube: Details on {OS}</summary>

            """);

        var results = new List<XElement>();

        Status().Start("Discovering test results...", ctx =>
        {
            // Process from newest files to oldest so that newest result we find (by test id) is the one we keep
            foreach (var trx in Directory.EnumerateFiles(path, "*.trx", search).OrderByDescending(File.GetLastWriteTime))
            {
                ctx.Status($"Discovering test results in {Path.GetFileName(trx).EscapeMarkup()}...");
                using var file = File.OpenRead(trx);
                // Clears namespaces
                var doc = HtmlDocument.Load(file, new HtmlReaderSettings { CaseFolding = Sgml.CaseFolding.None });
                foreach (var result in doc.CssSelectElements("UnitTestResult"))
                {
                    var id = result.Attribute("testId")!.Value;
                    // Process only once per test id, this avoids duplicates when multiple trx files are processed
                    if (testIds.Add(id))
                        results.Add(result);
                }
            }

            ctx.Status("Sorting tests by name...");
            results.Sort(new Comparison<XElement>((x, y) => x.Attribute("testName")!.Value.CompareTo(y.Attribute("testName")!.Value)));
        });

        foreach (var result in results)
        {
            var test = result.Attribute("testName")!.Value.EscapeMarkup();
            var elapsed = TimeSpan.Parse(result.Attribute("duration")?.Value ?? "0");
            var output = settings.Output ? result.CssSelectElement("StdOut")?.Value : default;

            switch (result.Attribute("outcome")?.Value)
            {
                case "Passed":
                    passed++;
                    duration += elapsed;
                    if (settings.Verbosity != Verbosity.Verbose)
                        break;

                    MarkupLine($":check_mark_button: {test}");
                    if (output == null)
                        details.AppendLine($":white_check_mark: {test}");
                    else
                        details.AppendLine(
                            $"""
                                <details>

                                <summary>:white_check_mark: {test}</summary>

                                """)
                            .AppendLineIndented(output, "> &gt; ")
                            .AppendLine(
                            """

                                </details>
                                """);
                    break;
                case "Failed":
                    failed++;
                    duration += elapsed;
                    MarkupLine($":cross_mark: {test}");
                    details.AppendLine(
                        $"""
                            <details>

                            <summary>:x: {test}</summary>
            
                            """);
                    WriteError(path, failures, result, details);
                    if (output != null)
                        details.AppendLineIndented(output, "> &gt; ");
                    details.AppendLine().AppendLine("</details>").AppendLine();
                    break;
                case "NotExecuted":
                    skipped++;
                    if (settings.Verbosity == Verbosity.Quiet)
                        break;

                    var reason = result.CssSelectElement("Output > ErrorInfo > Message")?.Value;
                    Markup($"[dim]:white_question_mark: {test}[/]");
                    details.Append($":grey_question: {test}");

                    if (reason != null)
                    {
                        Markup($"[dim] => {reason.EscapeMarkup()}[/]");
                        details.Append($" => {reason}");
                    }

                    WriteLine();
                    details.AppendLine();
                    break;
                default:
                    break;
            }

            if (output != null)
            {
                Write(new Panel($"[dim]{output.ReplaceLineEndings().EscapeMarkup()}[/]")
                {
                    Border = BoxBorder.None,
                    Padding = new Padding(5, 0, 0, 0),
                });
            }
        }

        details.AppendLine().AppendLine("</details>");

        var summary = new Summary(passed, failed, skipped, duration);
        WriteLine();
        MarkupSummary(summary);
        WriteLine();

        if (Environment.GetEnvironmentVariable("CI") == "true" &&
            (settings.GitHubComment || settings.GitHubSummary))
        {
            GitHubReport(settings, summary, details);
            if (failures.Count > 0)
            {
                // Send workflow commands for each failure to be annotated in GH CI
                // TODO: somehow the notice does not end up pointing to the right file/line
                //foreach (var failure in failures)
                //    WriteLine($"::error file={failure.File},line={failure.Line},title={failure.Title}::{failure.Message}");
            }
        }

        return settings.NoExitCode || failed == 0 ? 0 : -1;
    }

    static void MarkupSummary(Summary summary)
    {
        Markup($":backhand_index_pointing_right: Run {summary.Total} tests in ~ {summary.Duration.Humanize()}");

        if (summary.Failed > 0)
            MarkupLine($" :cross_mark:");
        else
            MarkupLine($" :check_mark_button:");

        if (summary.Passed > 0)
            MarkupLine($"   :check_mark_button: {summary.Passed} passed");

        if (summary.Failed > 0)
            MarkupLine($"   :cross_mark: {summary.Failed} failed");

        if (summary.Skipped > 0)
            MarkupLine($"   :white_question_mark: {summary.Skipped} skipped");
    }

    static void GitHubReport(TrxSettings settings, Summary summary, StringBuilder details)
    {
        // Don't report anything if there's nothing to report.
        if (summary.Total == 0)
            return;

        if (Environment.GetEnvironmentVariable("GITHUB_EVENT_NAME") != "pull_request" ||
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != "true")
            return;

        if (TryExecute("gh", "--version", out var output) && output?.StartsWith("gh version") != true)
            return;

        // See https://docs.github.com/en/actions/learn-github-actions/variables#default-environment-variables 
        if (Environment.GetEnvironmentVariable("GITHUB_REF_NAME") is not { } branch ||
            !branch.EndsWith("/merge") ||
            !int.TryParse(branch[..^6], out var pr) ||
            Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") is not { Length: > 0 } repo ||
            Environment.GetEnvironmentVariable("GITHUB_RUN_ID") is not { Length: > 0 } runId ||
            Environment.GetEnvironmentVariable("GITHUB_JOB") is not { Length: > 0 } jobName ||
            Environment.GetEnvironmentVariable("GITHUB_SERVER_URL") is not { Length: > 0 } serverUrl)
            return;

        // Some day, it might just show-up and we'd be forwards compatible. 
        // See https://github.com/orgs/community/discussions/129314 and https://github.com/actions/runner/issues/324
        var jobId = Environment.GetEnvironmentVariable("GITHUB_JOB_ID");

        // Provide a mechanism that would work on matrix in the meantime
        if (Environment.GetEnvironmentVariable("GH_JOB_NAME") is { Length: > 0 } ghJobName)
            jobName = ghJobName;

        string? jobUrl = default;

        if (!string.IsNullOrEmpty(jobId))
            jobUrl = $"{serverUrl}/{repo}/actions/runs/{runId}/jobs/{jobId}?pr={pr}";

        var sb = new StringBuilder();
        var elapsed = FormatTimeSpan(summary.Duration);
        long commentId = 0;

        if (jobUrl == null && TryExecute("gh",
            ["api", $"repos/devlooped/dotnet-trx/actions/runs/{runId}/jobs", "--jq", $"[.jobs[] | select(.name == \"{jobName}\") | .id]"],
            out var jobsJson) && jobsJson != null && JsonSerializer.Deserialize<long[]>(jobsJson) is { Length: 1 } jobIds)
        {
            jobUrl = $"{serverUrl}/{repo}/actions/runs/{runId}/job/{jobIds[0]}?pr={pr}";
        }

        static string Link(string image, string? url) => url == null ? image + " " : $"[{image}]({url}) ";

        static StringBuilder AppendBadges(Summary summary, StringBuilder builder, string elapsed, string? jobUrl)
        {
            elapsed = elapsed.Replace(" ", "%20");

            // ![5 passed](https://img.shields.io/badge/❌-linux%20in%2015m%206s-blue) ![5 passed](https://img.shields.io/badge/os-macOS%20✅-blue)
            if (summary.Failed > 0)
                builder.Append(Link($"![{summary.Failed} failed](https://img.shields.io/badge/❌-{Runtime}%20in%20{elapsed}-blue)", jobUrl));
            else if (summary.Passed > 0)
                builder.Append(Link($"![{summary.Passed} passed](https://img.shields.io/badge/✅-{Runtime}%20in%20{elapsed}-blue)", jobUrl));
            else
                builder.Append(Link($"![{summary.Skipped} skipped](https://img.shields.io/badge/⚪-{Runtime}%20in%20{elapsed}-blue)", jobUrl));

            if (summary.Passed > 0)
                builder.Append(Link($"![{summary.Passed} passed](https://img.shields.io/badge/passed-{summary.Passed}-brightgreen)", jobUrl));
            if (summary.Failed > 0)
                builder.Append(Link($"![{summary.Failed} failed](https://img.shields.io/badge/failed-{summary.Failed}-red)", jobUrl));
            if (summary.Skipped > 0)
                builder.Append(Link($"![{summary.Skipped} skipped](https://img.shields.io/badge/skipped-{summary.Skipped}-silver)", jobUrl));

            builder.AppendLine();
            return builder;
        }

        // Find potentially existing comment to update
        if (TryExecute("gh",
            ["api", $"repos/{repo}/issues/{pr}/comments", "--jq", "[.[] | { id:.id, body:.body } | select(.body | contains(\"<!-- trx\")) | .id][0]"],
            out var comment) && comment != null && long.TryParse(comment.Trim(), out commentId) &&
            TryExecute("gh", ["api", $"repos/{repo}/issues/comments/{commentId}", "--jq", ".body"], out var body) && body != null &&
            body.IndexOf(Header) is var start && start != -1 &&
            body.IndexOf(Footer) is var end && end != -1 && end > start &&
            // We only append if the run id matches the current run, otherwise we'll just create
            // entirely new body. Means we'll only keep the latest run for the PR to keep the noise down.
            TrxRunId().Match(body) is { Success: true } match &&
            match.Groups["id"].Value == runId)
        {
            sb.AppendLine(body[..start].TrimEnd());
            AppendBadges(summary, sb, elapsed, jobUrl);
            sb.AppendLine(body[start..end].Trim())
              .AppendLine()
              .Append(details)
              .AppendLine()
              .AppendLine(body[end..].TrimStart());
        }
        else
        {
            AppendBadges(summary, sb, elapsed, jobUrl);
            sb.AppendLine(Header)
              .AppendLine()
              .Append(details)
              .AppendLine(Footer)
              .AppendLine()
              .AppendLine(Author);
        }

        body = sb.ToString().Trim();
        if (body.IndexOf("<!-- trx") is var signatureIndex && signatureIndex != -1)
            body = body[..signatureIndex].Trim();

        // Always append the run this comment belongs to, for grouping.
        body += Environment.NewLine + $"<!-- trx:{runId} -->";

        var input = Path.GetTempFileName();
        if (settings.GitHubComment)
        {
            if (commentId > 0)
            {
                // API requires a json payload
                File.WriteAllText(input, JsonSerializer.Serialize(new { body }));
                TryExecute("gh", $"api repos/{repo}/issues/comments/{commentId} -X PATCH --input {input}", out _);
            }
            else
            {
                // CLI can use the straight body
                File.WriteAllText(input, body);
                TryExecute("gh", $"pr comment {pr} --body-file {input}", out _);
            }
        }

        if (settings.GitHubSummary &&
            Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY") is { Length: > 0 } summaryPath)
        {
            File.AppendAllText(summaryPath,
                AppendBadges(summary, new(), elapsed, jobUrl)
                .AppendLine()
                .Append(details)
                .AppendLine()
                .AppendLine(Author)
                .ToString());
        }
    }

    void WriteError(string baseDir, List<Failed> failures, XElement result, StringBuilder details)
    {
        if (result.CssSelectElement("Message")?.Value is not string message ||
            result.CssSelectElement("StackTrace")?.Value is not string stackTrace)
            return;

        var testName = result.Attribute("testName")!.Value;
        var testId = result.Attribute("testId")!.Value;
        var method = result.Document!.CssSelectElement($"UnitTest[id={testId}] TestMethod");
        var lines = stackTrace.ReplaceLineEndings().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        if (method != null)
        {
            var fullName = $"{method.Attribute("className")?.Value}.{method.Attribute("name")?.Value}";
            var last = Array.FindLastIndex(lines, x => x.Contains(fullName));
            // Stop lines when we find the last one from the test method
            if (last != -1)
                lines = lines[..(last + 1)];
        }

        Failed? failed = null;
        var cli = new StringBuilder();
        details.Append("> ```");
        if (stackTrace.Contains(".vb:line"))
            details.AppendLine("vb");
        else
            details.AppendLine("csharp");

        foreach (var line in lines.Select(x => x.EscapeMarkup()))
        {
            var match = ParseFile().Match(line);
            if (!match.Success)
            {
                cli.AppendLine(line);
                details.AppendLineIndented(line, "> ");
                continue;
            }

            var file = match.Groups["file"].Value;
            var pos = match.Groups["line"].Value;
            var relative = file;
            if (Path.IsPathRooted(file) && file.StartsWith(baseDir))
                relative = file[baseDir.Length..].TrimStart(Path.DirectorySeparatorChar);

            // NOTE: we replace whichever was last, since we want the annotation on the 
            // last one with a filename, which will be the test itself (see previous skip from last found).
            failed = new Failed(testName,
                message.ReplaceLineEndings().Replace(Environment.NewLine, "%0A"),
                stackTrace.ReplaceLineEndings().Replace(Environment.NewLine, "%0A"),
                relative, int.Parse(pos));

            cli.AppendLine(line.Replace(file, $"[link={file}][steelblue1_1]{relative}[/][/]"));
            // TODO: can we render a useful link in comment details?
            details.AppendLineIndented(line.Replace(file, relative), "> ");
        }

        var error = new Panel(
            $"""
            [red]{message.EscapeMarkup()}[/]
            [dim]{cli}[/]
            """);
        error.Padding = new Padding(5, 0, 0, 0);
        error.Border = BoxBorder.None;
        Write(error);

        // Use a blockquote for the entire error message

        details.AppendLine("> ```");

        // Add to collected failures we may report to GH CI
        if (failed != null)
            failures.Add(failed);
    }

    static string Runtime => RuntimeInformation.RuntimeIdentifier.Replace("-", "&dash;");
    static string OS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
        // Otherwise we end up with this, yuck: Darwin 23.5.0 Darwin Kernel Version 23.5.0: Wed May 1 20:12:39 PDT 2024; root:xnu-10063.121.3~5/RELEASE_ARM64_VMAPPLE
        ? $"macOS {Environment.OSVersion.VersionString}" :
        RuntimeInformation.OSDescription;

    static string FormatTimeSpan(TimeSpan timeSpan)
    {
        var parts = new List<string>();

        if (timeSpan.Hours > 0)
            parts.Add($"{timeSpan.Hours}h");

        if (timeSpan.Minutes > 0)
            parts.Add($"{timeSpan.Minutes}m");

        if (timeSpan.Seconds > 0 || parts.Count == 0) // Always include seconds if no other parts
            parts.Add($"{timeSpan.Seconds}s");

        return string.Join(" ", parts);
    }

    // in C:\path\to\file.cs:line 123
    [GeneratedRegex(@" in (?<file>.+):line (?<line>\d+)", RegexOptions.Compiled)]
    private static partial Regex ParseFile();

    [GeneratedRegex(@"<!--\strx:(?<id>\d+)\s-->")]
    private static partial Regex TrxRunId();

    record Summary(int Passed, int Failed, int Skipped, TimeSpan Duration)
    {
        public int Total => Passed + Failed + Skipped;
    }

    record Failed(string Test, string Title, string Message, string File, int Line);
}
