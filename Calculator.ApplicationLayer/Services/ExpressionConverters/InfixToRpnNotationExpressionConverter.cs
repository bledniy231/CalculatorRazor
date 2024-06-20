using System.Globalization;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Models.ConfigurationModels;
using Microsoft.Extensions.Options;

namespace Calculator.ApplicationLayer.Services.ExpressionConverters;

/// <summary>
/// Преобразует инфиксную форму записи выражения в постфиксную (обратная польская нотация)
/// </summary>
/// <param name="settings">Входные настройки преобразователя</param>
public class InfixToRpnNotationExpressionConverter(IOptions<ConverterSettings> settings) : IExpressionConverter
{
    private readonly Dictionary<string, int> _operatorsPriority = settings.Value.OperatorPriorities;
    
    public List<string> Convert(List<string> tokens)
    {
        var postfixExpression = new List<string>();
        var operatorsStack = new Stack<string>();

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
            else if (_operatorsPriority.TryGetValue(token, out var tokenPriority))
            {
                while (operatorsStack.Count > 0 && operatorsStack.Peek() != "(" && tokenPriority <= _operatorsPriority[operatorsStack.Peek()])
                {
                    postfixExpression.Add(operatorsStack.Pop());
                }

                operatorsStack.Push(token);
            }
            else
            {
                throw new ArgumentException($"Встречен не опознанный токен: {token}");
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