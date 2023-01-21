using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using WeatherApi.Config;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly ApiConfig _apiConfig;
        private readonly HttpClient _httpClient;
        public LocationController(IOptions<ApiConfig> apiConfig, HttpClient httpClient)
        {
            _apiConfig = apiConfig.Value;
            _httpClient = httpClient;
        }

        [DisableCors]
        [HttpGet("{location}")]
        public async Task<List<Location>> Get(string location)
        {
            var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/geo/1.0/direct?q={location}&limit=5&appid={_apiConfig.ApiKey}");

            if (!response.IsSuccessStatusCode)
            {
                string msg = await response.Content.ReadAsStringAsync();

                throw new Exception(msg);
            }

            return await response.Content.ReadFromJsonAsync<List<Location>>();
        }
    }
}
