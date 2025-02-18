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

var app = new CommandApp<TrxCommand>();

// Alias -? to -h for help
if (args.Contains("-?"))
    args = args.Select(x => x == "-?" ? "-h" : x).ToArray();

if (args.Contains("--debug"))
    Debugger.Launch();

app.Configure(config =>
{
    config.SetApplicationName(ThisAssembly.Project.ToolCommandName);
    // Causes -v|--version to be added to help
    config.SetApplicationVersion(ThisAssembly.Project.Version);

    if (Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
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
    if (args.Contains("-u") && !args.Contains("--unattended"))
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