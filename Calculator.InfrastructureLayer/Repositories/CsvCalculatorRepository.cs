using System.Globalization;
using Calculator.ApplicationLayer.Common.Interfaces.InfrastructureInterfaces;
using Calculator.DomainLayer.Entities;
using CsvHelper;

namespace Calculator.InfrastructureLayer.Repositories;

public class CsvCalculatorRepository : ICalculatorRepository
{
    private const string FilePath = "expressions.csv";
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public CsvCalculatorRepository()
    {
        if (File.Exists(FilePath))
        {
            return;
        }
        
        using (var writer = new StreamWriter(FilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<CompletedExpression>();
            csv.NextRecord();
        }
    }
    public async Task SaveExpressionAsync(CompletedExpression expression)
    {
        await _semaphore.WaitAsync();
        try
        {
            using var writer = new StreamWriter(FilePath, append: true);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecord(expression);
            csv.NextRecord();
            await writer.FlushAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IEnumerable<CompletedExpression>> GetExpressionsAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            using var reader = new StreamReader(FilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<CompletedExpression>();
            return records.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return [];
        }
        finally
        {
            _semaphore.Release();
        }
    }
}