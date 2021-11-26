namespace JC.Examples;
public static partial class GetExchangeRates
{
    [FunctionName("GetExchangeRates")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        DateOnly from = DateOnly.ParseExact((string)req.Query["from"], "yyyy-MM-dd");
        DateOnly to = DateOnly.ParseExact((string)req.Query["to"], "yyyy-MM-dd");
        string currency = (string)req.Query["currency"];
        log.LogInformation($"Getting exchange rates for {currency} from {from} to {to}.");
        var rates = await new ExchangeRateProvider().GetRates(from, to, currency);
        return new OkObjectResult(rates);
    }
}