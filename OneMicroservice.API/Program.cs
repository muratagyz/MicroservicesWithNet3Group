using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OneMicroservice.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, configure) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("RabbitMQ");
        configure.Host(connectionString);
    });
});

builder.Services.AddHttpClient<StockService>(x =>
{
    x.BaseAddress = new Uri(builder.Configuration.GetSection("Microservices").GetSection("Stock")["BaseUrl"]!);
});

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
