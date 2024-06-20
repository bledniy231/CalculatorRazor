using Calculator.ApplicationLayer.Common.Interfaces.InfrastructureInterfaces;
using Calculator.InfrastructureLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.InfrastructureLayer;

public static class ServiceCollectionExtensionsInfrLayer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ICalculatorRepository, CsvCalculatorRepository>();
        return services;
    }
}