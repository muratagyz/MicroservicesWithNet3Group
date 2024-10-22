using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController(StockService stockService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStock()
        {
            var result = await stockService.GetStockCount();

            return Ok(result);
        }
    }
}
