using System.Globalization;
using System.Text;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.DomainLayer.Entities;

namespace Calculator.ApplicationLayer.Services.ExpressionParsers;

public class ExpressionParser : IExpressionParser
{
    public ParsedExpression ParseExpression(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            throw new ArgumentException("Выражение пустое");
        }

        expression = ChangeComasToPoints(expression);
        var tokens = Tokenize(expression);
        var postfixExpression = ConvertToPostfix(tokens);
        var parsedExpression = new ParsedExpression
        {
            OriginalExpression = expression,
            Tokens = tokens,
            PostfixExpression = postfixExpression 
        };

        return parsedExpression;
    }

    private static string ChangeComasToPoints(string expression) => expression.Replace(",", ".", true, CultureInfo.InvariantCulture);
    
    private static List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        const string validChars = "+-*/()";

        for (int i = 0; i < expression.Length; i++)
        {
            if (char.IsDigit(expression[i]))
            {
                int j = i + 1;
                bool dotReached = false;
                if (j >= expression.Length)
                {
                    tokens.Add(expression[i].ToString());
                    continue;
                }
                
                var strBuilder = new StringBuilder();
                strBuilder.Append(expression[i]);
                while (char.IsDigit(expression[j]) || expression[j] == '.')
                {
                    switch (expression[j])
                    {
                        case '.' when !dotReached:
                            dotReached = true;
                            strBuilder.Append(expression[j]);
                            break;
                        case '.' when dotReached:
                            throw new ArgumentException("Выражение содержит недопустимое число с двумя или более мантиссами");
                        default:
                            strBuilder.Append(expression[j]);
                            break;
                    }

                    if (++j >= expression.Length)
                    {
                        break;
                    }
                }
                
                string number = strBuilder.ToString();
                if (number.Last().Equals('.'))
                {
                    throw new ArgumentException("Выражение содержит недопустимое число с пустой мантиссой");
                } 

                tokens.Add(number);
                i = j - 1;
            }
            else if (char.IsWhiteSpace(expression[i])) { }
            else
            {
                var token = expression[i].ToString();
                if (!validChars.Contains(token))
                {
                    throw new ArgumentException($"Недопустымый символ в выражении: {token}");
                }
                tokens.Add(token);
            }
        }

        return tokens;
    }

    private static List<string> ConvertToPostfix(List<string> tokens)
    {
        var postfixExpression = new List<string>();
        var operatorsStack = new Stack<string>();
        var operatorsPriority = new Dictionary<string, int>
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 }
        };

        int openBrackets = 0;

        foreach (var token in tokens)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                postfixExpression.Add(token);
            }
            else if (token == "(")
            {
                openBrackets++;
                operatorsStack.Push(token);
            }
            else if (token == ")")
            {
                openBrackets--;
                if (openBrackets < 0)
                {
                    throw new ArgumentException("Не верное количество скобок в выражении");
                }
                
                while (operatorsStack.Peek() != "(")
                {
                    postfixExpression.Add(operatorsStack.Pop());
                }
                operatorsStack.Pop();
            }
            else
            {
                while (operatorsStack.Count > 0 && operatorsStack.Peek() != "(" && operatorsPriority[token] <= operatorsPriority[operatorsStack.Peek()])
                {
                    postfixExpression.Add(operatorsStack.Pop());
                }
                operatorsStack.Push(token);
            }
        }

        if (openBrackets != 0)
        {
            throw new ArgumentException("Не верное количество скобок в выражении");
        }

        while (operatorsStack.Count > 0)
        {
            postfixExpression.Add(operatorsStack.Pop());
        }

        return postfixExpression;
    }
}