namespace Calculator.DomainLayer.Entities;

public class CompletedExpression
{
    public string OriginalExpression { get; set; }
    public string Result { get; set; }
    public DateTime DateTime { get; set; }
}