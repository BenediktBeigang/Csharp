using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests;

[TestFixture]
public class FizzBuzzTests
{

    [SetUp]
    public void SetUp() {}

    [Test]
    [TestCase(1, "1")]
    [TestCase(3, "Fizz")]
    [TestCase(5, "Buzz")]
    [TestCase(15, "FizzBuzz")]
    public void GetOutput_CallingFunction_ReturnsCorrectString(int number, string expectedResult)
    {
        var result = FizzBuzz.GetOutput(number);
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}