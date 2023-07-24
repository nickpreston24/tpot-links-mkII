public static class AirtableConfigurations
{
    public static void ConfigureAirtable(this IServiceCollection services)
    {
        string PAT = Environment.GetEnvironmentVariable("TPOT_PAT");
        string tpot_base_key = Environment.GetEnvironmentVariable("TPOT_BASE_KEY");

        services.AddHttpClient<IAirtableRepo, AirtableRepo>(client =>
        {
            client.BaseAddress = new Uri($"https://api.airtable.com/v0/{PAT}");
            return new AirtableRepo(client, tpot_base_key, PAT);
        });
    }
}