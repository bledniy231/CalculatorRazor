using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.UseCases;
using Calculator.ApplicationLayer.Services.CalculatorServices;
using Calculator.ApplicationLayer.Services.ExpressionParsers;
using Calculator.ApplicationLayer.UseCases;
using Calculator.DomainLayer.Entities;
using Calculator.InfrastructureLayer.Repositories;

namespace Calculator.Tests.ExpressionsUseCasesTests;

[Category("DevTests")]
public class ExpressionsUseCasesTests
{
    private IExpressionsUseCases userCases;
    
    [SetUp]
    public void SetUp()
    {
        userCases = new ExpressionsUseCases(new ExpressionParser(), new CalculatorService(), new CsvCalculatorRepository());   
    }

    [Test]
    [TestCase("15/(7-(1+1))*3-(2+(1+1))*15/(7-(200+1))*3-(2+(1+1))*(15/(7-(1+1))*3-(2+(1+1))+15/(7-(1+1))*3-(2+(1+1)))", "-30.0722")]
    [TestCase("1+1*11-5", "7")]
    [TestCase("(2+3)*4", "20")]
    [TestCase("1.5+2.3*3.7-4.6", "5.41")]
    [TestCase("1,4 + 0,3 * 2", "2")]
    public async Task CalculateExpression_Success(string expression, string expected)
    {
        var result = await userCases.CalculateExpression(expression);
        Assert.That(result.Result, Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase("1+*11-5")]
    [TestCase("(2+3)*4)")]
    [TestCase("1.66.")]
    [TestCase("1,994*44d")]
    [TestCase("(56+9+9+41.)+6,4")]
    public async Task CalculateExpression_Failed_ResultIsNotANumber(string expression)
    {
        var result = await userCases.CalculateExpression(expression);
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
        var result = (await userCases.GetCompletedExpressions()).ToList();
        Assert.That(result, Is.Not.Empty);
    }
}