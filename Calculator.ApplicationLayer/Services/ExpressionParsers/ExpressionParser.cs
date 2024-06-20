using System.Globalization;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.DomainLayer.Entities;

namespace Calculator.ApplicationLayer.Services.ExpressionParsers;

public class ExpressionParser(
    ITokenizer tokenizer,
    IExpressionConverter converter) : IExpressionParser
{
    public ParsedExpression ParseExpression(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            throw new ArgumentException("Выражение пустое");
        }

        expression = ChangeComasToPoints(expression);
        var tokens = tokenizer.Tokenize(expression);
        var convertedExpression = converter.Convert(tokens);
        var parsedExpression = new ParsedExpression
        {
            OriginalExpression = expression,
            ConvertedExpression = convertedExpression 
        };

        return parsedExpression;
    }

    private static string ChangeComasToPoints(string expression) => expression.Replace(",", ".", true, CultureInfo.InvariantCulture);
}