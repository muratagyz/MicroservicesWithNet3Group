using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddHttpClient<StockService>(x =>
{
    x.BaseAddress = new Uri(builder.Configuration.GetSection("Microservices").GetSection("Stock")["BaseUrl"]!);
}).AddPolicyHandler(AddRetryPolicy()).AddPolicyHandler(AddCircuitBreakerPolicy()).AddPolicyHandler(AddTimeOut());

static AsyncRetryPolicy<HttpResponseMessage> AddRetryPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(3, retry)));
}

static AsyncCircuitBreakerPolicy<HttpResponseMessage> AddCircuitBreakerPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}

static AsyncTimeoutPolicy<HttpResponseMessage> AddTimeOut()
{
    return Policy.TimeoutAsync<HttpResponseMessage>(10);
}



var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    dbContext.Database.Migrate();
//}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
