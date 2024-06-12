namespace Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;

public interface ICalculatorService
{
    double Calculate(List<string> postfixExpression);
}