using Calculator.DevTests.TestModels.CalculatorService;

namespace Calculator.DevTests.TestProviders.UseCases.ExpressionUseCases;

public static class ExpressionUCTestCasesProvider
{
    public static IEnumerable<TestCaseData> GetInvalidCalculationTestCases()
    {
        yield return new TestCaseData(new CalculationTestCase { OriginalExpression = "1+*11-5" });
        yield return new TestCaseData(new CalculationTestCase { OriginalExpression = "(2+3)*4)" });
        yield return new TestCaseData(new CalculationTestCase { OriginalExpression = "1.66." });
        yield return new TestCaseData(new CalculationTestCase { OriginalExpression = "1,994*44d" });
        yield return new TestCaseData(new CalculationTestCase { OriginalExpression = "(56+9+9+41.)+6,4" });
    }
}