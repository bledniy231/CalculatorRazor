using Calculator.ApplicationLayer;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.UseCases;
using Calculator.DevTests.TestModels.CalculatorService;
using Calculator.DevTests.TestProviders.Services.CalculatorService;
using Calculator.DevTests.TestProviders.UseCases.ExpressionUseCases;
using Calculator.DomainLayer.Entities;
using Calculator.InfrastructureLayer;
using Calculator.InfrastructureLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculator.DevTests.Tests.ExpressionsUseCasesTests;

[Category("DevTests")]
public class ExpressionsUseCasesTests
{
    private IHost _host;
    
    [SetUp]
    public void SetUp()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.SetBasePath(Directory.GetCurrentDirectory());
                cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((ctx, s) => s
                .ConfigureApplicationServices(ctx.Configuration)
                .AddInfrastructureServices())
            .Build();
    }

    [TearDown]
    public void TearDown()
    {
        _host.Dispose();
    }

    [Test]
    [TestCaseSource(typeof(CalculationTestCasesProvider), nameof(CalculationTestCasesProvider.GetCalculationTestCases))]
    public async Task CalculateExpression_Success(CalculationTestCase testCase)
    {
        var userCases = GetUseCases();
        var result = await userCases.CalculateExpression(testCase.OriginalExpression);
        Assert.That(result.Result, Is.EqualTo(testCase.Expected));
    }
    
    [Test]
    [TestCaseSource(typeof(ExpressionUCTestCasesProvider), nameof(ExpressionUCTestCasesProvider.GetInvalidCalculationTestCases))]
    public async Task CalculateExpression_Failed_ResultIsNotANumber(CalculationTestCase testCase)
    {
        var userCases = GetUseCases();
        var result = await userCases.CalculateExpression(testCase.OriginalExpression);
        Assert.That(double.TryParse(result.Result, out _), Is.False);
    }
    
    [Test]
    public async Task GetCompletedExpressions_Success()
    {
        var completedExpression = new CompletedExpression
        {
            OriginalExpression = "Test",
            Result = "test",
            DateTime = DateTime.UtcNow
        };
        await new CsvCalculatorRepository().SaveExpressionAsync(completedExpression);
        
        var userCases = GetUseCases();
        var result = (await userCases.GetCompletedExpressions()).ToList();
        Assert.That(result, Is.Not.Empty);
    }

    private IExpressionsUseCases GetUseCases()
    {
        using var scope = _host.Services.CreateScope();
        var useCases = scope.ServiceProvider.GetRequiredService<IExpressionsUseCases>();
        return useCases;
    }
}