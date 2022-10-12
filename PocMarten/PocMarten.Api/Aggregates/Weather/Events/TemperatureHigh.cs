namespace PocMarten.Api.Aggregates.Weather.Events
{
    public class TemperatureHigh
    {
        public int TemperatureHighC { get; set; }

        public TemperatureHigh(int temperature)
        {
            TemperatureHighC = temperature;
        }
    }
}
