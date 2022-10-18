using MediatR;

namespace PocMarten.Api.Aggregates.Weather.Notifications
{

    public record TemperatureNotifyCommand(int temperature) : INotification;


    public class TemperatureSendEmailNotification : INotificationHandler<TemperatureNotifyCommand>
    {
        public Task Handle(TemperatureNotifyCommand notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }



    public class TemperatureSendSMSNotification : INotificationHandler<TemperatureNotifyCommand>
    {
        public Task Handle(TemperatureNotifyCommand notification, CancellationToken cancellationToken)
        {



            return Task.CompletedTask;
        }
    }

}
