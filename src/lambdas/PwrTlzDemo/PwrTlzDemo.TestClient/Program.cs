using Amazon.Lambda.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;

namespace PwrTlzDemo.TestClient;

internal class Program
{
    public static void Main(string[] args)
    {
        var autoFixture = new Fixture();
        autoFixture.Customize(
            new CompositeCustomization(
                new AutoMoqCustomization(),
                new SupportMutableValueTypesCustomization())
        );

        var mockLambdaContext = autoFixture.Freeze<Mock<ILambdaContext>>();

        var function = new PwrTlzDemo.Function();

        var result = function.FunctionHandler("Hello from the test client!", mockLambdaContext.Object);

        Console.WriteLine(result);
    }
}