using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using System.Globalization;

namespace Calculator.ApplicationLayer.Services.CalculatorServices;

public class CalculatorService : ICalculatorService
{
    public double Calculate(List<string> postfixExpression)
    {
        var numbersStack = new Stack<double>();

        foreach (var token in postfixExpression)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                numbersStack.Push(number);
            }
            else
            {
                if (numbersStack.Count < 2)
                {
                    throw new ArgumentException("Неверное выражение в обратной польской нотации");
                }
                var rightOperand = numbersStack.Pop();
                var leftOperand = numbersStack.Pop();
                numbersStack.Push(Execute(token, leftOperand, rightOperand));
            }
        }

        if (numbersStack.Count != 1)
        {
            throw new ArgumentException("Неверное выражение в обратной польской нотации");
        }

        var result = numbersStack.Pop();
        return Math.Round(result, 4);
    }
    
    private static double Execute(string operation, double leftOperand, double rightOperand)
    {
        return operation switch
        {
            "+" => leftOperand + rightOperand,
            "-" => leftOperand - rightOperand,
            "*" => leftOperand * rightOperand,
            "/" => rightOperand == 0 
                ? throw new DivideByZeroException("Попытка деления на ноль") 
                : leftOperand / rightOperand,
            _ => throw new ArgumentException("Неизвестная операция")
        };
    }
}