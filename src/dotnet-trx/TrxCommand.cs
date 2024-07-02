using System;
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
        public bool? Output { get; init; }

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

        foreach (var trx in Directory.EnumerateFiles(path, "*.trx", search))
        {
            using var file = File.OpenRead(trx);
            // This is not counted in the Counters.
            var skipped = 0;

            // Clears namespaces
            var doc = HtmlDocument.Load(file, new HtmlReaderSettings { CaseFolding = Sgml.CaseFolding.None });
            foreach (var result in doc.CssSelectElements("UnitTestResult").OrderBy(x => x.Attribute("testName")?.Value))
            {
                var test = result.Attribute("testName")?.Value;
                switch (result.Attribute("outcome")?.Value)
                {
                    case "Passed":
                        MarkupLine($":check_mark_button: {test}");
                        break;
                    case "Failed":
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
                        MarkupLine($":fast_forward_button: {test}");
                        skipped++;
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

            var counters = doc.CssSelectElement("ResultSummary > Counters");
            var times = doc.CssSelectElement("Times");
            if (counters == null || times == null)
                continue;

            var total = int.Parse(counters.Attribute("total")?.Value ?? "0");
            var passed = int.Parse(counters.Attribute("passed")?.Value ?? "0");
            var failed = int.Parse(counters.Attribute("failed")?.Value ?? "0");

            var start = DateTime.Parse(times.Attribute("start")?.Value!);
            var finish = DateTime.Parse(times.Attribute("finish")?.Value!);
            var duration = finish - start;

            WriteLine();

            Markup($":backhand_index_pointing_right: Run {total} tests in ~ {duration.Humanize()}");

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
        }

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
