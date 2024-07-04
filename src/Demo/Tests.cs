using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Demo;

public class Tests(ITestOutputHelper output)
{
    [Fact]
    public void Test_With_Output() => output.WriteLine("Hello, world from xunit ITestOutputHelper!");

    [Fact(Skip = "Shouldn't run for now :)")]
    public void Skipped_Test_Does_Not_Run() { }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    [InlineData("de")]
    [InlineData("es")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public async Task Parameterized(string culture) => await Task.Delay(Random.Shared.Next(100, 2000));
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters

    [Fact]
    public void Fails_With_Output()
    {
        output.WriteLine("It was going so well... ");
        output.WriteLine("Yet you never know");
        output.WriteLine("Which is why you sprinkle all these WriteLines :eyes:");
        Assert.Equal(42, 22);
    }

    [Fact]
    public void Fails_With_Complex_StackTrace()
    {
        Action runner = () => Run();

        runner();
    }

    void Run() => Unexpected();

    void Unexpected() => throw new InvalidOperationException("This should not happen!");
}