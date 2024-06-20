namespace Calculator.DevTests.TestModels.CalculatorService;

public class CalculationTestCase
{
    public string OriginalExpression { get; set; }
    public List<string> PostfixExpression { get; set; }
    public string Expected { get; set; }
}
