using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Models.ConfigurationModels;
using Calculator.ApplicationLayer.Services.ExpressionConverters;
using Calculator.ApplicationLayer.Services.ExpressionParsers;
using Calculator.ApplicationLayer.Services.MathematicsOperations;
using Calculator.ApplicationLayer.Services.Tokenizers;
using Calculator.DevTests.TestModels.Expression;
using Calculator.DevTests.TestProviders.ExpressionParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculator.DevTests.Tests.ExpressionParserTests;
    
[Category("DevTests")]
public class ExpressionParserTests
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
                .AddSingleton<IExpressionParser, ExpressionParser>()
                .AddSingleton<ITokenizer, SingleCharacterTokenizer>()
                .AddSingleton<IExpressionConverter, InfixToRpnNotationExpressionConverter>()
                .AddSingleton<IMathematicsOperation, DivisionMathematicsOperation>()
                .AddSingleton<IMathematicsOperation, TimesMathematicsOperation>()
                .Configure<TokenizerSettings>(ctx.Configuration.GetSection("TokenizerSettings"))
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
    public void ParseExpression_Correct(ExpressionInThreeVariantsTestCase testCase)
    {
        var parser = GetExpressionParser();
        var parsedExpression = parser.ParseExpression(testCase.OriginalExpression);

        Assert.That(testCase.ConvertedExpression.SequenceEqual(parsedExpression.ConvertedExpression), Is.True);
    }

    [Test]
    [TestCaseSource(typeof(ExpressionTestCasesProvider), nameof(ExpressionTestCasesProvider.GetInvalidExpressionsForExpressionParserTestCases))]
    public void ParseExpression_Exceptions(ExpressionInThreeVariantsTestCase testCase)
    {
        var parser = GetExpressionParser();
        Assert.Throws<ArgumentException>(() => parser.ParseExpression(testCase.OriginalExpression));
    }

    private IExpressionParser GetExpressionParser()
    {
        using var scope = _host.Services.CreateScope();
        var parser = scope.ServiceProvider.GetRequiredService<IExpressionParser>();
        return parser;
    }
}