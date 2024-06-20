namespace Calculator.DomainLayer.Entities;

public class ParsedExpression
{
    public string OriginalExpression { get; set; }
    public List<string> ConvertedExpression { get; set; }
}