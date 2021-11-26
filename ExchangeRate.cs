namespace JC.Examples;
public readonly record struct ExchangeRate
{
    public string Currency { get; init; }
    public int Amount { get; init; }
    public DateOnly Date { get; init; }
    public decimal Rate { get; init; }
}
