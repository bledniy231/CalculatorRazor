using System.Globalization;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Services.CalculatorServices;
using Calculator.ApplicationLayer.Services.MathematicsOperations;
using Calculator.DevTests.TestModels.CalculatorService;
using Calculator.DevTests.TestProviders.Services.CalculatorService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculator.DevTests.Tests.CalculatorServiceTests;

[Category("DevTests")]
public class CalculatorServiceTests
{
    private IHost _host;

    [SetUp]
    public void SetUp()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((ctx, s) => s
                .AddSingleton<ICalculatorService, CalculatorService>()
                .AddSingleton<IMathematicsOperation, PlusMathematicsOperation>()
                .AddSingleton<IMathematicsOperation, MinusMathematicsOperation>()
                .AddSingleton<IMathematicsOperation, DivisionMathematicsOperation>()
                .AddSingleton<IMathematicsOperation, TimesMathematicsOperation>())
            .Build();
    }

    [TearDown]
    public void TearDown()
    {
        _host.Dispose();
    }

    [Test]
    [TestCaseSource(typeof(CalculationTestCasesProvider), nameof(CalculationTestCasesProvider.GetCalculationTestCases))]
    public void Calculate_Correct(CalculationTestCase testCase)
    {
        var calculatorService = GetCalculatorService();
        var completedExpression = calculatorService.Calculate(testCase.PostfixExpression);
        Assert.That(completedExpression.ToString(CultureInfo.InvariantCulture), Is.EqualTo(testCase.Expected));
    }

    [Test]
    public void Calculate_DivisionByZeroException()
    {
        var expression = new List<string> { "2", "0", "/" };
        var calculatorService = GetCalculatorService();
        Assert.Throws<DivideByZeroException>(() => calculatorService.Calculate(expression));
    }

    [Test]
    public void Calculate_InvalidExpression_ThrowsException()
    {
        var expression = new List<string> { "2", "3", "+", "*" };
        var calculatorService = GetCalculatorService();
        Assert.Throws<ArgumentException>(() => calculatorService.Calculate(expression));
    }

    private ICalculatorService GetCalculatorService()
    {
        using var scope = _host.Services.CreateScope();
        var calculatorService = scope.ServiceProvider.GetRequiredService<ICalculatorService>();
        return calculatorService;
    }
}
