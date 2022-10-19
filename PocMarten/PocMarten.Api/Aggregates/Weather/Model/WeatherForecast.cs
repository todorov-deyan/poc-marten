using System.Drawing;
using Marten.Events;
using Newtonsoft.Json;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Weather.Model
{
    public class WeatherForecast : Aggregate
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public WeatherTemperatureStatus Status { get; set; }

        [JsonConstructor]
        private WeatherForecast()
        {
            
        }

        public WeatherForecast(TemperatureMonitoringStarted @event)
        {
            Id = Guid.NewGuid();
            TemperatureC = @event.TemperatureC;
            Status = WeatherTemperatureStatus.Init;
        }


        public void Apply(TemperatureHigh @event)
        {
            TemperatureC += @event.TemperatureHighC;
            Status = WeatherTemperatureStatus.High;
        }

        public void Apply(IEvent<TemperatureLow> @event)
        {
            TemperatureC -= @event.Data.TemperatureLowC;
            Status = WeatherTemperatureStatus.Low;
        }
    }
}