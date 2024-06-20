using Calculator.DevTests.TestModels.Expression;

namespace Calculator.DevTests.TestProviders.ExpressionParser;

public static class ExpressionTestCasesProvider
{
    public static IEnumerable<TestCaseData> GetValidExpressionsTestCases()
    {
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "(2+3)*+4",
            TokenizedExpression = ["(", "2", "+", "3", ")", "*", "4" ],
            ConvertedExpression = [ "2", "3", "+", "4", "*" ]
        });

        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "1+1*-11-5",
            TokenizedExpression = [ "1", "+", "1", "*", "-11", "-", "5" ],
            ConvertedExpression = [ "1", "1", "-11", "*", "+", "5", "-" ]
        });

        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "1.5+2.3*3.7-4.6",
            TokenizedExpression = [ "1.5", "+", "2.3", "*", "3.7", "-", "4.6" ],
            ConvertedExpression = [ "1.5", "2.3", "3.7", "*", "+", "4.6", "-" ]
        });

        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "1,4 + 0,3 * 2",
            TokenizedExpression = [ "1.4", "+", "0.3", "*", "2" ],
            ConvertedExpression = [ "1.4", "0.3", "2", "*", "+" ]
        });

        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "15/(7-(1+1))*3-(2+(1+1))*15/(7-(200+1))*3-(2+(1+1))*(15/(7-(1+1))*3-(2+(1+1))+15/(7-(1+1))*3-(2+(1+1)))",
            TokenizedExpression = [
                "15", "/", "(", "7", "-", "(", "1", "+", "1", ")", ")", "*", "3",
                "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", "*", "15",
                "/", "(", "7", "-", "(", "200", "+", "1", ")", ")", "*", "3",
                "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", "*",
                "(", "15", "/", "(", "7", "-", "(", "1", "+", "1", ")", ")", "*", "3",
                "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", "+", "15",
                "/", "(", "7", "-", "(", "1", "+", "1", ")", ")", "*", "3", 
                "-", "(", "2", "+", "(", "1", "+", "1", ")", ")", ")" 
            ],
            ConvertedExpression = [
                "15", "7", "1", "1", "+", "-", "/", "3", "*", 
                "2", "1", "1", "+", "+", 
                "15", "*", "7", "200", "1", "+", "-", "/", "3", "*", "-", 
                "2", "1", "1", "+", "+", 
                "15", "7", "1", "1", "+", "-", "/", "3", "*", 
                "2", "1", "1", "+", "+", "-", 
                "15", "7", "1", "1", "+", "-", "/", "3", "*", 
                "+", "2", "1", "1", "+", "+", "-", "*", "-"
            ]
        });
    }

    public static IEnumerable<TestCaseData> GetValidExpressionsForTokenizeTestCases()
    {
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "(2+3)*+4",
            TokenizedExpression = ["(", "2", "+", "3", ")", "*", "4" ],
            ConvertedExpression = [ "2", "3", "+", "4", "*" ]
        });

        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "1+1*-11-5",
            TokenizedExpression = [ "1", "+", "1", "*", "-11", "-", "5" ],
            ConvertedExpression = [ "1", "1", "-11", "*", "+", "5", "-" ]
        });

        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            OriginalExpression = "1.5+2.3*3.7-4.6",
            TokenizedExpression = [ "1.5", "+", "2.3", "*", "3.7", "-", "4.6" ],
            ConvertedExpression = [ "1.5", "2.3", "3.7", "*", "+", "4.6", "-" ]
        });
    }
    
    public static IEnumerable<TestCaseData> GetInvalidExpressionsForExpressionParserTestCases()
    {
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "))" });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "((22222" });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "." });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "1." });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "15.16.56.5" });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "(2+3)*4)" });
    }

    public static IEnumerable<TestCaseData> GetInvalidExpressionsForTokenizerTestCases()
    {
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "" });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = null });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = " " });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "15+88/69/88.Some string" });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "," });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase { OriginalExpression = "1," });
    }
    
    public static IEnumerable<TestCaseData> GetInvalidTokenizedExpressionTestCases()
    {
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            TokenizedExpression = [ "1", "+", "*", "(", "f", "11", "-", "5" ]
        });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            TokenizedExpression = [ "(", "2", "+", "3", ")", "*", "4", ")" ]
        });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            TokenizedExpression = [ "1.66", ".," ]
        });
        yield return new TestCaseData(new ExpressionInThreeVariantsTestCase
        {
            TokenizedExpression = [ "(", "(", "1,994", "*", "44d", ")" ]
        });
    }
}