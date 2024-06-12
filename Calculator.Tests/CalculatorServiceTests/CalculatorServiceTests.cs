using System.Globalization;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Services.CalculatorServices;

namespace Calculator.Tests.CalculatorServiceTests;

[Category("DevTests")]
public class CalculatorServiceTests
{
    private ICalculatorService _calculatorService;
    
    [SetUp]
    public void SetUp()
    {
        _calculatorService = new CalculatorService();
    }
    
    [Test]
    [TestCase(new[] { "2", "3", "+", "4", "*" }, "20")]
    [TestCase(new[] { "1", "1", "11", "*", "+", "5", "-" }, "7")]
    [TestCase(new[] { "1.5", "2.3", "3.7", "*", "+", "4.6", "-" }, "5.41")]
    [TestCase(new[] {
        "15", "7", "1", "1", "+", "-", "/", "3", "*", 
        "2", "1", "1", "+", "+", 
        "15", "*", "7", "200", "1", "+", "-", "/", "3", "*", "-", 
        "2", "1", "1", "+", "+", 
        "15", "7", "1", "1", "+", "-", "/", "3", "*", 
        "2", "1", "1", "+", "+", "-", 
        "15", "7", "1", "1", "+", "-", "/", "3", "*", 
        "+", "2", "1", "1", "+", "+", "-", "*", "-"
    }, "-30.0722")]
    public void Calculate_Correct(string[] postfixExpression, string expected)
    {
        var completedExpression = _calculatorService.Calculate(postfixExpression.ToList());

        Assert.That(completedExpression.ToString(CultureInfo.InvariantCulture), Is.EqualTo(expected));
    }
    
    [Test]
    public void Calculate_DivisionByZeroException()
    {
        var expression = new List<string> { "2", "0", "/" };
        Assert.Throws<DivideByZeroException>(() => _calculatorService.Calculate(expression));
    }
    
    [Test]
    public void Calculate_InvalidExpression_ThrowsException()
    {
        var expression = new List<string> { "2", "3", "+", "*" };
        Assert.Throws<ArgumentException>(() => _calculatorService.Calculate(expression));
    }
}