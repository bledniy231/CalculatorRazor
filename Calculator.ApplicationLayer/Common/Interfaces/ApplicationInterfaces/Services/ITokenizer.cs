namespace Calculator.ApplicationLayer.Common.Interfaces.ApplicationInterfaces.Services;

public interface ITokenizer
{
    List<string> Tokenize(string expression);
}