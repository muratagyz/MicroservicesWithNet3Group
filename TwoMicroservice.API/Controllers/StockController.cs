using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TwoMicroservice.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetSockCount()
        {
            return Ok(new { Count = 100 });
        }
    }
}
