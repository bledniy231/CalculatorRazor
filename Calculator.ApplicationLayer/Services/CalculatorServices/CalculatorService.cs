using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using System.Globalization;

namespace Calculator.ApplicationLayer.Services.CalculatorServices;

public class CalculatorService : ICalculatorService
{
    private readonly Dictionary<string, IMathematicsOperation> _operations;

    public CalculatorService(IEnumerable<IMathematicsOperation> operations)
    {
        if (!operations.Any())
        {
            throw new ArgumentException("Не зарегистрировн не один обработчик математических операций");
        }
        
        _operations = operations.ToDictionary(o => o.Operator, o => o);
    }
    
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
                numbersStack.Push(Operate(token, leftOperand, rightOperand));
            }
        }

        if (numbersStack.Count != 1)
        {
            throw new ArgumentException("Неверное выражение в обратной польской нотации");
        }

        var result = numbersStack.Pop();
        return Math.Round(result, 4);
    }

    private double Operate(string token, double leftOperand, double rightOperand)
    {
        if (!_operations.TryGetValue(token, out var mathOperation))
        {
            throw new ArgumentException($"Неизвестный операнд: \"{token}\"");
        }
        
        return mathOperation.Execute(leftOperand, rightOperand);
    }
}