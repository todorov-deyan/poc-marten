using Marten;
using Marten.Events.Projections;
using MediatR;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Aggregates.Invoices.Repository;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Repository;
using PocMarten.Api.Aggregates.Order.Models;
using PocMarten.Api.Aggregates.Order.Repository;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Models;
using PocMarten.Api.Aggregates.Weather.Behaviours;

namespace PocMarten.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(typeof(Program));
            builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(WeatherBehaviour<,>));

            builder.Services.AddMarten(opt =>
            {
                var connString = builder.Configuration.GetConnectionString("Postgre");
             
                opt.Connection(connString);

                opt.Projections.SelfAggregate<WeatherForecast>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<OrderModel>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<ExchangeRateDetails>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<Account>(ProjectionLifecycle.Inline);
            });
        
            builder.Services.AddScoped<WeatherRepository>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<BankAccountRepository>();
            builder.Services.AddScoped<InvoiceRepository>();
            builder.Services.AddScoped<ExchangeRateRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}