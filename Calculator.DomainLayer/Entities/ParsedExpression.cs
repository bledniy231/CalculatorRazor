namespace Calculator.DomainLayer.Entities;

public class ParsedExpression
{
    public string OriginalExpression { get; set; }
    public List<string> Tokens { get; set; }
    public List<string> PostfixExpression { get; set; }
}