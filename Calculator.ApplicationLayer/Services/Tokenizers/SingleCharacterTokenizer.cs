using System.Text;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Models.ConfigurationModels;
using Microsoft.Extensions.Options;

namespace Calculator.ApplicationLayer.Services.Tokenizers;

/// <summary>
/// Делит входное выражение на одиночные токены в этом выражении
/// </summary>
/// <param name="settings">Входные настройки токенайзера</param>
public class SingleCharacterTokenizer(IOptions<TokenizerSettings> settings) : ITokenizer
{
    private readonly string _validChars = settings.Value.ValidServiceCharacters;
    
    public List<string> Tokenize(string expression)
    {
        if (string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException("Выражение не может быть пустым");
        }
        
        var tokens = new List<string>();

        for (int i = 0; i < expression.Length; i++)
        {
            if (char.IsDigit(expression[i]))
            {
                if (i + 1 >= expression.Length)
                {
                    tokens.Add(expression[i].ToString());
                    continue;
                }

                var numberBuilder = BuildNextNumber(expression, ref i, false);
                tokens.Add(numberBuilder.ToString());
            }
            else if (char.IsWhiteSpace(expression[i])) { }
            else
            {
                if ((expression[i] == '+' || expression[i] == '-')
                    && (i == 0
                        || tokens.Count == 0
                        || (tokens[^1] != ")" && _validChars.Contains(tokens[^1][^1]))
                        )
                   )
                {
                    if (i + 1 >= expression.Length)
                    {
                        throw new ArgumentException($"Недопустимый символ в конце выражения: \"{expression[i]}\"");
                    }

                    var numberBuilder = BuildNextNumber(expression, ref i, true);
                    if (numberBuilder[0] == '+')
                    {
                        numberBuilder.Remove(0, 1);
                    }

                    tokens.Add(numberBuilder.ToString());
                    continue;
                }
                
                if (!_validChars.Contains(expression[i]))
                {
                    throw new ArgumentException($"Недопустымый символ в выражении: {expression[i]}");
                }
                
                tokens.Add(expression[i].ToString());
            }
        }

        return tokens;
    }

    private static StringBuilder BuildNextNumber(string expression, ref int originalIndexInExpression, bool isUnaryWithNumberExpected)
    {
        var numberBuilder = new StringBuilder();
        numberBuilder.Append(expression[originalIndexInExpression]);
        
        int j = originalIndexInExpression + 1;
        bool dotReached = false;

        if (isUnaryWithNumberExpected && expression[j] == '.')
        {
            throw new ArgumentException("Недопустимое расположение мантиссы после унарного оператора");
        }
        
        while (j < expression.Length && (char.IsDigit(expression[j]) || expression[j] == '.'))
        {
            switch (expression[j])
            {
                case '.' when !dotReached:
                    dotReached = true;
                    numberBuilder.Append(expression[j]);
                    break;
                case '.' when dotReached:
                    throw new ArgumentException("Выражение содержит недопустимое число с двумя или более мантиссами");
                default:
                    numberBuilder.Append(expression[j]);
                    break;
            }

            j++;
        }
        
        if (numberBuilder[^1].Equals('.'))
        {
            throw new ArgumentException("Выражение содержит недопустимое число с пустой мантиссой");
        } 

        originalIndexInExpression = j - 1;

        return numberBuilder;
    }
}