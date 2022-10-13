using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Weather.Events
{
    public class TemperatureMonitoringStarted : IEventState
    {
        public int TemperatureC { get; set; }

        public TemperatureMonitoringStarted(int temperature)
        {
            TemperatureC = temperature;
        }
    }

}
