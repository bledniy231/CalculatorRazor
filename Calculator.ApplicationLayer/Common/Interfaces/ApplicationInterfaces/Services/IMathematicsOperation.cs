namespace Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;

public interface IMathematicsOperation
{
    string Operator { get; }
    double Execute(double firstOperand, double secondOperand);
}