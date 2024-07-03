using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Devlooped.Web;
using Humanizer;
using Spectre.Console;
using Spectre.Console.Cli;
using static Spectre.Console.AnsiConsole;

namespace Devlooped;

public class TrxCommand : Command<TrxCommand.TrxSettings>
{
    public class TrxSettings : CommandSettings
    {
        [Description("Optional base directory for *.trx files discovery. Defaults to current directory.")]
        [CommandOption("-p|--path")]
        public string? Path { get; init; }

        [Description("Include test output")]
        [CommandOption("-o|--output")]
        [DefaultValue(false)]
        public bool Output { get; init; }

        [Description("Recursively search for *.trx files")]
        [CommandOption("-r|--recursive")]
        [DefaultValue(true)]
        public bool Recursive { get; init; } = true;

        [Description("Show version information")]
        [CommandOption("--version")]
        public bool? Version { get; init; }
    }

    public override int Execute(CommandContext context, TrxSettings settings)
    {
        var path = settings.Path ?? Directory.GetCurrentDirectory();
        var search = settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var testIds = new HashSet<string>();
        var passed = 0;
        var failed = 0;
        var skipped = 0;
        var duration = TimeSpan.Zero;

        // Process from newest files to oldest
        foreach (var trx in Directory.EnumerateFiles(path, "*.trx", search).OrderByDescending(File.GetLastWriteTime))
        {
            using var file = File.OpenRead(trx);
            // Clears namespaces
            var doc = HtmlDocument.Load(file, new HtmlReaderSettings { CaseFolding = Sgml.CaseFolding.None });

            foreach (var result in doc.CssSelectElements("UnitTestResult").OrderBy(x => x.Attribute("testName")?.Value))
            {
                var id = result.Attribute("testId")!.Value;
                // Process only once per test id, this avoids duplicates when multiple trx files are processed
                if (!testIds.Add(id))
                    continue;

                var test = result.Attribute("testName")!.Value;
                switch (result.Attribute("outcome")?.Value)
                {
                    case "Passed":
                        passed++;
                        MarkupLine($":check_mark_button: {test}");
                        break;
                    case "Failed":
                        failed++;
                        MarkupLine($":cross_mark: {test}");
                        if (result.CssSelectElement("Message")?.Value is string message &&
                            result.CssSelectElement("StackTrace")?.Value is string stackTrace)
                        {
                            var error = new Panel(
                                $"""
                                [red]{message}[/]
                                [dim]{CleanStackTrace(result, stackTrace.ReplaceLineEndings())}[/]
                                """);
                            error.Padding = new Padding(5, 0, 0, 0);
                            error.Border = BoxBorder.None;
                            Write(error);
                        }
                        break;
                    case "NotExecuted":
                        skipped++;
                        MarkupLine($":fast_forward_button: [dim]{test}[/]");
                        break;
                    default:
                        break;
                }

                if (settings.Output == true && result.CssSelectElement("StdOut")?.Value is { } output)
                    Write(new Panel($"[dim]{output.ReplaceLineEndings()}[/]")
                    {
                        Border = BoxBorder.None,
                        Padding = new Padding(5, 0, 0, 0),
                    });
            }

            var times = doc.CssSelectElement("Times");
            if (times == null)
                continue;

            var start = DateTime.Parse(times.Attribute("start")!.Value);
            var finish = DateTime.Parse(times.Attribute("finish")!.Value);
            duration += finish - start;
        }

        WriteLine();
        Markup($":backhand_index_pointing_right: Run {passed + failed + skipped} tests in ~ {duration.Humanize()}");

        if (failed > 0)
            MarkupLine($" :cross_mark:");
        else
            MarkupLine($" :check_mark_button:");

        if (passed > 0)
            MarkupLine($"   :check_mark_button: {passed} passed");

        if (failed > 0)
            MarkupLine($"   :cross_mark: {failed} failed");

        if (skipped > 0)
            MarkupLine($"   :fast_forward_button: {skipped} skipped");

        WriteLine();

        return 0;
    }

    string CleanStackTrace(XElement result, string stackTrace)
    {
        // Stop lines when we find the last one from the test method
        var testId = result.Attribute("testId")!.Value;
        var method = result.Document!.CssSelectElement($"UnitTest[id={testId}] TestMethod");
        if (method == null)
            return stackTrace;

        var fullName = $"{method.Attribute("className")?.Value}.{method.Attribute("name")?.Value}";

        var lines = stackTrace.Split(Environment.NewLine);
        var last = Array.FindLastIndex(lines, x => x.Contains(fullName));
        if (last == -1)
            return stackTrace;

        return string.Join(Environment.NewLine, lines.Take(last + 1));
    }
}
