// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Devlooped;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Spectre.Console;
using Spectre.Console.Cli;

// Scripts and CI pass --batch for plain text. Capabilities are fixed up front so
// help, version output and results all skip ANSI colors, hyperlinks and spinners.
var batch = args.Contains("--batch");
if (batch)
{
    var capabilities = AnsiConsole.Profile.Capabilities;
    capabilities.Ansi = false;
    capabilities.ColorSystem = ColorSystem.NoColors;
    capabilities.Links = false;
    capabilities.Interactive = false;
}

var app = new CommandApp<TrxCommand>();

// Alias -? to -h for help
if (args.Contains("-?"))
    args = args.Select(x => x == "-?" ? "-h" : x).ToArray();

if (args.Contains("--debug"))
    Debugger.Launch();

app.Configure(config =>
{
    config.SetApplicationName(ThisAssembly.Project.ToolCommandName);
    if (batch || Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
        config.Settings.HelpProviderStyles = null;
});

if (args.Contains("--version"))
{
    AnsiConsole.MarkupLine($"{ThisAssembly.Project.ToolCommandName} version [lime]{ThisAssembly.Project.Version}[/] ({ThisAssembly.Project.BuildDate})");
    AnsiConsole.MarkupLine($"[link]{ThisAssembly.Git.Url}/releases/tag/{ThisAssembly.Project.BuildRef}[/]");

    foreach (var message in await CheckUpdates(args))
        AnsiConsole.MarkupLine(message);

    return 0;
}

var updates = Task.Run(() => CheckUpdates(args));
var exit = app.Run(args);

if (await updates is { Length: > 0 } messages)
{
    foreach (var message in messages)
        AnsiConsole.MarkupLine(message);
}

return exit;

static async Task<string[]> CheckUpdates(string[] args)
{
    // Separate from the -u/--no-updates scan below so that flag can change independently.
    if (args.Contains("--batch"))
        return [];

    // Honored before Spectre runs, including on --version. --unattended is a hidden alias.
    if (args.Contains("-u") || args.Contains("--no-updates") || args.Contains("--unattended"))
        return [];

    var providers = Repository.Provider.GetCoreV3();
    var repository = new SourceRepository(new PackageSource("https://api.nuget.org/v3/index.json"), providers);
    var resource = await repository.GetResourceAsync<PackageMetadataResource>();
    var localVersion = new NuGetVersion(ThisAssembly.Project.Version);
    var metadata = await resource.GetMetadataAsync(ThisAssembly.Project.PackageId, true, false,
        new SourceCacheContext
        {
            NoCache = true,
            RefreshMemoryCache = true,
        },
        NuGet.Common.NullLogger.Instance, CancellationToken.None);

    var update = metadata
        .Select(x => x.Identity)
        .Where(x => x.Version > localVersion)
        .OrderByDescending(x => x.Version)
        .Select(x => x.Version)
        .FirstOrDefault();

    if (update != null)
    {
        return [
            $"There is a new version of [yellow]{ThisAssembly.Project.PackageId}[/]: [dim]v{localVersion.ToNormalizedString()}[/] -> [lime]v{update.ToNormalizedString()}[/]",
            $"Update with: [yellow]dotnet[/] tool update -g {ThisAssembly.Project.PackageId}"
        ];
    }

    return [];
}