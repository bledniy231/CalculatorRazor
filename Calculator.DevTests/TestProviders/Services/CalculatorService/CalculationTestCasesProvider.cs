using Calculator.DevTests.TestModels.CalculatorService;

namespace Calculator.DevTests.TestProviders.Services.CalculatorService;

public static class CalculationTestCasesProvider
{
    public static IEnumerable<TestCaseData> GetCalculationTestCases()
    {
        yield return new TestCaseData(new CalculationTestCase
        {
            OriginalExpression = "(2+3)*4",
            PostfixExpression = ["2", "3", "+", "4", "*" ], 
            Expected = "20"
        });
        yield return new TestCaseData(new CalculationTestCase
        {
            OriginalExpression = "1+1*11-5",
            PostfixExpression = ["1", "1", "11", "*", "+", "5", "-" ], 
            Expected = "7"
        });
        yield return new TestCaseData(new CalculationTestCase
        {
            OriginalExpression = "1.5+2.3*3.7-4.6",
            PostfixExpression = ["1.5", "2.3", "3.7", "*", "+", "4.6", "-" ], 
            Expected = "5.41"
        });
        yield return new TestCaseData(new CalculationTestCase
        {
            OriginalExpression = "-4*-1",
            PostfixExpression = ["-4", "-1", "*"],
            Expected = "4"
        });
        yield return new TestCaseData(new CalculationTestCase
        {
            OriginalExpression = "6--8",
            PostfixExpression = ["6", "-8", "-"],
            Expected = "14"
        });
        yield return new TestCaseData(new CalculationTestCase
        {
            OriginalExpression = "15/(7-(1+1))*3-(2+(1+1))*15/(7-(200+1))*3-(2+(1+1))*(15/(7-(1+1))*3-(2+(1+1))+15/(7-(1+1))*3-(2+(1+1)))",
            PostfixExpression = [
                "15", "7", "1", "1", "+", "-", "/", "3", "*",
                "2", "1", "1", "+", "+",
                "15", "*", "7", "200", "1", "+", "-", "/", "3", "*", "-", 
                "2", "1", "1", "+", "+",
                "15", "7", "1", "1", "+", "-", "/", "3", "*", 
                "2", "1", "1", "+", "+", "-", 
                "15", "7", "1", "1", "+", "-", "/", "3", "*", 
                "+", "2", "1", "1", "+", "+", "-", "*", "-"
            ],
            Expected = "-30.0722"
        });
        yield return new TestCaseData(new CalculationTestCase
        {
            OriginalExpression = "1,4 + 0,3 * 2",
            PostfixExpression = ["1.4", "0.3", "2", "*", "+"],
            Expected = "2"
        });
    }
}
