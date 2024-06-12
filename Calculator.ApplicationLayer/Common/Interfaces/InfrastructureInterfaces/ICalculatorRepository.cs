using Calculator.DomainLayer.Entities;

namespace Calculator.ApplicationLayer.Common.Interfaces.InfrastructureInterfaces;

public interface ICalculatorRepository
{
    Task SaveExpressionAsync(CompletedExpression expression);
    Task<IEnumerable<CompletedExpression>> GetExpressionsAsync();
}