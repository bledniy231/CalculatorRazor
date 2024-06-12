using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Services.ExpressionParsers;

namespace Calculator.Tests.ExpressionParserTests;
    
[Category("DevTests")]
public class ExpressionParserTests
{
    private IExpressionParser _parser;

    [SetUp]
    public void SetUp()
    {
        _parser = new ExpressionParser();
    }

    [Test]
    [TestCase("(2+3)*4", new[] { "(", "2", "+", "3", ")", "*", "4" }, new[] { "2", "3", "+", "4", "*" })]
    [TestCase("1+1*11-5", new[] { "1", "+", "1", "*", "11", "-", "5" }, new[] { "1", "1", "11", "*", "+", "5", "-" })]
    [TestCase("1.5+2.3*3.7-4.6", new[] { "1.5", "+", "2.3", "*", "3.7", "-", "4.6" }, new[] { "1.5", "2.3", "3.7", "*", "+", "4.6", "-" })]
    [TestCase("1,4 + 0,3 * 2", new[] { "1.4", "+", "0.3", "*", "2" }, new[] { "1.4", "0.3", "2", "*", "+" })]
    [TestCase("15/(7-(1+1))*3-(2+(1+1))*15/(7-(200+1))*3-(2+(1+1))*(15/(7-(1+1))*3-(2+(1+1))+15/(7-(1+1))*3-(2+(1+1)))",
        new[] { "15", "/", "(", "7", "-", "(", "1", "+", "1", ")", ")", "*", "3",
            "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", "*", "15",
            "/", "(", "7", "-", "(", "200", "+", "1", ")", ")", "*", "3",
            "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", "*",
            "(", "15", "/", "(", "7", "-", "(", "1", "+", "1", ")", ")", "*", "3",
            "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", "+", "15",
            "/", "(", "7", "-", "(", "1", "+", "1", ")", ")", "*", "3", 
            "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", ")" },
        new[] {
            "15", "7", "1", "1", "+", "-", "/", "3", "*", 
            "2", "1", "1", "+", "+", 
            "15", "*", "7", "200", "1", "+", "-", "/", "3", "*", "-", 
            "2", "1", "1", "+", "+", 
            "15", "7", "1", "1", "+", "-", "/", "3", "*", 
            "2", "1", "1", "+", "+", "-", 
            "15", "7", "1", "1", "+", "-", "/", "3", "*", 
            "+", "2", "1", "1", "+", "+", "-", "*", "-"
        })]
    public void ParseExpression_Correct(string expression, string[] expectedTokens, string[] expectedPostfix)
    {
        var parsedExpression = _parser.ParseExpression(expression);

        Assert.That(expectedTokens.SequenceEqual(parsedExpression.Tokens), Is.True);
        Assert.That(expectedPostfix.SequenceEqual(parsedExpression.PostfixExpression), Is.True);
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    [TestCase("))")]
    [TestCase("((22222")]
    [TestCase("15+88/69/88.Some string")]
    [TestCase(".")]
    [TestCase(",")]
    [TestCase("1,")]
    [TestCase("1.")]
    [TestCase("15.16.56.5")]
    [TestCase("(2+3)*4)")]
    public void ParseExpression_Exception_Tokenize(string? expression)
    {
        Assert.Throws<ArgumentException>(() => _parser.ParseExpression(expression));
    }
}  