using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Demo;

public class Tests(ITestOutputHelper output)
{
    [Fact]
    public void ICanHasOutput() => output.WriteLine("Hello, world from xunit ITestOutputHelper!");

    [Fact(Skip = "Shouldn't run for now :)")]
    public void SampleSkipped() { }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    [InlineData("de")]
    [InlineData("es")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public async Task Parameterized(string culture) => await Task.Delay(Random.Shared.Next(100, 2000));
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters

    [Fact]
    public void OhNoh() => Assert.Equal(42, 22);

    [Fact]
    public void CleanStackTrace()
    {
        Action runner = () => Run();

        runner();
    }

    void Run() => Unexpected();

    void Unexpected() => throw new InvalidOperationException("This should not happen!");
}