using System.Net.Http;
using System.Text.Json;
using System.Text;
using OrderService.Dtos;

namespace OrderService.ProductClient
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _httpClient;
        public ProductServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ProductServiceClient");
        }

        public async Task<ProductDto?> GetProductAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"api/Products/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var productJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDto>(productJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantityChange)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { quantity = quantityChange }),
            Encoding.UTF8,
            "application/json");

            var response = await _httpClient.PutAsync($"api/Products/{productId}/stock", content);
            return response.IsSuccessStatusCode;
        }


    }
}
