using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.UseCases;
using Calculator.ApplicationLayer.Common.Models.ConfigurationModels;
using Calculator.ApplicationLayer.Services.CalculatorServices;
using Calculator.ApplicationLayer.Services.ExpressionConverters;
using Calculator.ApplicationLayer.Services.ExpressionParsers;
using Calculator.ApplicationLayer.Services.MathematicsOperations;
using Calculator.ApplicationLayer.Services.Tokenizers;
using Calculator.ApplicationLayer.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ApplicationLayer;

public static class ServiceCollectionExtensionsAppLayer
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ICalculatorService, CalculatorService>();
        services.AddSingleton<IExpressionParser, ExpressionParser>();
        services.AddSingleton<IExpressionsUseCases, ExpressionsUseCases>();
        services.AddSingleton<IMathematicsOperation, PlusMathematicsOperation>();
        services.AddSingleton<IMathematicsOperation, MinusMathematicsOperation>();
        services.AddSingleton<IMathematicsOperation, DivisionMathematicsOperation>();
        services.AddSingleton<IMathematicsOperation, TimesMathematicsOperation>();
        services.AddSingleton<ITokenizer, SingleCharacterTokenizer>();
        services.AddSingleton<IExpressionConverter, InfixToRpnNotationExpressionConverter>();
        services.Configure<TokenizerSettings>(config.GetSection("TokenizerSettings"));
        services.Configure<ConverterSettings>(config.GetSection("ConverterSettings"));
        return services;
    }
}