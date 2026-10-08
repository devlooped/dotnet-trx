using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Devlooped.Tests;

public class BatchTests
{
    static readonly SemaphoreSlim Gate = new(1, 1);
    static string? tool;

    [Fact]
    public async Task Batch_IsPlainTextWithoutStatusOrLinks()
    {
        var directory = WriteSample();
        try
        {
            var (code, stdout, stderr) = await Run("--batch", "--no-exit-code", "-p", directory);

            Assert.Equal(0, code);
            Assert.Equal(string.Empty, stderr);
            Assert.DoesNotContain('\u001b', stdout);
            Assert.DoesNotContain("Discovering test results", stdout);
            Assert.DoesNotContain("Sorting tests by name", stdout);
            Assert.Contains("Sample.BatchFails", stdout);
            Assert.Contains("Tests.cs", stdout);
            Assert.Contains("Assert.Equal() Failure", stdout);
            Assert.DoesNotContain(Path.Combine(directory, "Tests.cs"), stdout);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task Default_KeepsColorsLinksAndStatus()
    {
        var directory = WriteSample();
        try
        {
            // -u skips the update check the same way production already does, so this
            // stays about rendering and does not depend on NuGet being reachable.
            var (code, stdout, stderr) = await Run("-u", "--no-exit-code", "-p", directory);
            var source = Path.Combine(directory, "Tests.cs");

            Assert.Equal(0, code);
            Assert.Equal(string.Empty, stderr);
            Assert.Contains('\u001b', stdout);
            Assert.Contains("\u001b]8;", stdout);
            Assert.Contains(source, stdout);
            Assert.Contains("Sorting tests by name", stdout);
            Assert.Contains("Sample.BatchFails", stdout);
            Assert.Contains("Tests.cs", stdout);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task BatchHelp_IsUnstyledAndWrapsAtNormalWidth()
    {
        var (code, stdout, stderr) = await Run("--batch", "--help");

        Assert.Equal(0, code);
        Assert.Equal(string.Empty, stderr);
        Assert.Contains("--batch", stdout);
        Assert.Contains("Plain text for scripts and CI", stdout);
        Assert.DoesNotContain('\u001b', stdout);
        Assert.InRange(stdout.Replace("\r", string.Empty).Split('\n').Max(line => line.Length), 1, 100);
    }

    [Fact]
    public async Task BatchVersion_PrintsPlainLinkText()
    {
        var (code, stdout, stderr) = await Run("--batch", "--version");

        Assert.Equal(0, code);
        Assert.Equal(string.Empty, stderr);
        Assert.Contains("version", stdout);
        Assert.Contains("https://", stdout);
        Assert.DoesNotContain('\u001b', stdout);
    }

    static string WriteSample()
    {
        var directory = Path.Combine(Path.GetTempPath(), "trx-batch-" + Guid.NewGuid().ToString("n"));
        Directory.CreateDirectory(directory);
        var source = Path.Combine(directory, "Tests.cs");
        var stack = SecurityElement.Escape($"   at Sample.BatchFails() in {source}:line 42");
        File.WriteAllText(Path.Combine(directory, "sample.trx"),
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun>
              <Results>
                <UnitTestResult testId="2" testName="Sample.BatchFails" duration="00:00:00.2000000" outcome="Failed">
                  <Output>
                    <ErrorInfo>
                      <Message>Assert.Equal() Failure</Message>
                      <StackTrace>{stack}</StackTrace>
                    </ErrorInfo>
                  </Output>
                </UnitTestResult>
              </Results>
            </TestRun>
            """);
        return directory;
    }

    static async Task<(int ExitCode, string Stdout, string Stderr)> Run(params string[] args)
    {
        var dll = await ToolDll();
        var start = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
        };
        start.ArgumentList.Add(dll);
        foreach (var arg in args)
            start.ArgumentList.Add(arg);

        // Force a color-capable terminal so the default path is observable when stdout is captured.
        start.Environment["TERM"] = "xterm-256color";
        start.Environment["COLORTERM"] = "truecolor";
        start.Environment.Remove("NO_COLOR");
        start.Environment["CI"] = "false";
        start.Environment.Remove("GITHUB_ACTIONS");
        start.Environment.Remove("GITHUB_EVENT_NAME");

        using var process = Process.Start(start) ?? throw new InvalidOperationException("Failed to start trx.");
        process.StandardInput.Close();
        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return (process.ExitCode, await stdoutTask, await stderrTask);
    }

    static async Task<string> ToolDll()
    {
        await Gate.WaitAsync();
        try
        {
            if (tool != null)
                return tool;

            var root = RepoRoot();
            var project = Path.Combine(root, "src", "dotnet-trx", "dotnet-trx.csproj");
            var configuration = typeof(BatchTests).Assembly.Location.Contains($"{Path.DirectorySeparatorChar}Release{Path.DirectorySeparatorChar}")
                ? "Release"
                : "Debug";
            var start = new ProcessStartInfo("dotnet", $"build \"{project}\" -c {configuration} -p:NoHelp=true --nologo")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            using var process = Process.Start(start) ?? throw new InvalidOperationException("Failed to build trx.");
            var stdout = await process.StandardOutput.ReadToEndAsync();
            var stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
                throw new InvalidOperationException(stdout + stderr);

            tool = Path.Combine(root, "src", "dotnet-trx", "bin", configuration, "net8.0", "dotnet-trx.dll");
            if (!File.Exists(tool))
                throw new FileNotFoundException("Built trx tool was not found.", tool);

            return tool;
        }
        finally
        {
            Gate.Release();
        }
    }

    static string RepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "dotnet-trx.sln")))
            dir = dir.Parent;

        return dir?.FullName ?? throw new InvalidOperationException("Could not find the repository root.");
    }
}
