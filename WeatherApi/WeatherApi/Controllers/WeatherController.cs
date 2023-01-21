using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherApi.Config;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;
        private readonly ILogger<WeatherController> _logger;
        private readonly IValidator<GeoPoint> _validator;

        public WeatherController(
            HttpClient httpClient,
            ILogger<WeatherController> logger,
            IOptions<ApiConfig> options,
            IValidator<GeoPoint> validator)
        {
            _httpClient = httpClient;
            _apiConfig = options.Value;
            _logger = logger;
            _validator = validator;
        }

        [DisableCors]
        [HttpGet]
        public async Task<WeatherForecast> Get([FromQuery] GeoPoint geoPoint)
        {
            // Validate query params and throw an exception
            await _validator.ValidateAndThrowAsync(geoPoint);

            var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/data/2.5/weather?lat={geoPoint.lat}&lon={geoPoint.lon}&appid={_apiConfig.ApiKey}");

            if(!response.IsSuccessStatusCode)
            {
                string msg = await response.Content.ReadAsStringAsync();

                throw new Exception(msg);
            }

            return await response.Content.ReadFromJsonAsync<WeatherForecast>();
        }
    }
}
