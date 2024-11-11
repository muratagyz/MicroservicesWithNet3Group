using IdentityModel.Client;

namespace keycloak.web.Services
{
    public class WeatherService(HttpClient client, ILogger<WeatherService> logger, IConfiguration configuration)
    {
        public async Task<List<WeatherForecastResponseModel>> GetWeatherForecasts()
        {         
            var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync("http://localhost:8080/realms/MyCompany/.well-known/openid-configuration");

            if (discoveryDocumentResponse.IsError)
            {
                logger.LogError(discoveryDocumentResponse.Error);
            }

            var clientId = configuration.GetSection("keycloak-web")["ClientId"];
            var clientSecret = configuration.GetSection("keycloak-web")["ClientSecret"];

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = clientId
            });

            if (tokenResponse.IsError)
            {
                logger.LogError(tokenResponse.Error);
            }

            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5130/WeatherForecast");

            if (response.IsSuccessStatusCode is false)
            {
                logger.LogError(response.ReasonPhrase);
            }

            var weatherList = await response.Content.ReadFromJsonAsync<List<WeatherForecastResponseModel>>();

            return weatherList;
        }
    }
}
