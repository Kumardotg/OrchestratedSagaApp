namespace OrchestratedSagaApp
{
    public record WeatherForecast(Guid Id, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}