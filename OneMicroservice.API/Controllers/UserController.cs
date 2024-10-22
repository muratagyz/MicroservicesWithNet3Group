using Bus.Shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OneMicroservice.API.Services;

namespace OneMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IPublishEndpoint publishEndpoint,StockService stockService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStock()
        {
            var result = await stockService.GetStockCount();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            //Outbox design
            //Inbox design

            //transaction begin
            // user to create; Sql server
            //Outbox(created,message paylaod, status)
            //transcation end

            // Retry => count,timeout

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(60));

            await publishEndpoint.Publish(new UserCreatedEvent(Guid.NewGuid(), "muratagyuz@outlook.com", "555 555 55 55"),
                pipile =>
                {
                    pipile.SetAwaitAck(true);
                    pipile.Durable = true;
                }, cancellationTokenSource.Token);

            return Ok();
        }
    }
}
