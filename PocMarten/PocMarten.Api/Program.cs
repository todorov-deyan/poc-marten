using FluentValidation;
using FluentValidation.AspNetCore;
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
using PocMarten.Api.Aggregates.Invoices.Behaviours;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.BankAccount.Behaviours;

namespace PocMarten.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //MediatR
            builder.Services.AddMediatR(typeof(Program));
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(BankAccountValidationBehavior<,>));
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(WeatherLoggingBehaviour<,>));
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(InvoiceBehaviour<,>));

            //MartenDB
            builder.Services.AddMarten(opt =>
            {
                var connString = builder.Configuration.GetConnectionString("Postgre");
             
                opt.Connection(connString);

                opt.Projections.SelfAggregate<WeatherForecast>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<InvoiceModel>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<OrderModel>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<ExchangeRateDetails>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<Account>(ProjectionLifecycle.Inline);
            });
            
            //Repositories
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