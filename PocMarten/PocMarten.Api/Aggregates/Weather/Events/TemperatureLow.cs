namespace PocMarten.Api.Aggregates.Weather.Events
{
    public class TemperatureLow
    {
        public int TemperatureLowC { get; set; }

        public TemperatureLow(int temperature)
        {
            TemperatureLowC = temperature;
        }
    }
}
