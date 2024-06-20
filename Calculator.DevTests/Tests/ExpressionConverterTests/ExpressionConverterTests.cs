using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Models.ConfigurationModels;
using Calculator.ApplicationLayer.Services.ExpressionConverters;
using Calculator.DevTests.TestModels.Expression;
using Calculator.DevTests.TestProviders.ExpressionParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculator.DevTests.Tests.ExpressionConverterTests;

[Category("DevTests")]
public class ExpressionConverterTests
{
    private IHost _host;

    [SetUp]
    public void SetUp()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.SetBasePath(Directory.GetCurrentDirectory());
                cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((ctx, s) => s
                .AddSingleton<IExpressionConverter, InfixToRpnNotationExpressionConverter>()
                .Configure<ConverterSettings>(ctx.Configuration.GetSection("ConverterSettings")))
            .Build();
    }

    [TearDown]
    public void TearDown()
    {
        _host.Dispose();
    }

    [Test]
    [TestCaseSource(typeof(ExpressionTestCasesProvider), nameof(ExpressionTestCasesProvider.GetValidExpressionsTestCases))]
    public void ConvertTokenizeExpression_Correct(ExpressionInThreeVariantsTestCase testCase)
    {
        var converter = GetExpressionConverter();
        var result = converter.Convert(testCase.TokenizedExpression);
        Assert.That(testCase.ConvertedExpression.SequenceEqual(result), Is.True);
    }

    [Test]
    [TestCaseSource(typeof(ExpressionTestCasesProvider), nameof(ExpressionTestCasesProvider.GetInvalidTokenizedExpressionTestCases))]
    public void ConvertTokenizeExpression_Exception(ExpressionInThreeVariantsTestCase testCase)
    {
        var converter = GetExpressionConverter();
        Assert.Throws<ArgumentException>(() => converter.Convert(testCase.TokenizedExpression));
    }
    
    private IExpressionConverter GetExpressionConverter()
    {
        using var scope = _host.Services.CreateScope();
        var converter = scope.ServiceProvider.GetRequiredService<IExpressionConverter>();
        return converter;
    }
}