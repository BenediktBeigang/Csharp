using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests;

[TestFixture]
public class StackTests
{
    [SetUp]
    public void SetUp() {}

    [Test]
    public void Push_NoItem_Exception()
    {
        Stack<string> stack = new Stack<string>();

        Assert.That(() => stack.Push(null), Throws.ArgumentNullException);
    }

    [Test]
    public void Push_WhenCalled_AddItem()
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(1);

        var expectedCount = 1;

        Assert.That(expectedCount, Is.EqualTo(stack.Count));
    }

    [Test]
    public void Pop_WhenCalled_DeleteItem()
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(1);
        stack.Pop();

        var expectedCount = 0;

        Assert.That(expectedCount, Is.EqualTo(stack.Count));
    }

    [Test]
    public void Peek_WhenCalled_ReturnItem()
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(5);

        var expectedCount = 5;

        Assert.That(expectedCount, Is.EqualTo(stack.Peek()));
    }
}