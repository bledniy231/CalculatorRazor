using Calculator.DomainLayer.Entities;

namespace Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.UseCases;

public interface IExpressionsUseCases
{
    Task<CompletedExpression> CalculateExpression(string expression);
    Task<IEnumerable<CompletedExpression>> GetCompletedExpressions();
}