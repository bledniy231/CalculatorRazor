using Calculator.DomainLayer.Entities;

namespace Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;

public interface IExpressionParser
{
    ParsedExpression ParseExpression(string expression);
}