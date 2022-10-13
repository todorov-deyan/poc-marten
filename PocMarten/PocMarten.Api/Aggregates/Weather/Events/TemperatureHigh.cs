using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Weather.Events
{
    public class TemperatureHigh : IEventState
    {
        public int TemperatureHighC { get; set; }

        public TemperatureHigh(int temperature)
        {
            TemperatureHighC = temperature;
        }
    }
}
