using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests;

[TestFixture]
public class DemeritPointCalculatorTests
{
    private DemeritPointsCalculator _demeritPointCalculator;

    [SetUp]
    public void SetUp()
    {
        _demeritPointCalculator = new DemeritPointsCalculator();
    }

    [Test]
    [TestCase(0,0)]
    [TestCase(64,0)]
    [TestCase(65,0)]
    [TestCase(66,0)]
    [TestCase(69,0)]
    [TestCase(70,1)]
    [TestCase(71,1)]
    [TestCase(75,2)]
    [TestCase(80,3)]
    public void CalculateDemeritPoints_Speeds_ReturnsDemeritPoints(int speed, int expectedResult)
    {
        var result = _demeritPointCalculator.CalculateDemeritPoints(speed);
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}