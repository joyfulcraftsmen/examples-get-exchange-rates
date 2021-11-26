namespace JC.Examples;
public class ExchangeRateProvider
{
    // TODO: move to config
    const string urlTemplate = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/vybrane.txt?od=#FROM#&do=#TO#&mena=#CURRENCY#&format=txt";
    public async Task<IEnumerable<ExchangeRate>> GetRates(DateOnly from, DateOnly to, string currency)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(currency);

        using HttpClient cli = new HttpClient();
        string url = urlTemplate
            .Replace("#FROM#", from.ToString("dd.MM.yyyy"))
            .Replace("#TO#", to.ToString("dd.MM.yyyy"))
            .Replace("#CURRENCY#", currency);

        var data = await cli.GetStringAsync(url);
        string[] records = data.Split("\n").Where(r => !string.IsNullOrWhiteSpace(r)).ToArray();
        int amount = int.Parse(records[0].Replace($"Měna: {currency}|Množství: ", ""));
        var results = new List<ExchangeRate>();

        for (var i = 2; i < records.Length; i++)
        {
            string[] record = records[i].Split('|');
            results.Add(new ExchangeRate()
            {
                Currency = currency,
                Amount = amount,
                Date = DateOnly.ParseExact(record[0], "dd.MM.yyyy"),
                Rate = decimal.Parse(record[1].Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            });
        }
        return results;
    }
}