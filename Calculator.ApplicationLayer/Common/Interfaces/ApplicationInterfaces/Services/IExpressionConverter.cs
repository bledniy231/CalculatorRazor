namespace Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;

public interface IExpressionConverter
{
    List<string> Convert(List<string> tokens);
}