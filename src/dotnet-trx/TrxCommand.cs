using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Devlooped.Web;
using Humanizer;
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

        [Description("Recursively search for *.trx files.")]
        [CommandOption("-r|--recursive")]
        [DefaultValue(true)]
        public bool Recursive { get; init; } = true;

        [Description("Show version information.")]
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
                        break;
                    case "NotExecuted":
                        MarkupLine($":fast_forward_button: {test}");
                        skipped++;
                        break;
                    default:
                        break;
                }
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
                MarkupLine($"  :check_mark_button: {passed} passed");

            if (failed > 0)
                MarkupLine($"  :cross_mark: {failed} failed");

            if (skipped > 0)
                MarkupLine($"  :fast_forward_button: {skipped} skipped");
        }

        return 0;
    }
}
