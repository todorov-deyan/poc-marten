using Marten.Events;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Weather
{
    public class WeatherForecast : Aggregate
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public WeatherTemperatureStatus Status { get; set; }


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