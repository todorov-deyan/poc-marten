using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Weather.Events
{
    public class TemperatureLow : IEventState
    {
        public int TemperatureLowC { get; set; }

        public TemperatureLow(int temperature)
        {
            TemperatureLowC = temperature;
        }
    }
}
