using OneMicroservice.API.Models;

namespace OneMicroservice.API.Services
{
    public class StockService(HttpClient client)
    {
       public async Task<int> GetStockCount()
        {
            var response = await client.GetAsync("/api/Stock/GetSockCount");
            if (response.IsSuccessStatusCode)
            {
                GetStockCountResponse content = await response.Content.ReadFromJsonAsync<GetStockCountResponse>();
                return content.Count;
            }

            return 0;
        }
    }
}
