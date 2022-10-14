using Marten;
using Marten.Events.Projections;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Aggregates.Invoices.Repository;
using PocMarten.Api.Aggregates.Order.Models;
using PocMarten.Api.Aggregates.Order.Repository;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;

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

            builder.Services.AddMarten(opt =>
            {
                var connString = builder.Configuration.GetConnectionString("Postgre");
             
                opt.Connection(connString);

                opt.Projections.SelfAggregate<WeatherForecast>(ProjectionLifecycle.Inline);
                opt.Projections.SelfAggregate<OrderModel>(ProjectionLifecycle.Inline);
            });
        
            builder.Services.AddScoped<WeatherRepository>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<BankAccountRepository>();
            builder.Services.AddScoped<BankTransactionRepository>();
            builder.Services.AddScoped<InvoiceRepository>();

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