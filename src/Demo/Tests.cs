using System;
using System.Threading.Tasks;

namespace Demo;

public class Tests
{
    [Fact(Skip = "Shouldn't run for now :)")]
    public void SampleSkipped() { }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    [InlineData("de")]
    [InlineData("es")]
    public async Task Parameterized(string culture) => await Task.Delay(Random.Shared.Next(100, 2000));

    [Fact]
    public void OhNoh() => Assert.Fail();
}