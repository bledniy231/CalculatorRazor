using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.UseCases;
using Calculator.ApplicationLayer.Services.CalculatorServices;
using Calculator.ApplicationLayer.Services.ExpressionParsers;
using Calculator.ApplicationLayer.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ApplicationLayer;

public static class HostExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ICalculatorService, CalculatorService>();
        services.AddSingleton<IExpressionParser, ExpressionParser>();
        services.AddSingleton<IExpressionsUseCases, ExpressionsUseCases>();
        return services;
    }
}