using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;

namespace Calculator.ApplicationLayer.Services.MathematicsOperations;

public class DivisionMathematicsOperation : IMathematicsOperation
{
    public string Operator => "/";
    public double Execute(double firstOperand, double secondOperand)
    {
        if (secondOperand == 0)
        {
            throw new DivideByZeroException("Попытка деления на ноль");
        }

        return firstOperand / secondOperand;
    }
}