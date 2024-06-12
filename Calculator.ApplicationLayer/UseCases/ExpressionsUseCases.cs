using System.Globalization;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.UseCases;
using Calculator.ApplicationLayer.Common.Interfaces.InfrastructureInterfaces;
using Calculator.DomainLayer.Entities;

namespace Calculator.ApplicationLayer.UseCases;

public class ExpressionsUseCases(
    IExpressionParser parser,
    ICalculatorService calculator,
    ICalculatorRepository repository) : IExpressionsUseCases
{
    public async Task<CompletedExpression> CalculateExpression(string expression)
    {
        var completedExpression = new CompletedExpression
        {
            OriginalExpression = expression,
            DateTime = DateTime.UtcNow
        };
        
        try
        {
            var parsedModel = parser.ParseExpression(expression);
            var result = calculator.Calculate(parsedModel.PostfixExpression);
            completedExpression.Result = result.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            completedExpression.Result = e.Message;
        }
        
        await repository.SaveExpressionAsync(completedExpression);

        return completedExpression;
    }

    public async Task<IEnumerable<CompletedExpression>> GetCompletedExpressions()
    {
        return await repository.GetExpressionsAsync();
    }
}