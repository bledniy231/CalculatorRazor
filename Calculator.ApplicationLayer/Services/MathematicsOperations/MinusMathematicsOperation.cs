using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;

namespace Calculator.ApplicationLayer.Services.MathematicsOperations;

public class MinusMathematicsOperation : IMathematicsOperation
{
    public string Operator => "-";

    public double Execute(double firstOperand, double secondOperand)
        => firstOperand - secondOperand;
}