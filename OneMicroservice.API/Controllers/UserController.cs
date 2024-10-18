using Bus.Shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace OneMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IPublishEndpoint publishEndpoint) : ControllerBase
    {
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
