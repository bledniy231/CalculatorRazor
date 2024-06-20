using Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;
using Calculator.ApplicationLayer.Common.Models.ConfigurationModels;
using Calculator.ApplicationLayer.Services.Tokenizers;
using Calculator.DevTests.TestModels.Expression;
using Calculator.DevTests.TestProviders.ExpressionParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculator.DevTests.Tests.TokenizerTests;

[Category("DevTests")]
public class TokenizerTests
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
                .AddSingleton<ITokenizer, SingleCharacterTokenizer>()
                .Configure<TokenizerSettings>(ctx.Configuration.GetSection("TokenizerSettings")))
            .Build();
    }

    [TearDown]
    public void TearDown()
    {
        _host.Dispose();
    }

    [Test]
    [TestCaseSource(typeof(ExpressionTestCasesProvider), nameof(ExpressionTestCasesProvider.GetValidExpressionsForTokenizeTestCases))]
    public void TokenizeExpression_Correct(ExpressionInThreeVariantsTestCase testCase)
    {
        var tokenizer = GetTokenizer();
        var result = tokenizer.Tokenize(testCase.OriginalExpression);
        Assert.That(testCase.TokenizedExpression.SequenceEqual(result), Is.True);
    }
    
    [Test]
    [TestCaseSource(typeof(ExpressionTestCasesProvider), nameof(ExpressionTestCasesProvider.GetInvalidExpressionsForTokenizerTestCases))]
    public void TokenizeExpression_Exception(ExpressionInThreeVariantsTestCase testCase)
    {
        var tokenizer = GetTokenizer();
        Assert.Throws<ArgumentException>(() => tokenizer.Tokenize(testCase.OriginalExpression));
    }

    private ITokenizer GetTokenizer()
    {
        using var scope = _host.Services.CreateScope();
        var tokenizer = scope.ServiceProvider.GetRequiredService<ITokenizer>();
        return tokenizer;
    }
}