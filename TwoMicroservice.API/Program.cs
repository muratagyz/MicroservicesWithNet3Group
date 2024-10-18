using MassTransit;
using TwoMicroservice.API.Consumers;
using TwoMicroservice.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedEventConsumer>();

    x.UsingRabbitMq((context, configure) =>
    {
        configure.UseMessageRetry(r =>
        {
            //r.Immediate(5);
            //r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            r.Interval(5, TimeSpan.FromSeconds(5));

            r.Handle<QueueCriticalException>();
            r.Ignore<QueueNormalException>();
        });

        configure.UseDelayedRedelivery(x => x.Intervals(TimeSpan.FromHours(1), TimeSpan.FromHours(2)));

        //configure.UseInMemoryOutbox();

        var connectionString = builder.Configuration.GetConnectionString("RabbitMQ");
        configure.Host(connectionString);


        configure.ReceiveEndpoint("email-microservice.user-created-event.queue", e =>
        {
            e.ConfigureConsumer<UserCreatedEventConsumer>(context);
        });
    });
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