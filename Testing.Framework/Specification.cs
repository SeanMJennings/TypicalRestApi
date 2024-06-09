using FluentAssertions;
using NUnit.Framework;

namespace Framework;

[TestFixture]
public class Specification
{
    protected static Exception error = null!;

    [OneTimeSetUp]
    protected virtual void before_all()
    {
        error = null!;
    }     
    
    [SetUp]
    protected virtual void before_each()
    {
        error = null!;
    }    
    
    [TearDown]
    protected virtual void after_each(){}    
    
    [OneTimeTearDown]
    protected virtual void after_all(){}

    protected static void Given(Action testAction)
    {
        testAction.Invoke();
    }

    protected static void And(Action testAction)
    {
        testAction.Invoke();
    }

    protected static void When(Action testAction)
    {
        testAction.Invoke();
    }

    protected static void Then(Action testAction)
    {
        testAction.Invoke();
    }
    
    protected static void Scenario(Action testAction)
    {
        testAction.Invoke();
    }
    
    protected static Action Validating(Action testAction)
    {
        return () =>
        {
            try
            {
                testAction.Invoke();
            }
            catch (Exception e)
            {
                error = e;
            }
        };
    }
    
    protected static Action Informs(string message)
    {
        return () =>
        {
            error.Should().NotBeNull();
            error.Message.Should().Be(message);
        };
    }
}