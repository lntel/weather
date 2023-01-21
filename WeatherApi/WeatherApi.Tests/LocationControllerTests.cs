using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using WeatherApi.Config;
using WeatherApi.Controllers;
using WeatherApi.Models;

namespace WeatherApi.Tests
{
    public class LocationControllerTests
    {
        private LocationController _locationController;
        private HttpClient _httpClient;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpResponseMessage _mockResponse;

        [SetUp]
        public void Setup()
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _httpMessageHandlerMock
                  .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    _mockResponse = new HttpResponseMessage();

                    _mockResponse.StatusCode = HttpStatusCode.OK;
                    _mockResponse.Content = new StringContent("[ { \"name\": \"Northampton\", \"local_names\": { \"sr\": \"Нортхамптон\", \"uk\": \"Нортгемптон\", \"en\": \"Northampton\", \"ur\": \"نارتھیمپٹن\", \"lt\": \"Nortamptonas\", \"ru\": \"Нортгемптон\" }, \"lat\": 52.2381355, \"lon\": -0.8963907, \"country\": \"GB\", \"state\": \"England\" }, { \"name\": \"Northampton\", \"local_names\": { \"en\": \"Northampton\", \"ko\": \"노샘프턴\" }, \"lat\": 42.3178989, \"lon\": -72.6311006, \"country\": \"US\", \"state\": \"Massachusetts\" }, { \"name\": \"Northampton Township\", \"local_names\": { \"en\": \"Northampton Township\" }, \"lat\": 40.20943295, \"lon\": -75.0020315130097, \"country\": \"US\", \"state\": \"Pennsylvania\" }, { \"name\": \"Town of Northampton\", \"local_names\": { \"en\": \"Town of Northampton\" }, \"lat\": 43.1884705, \"lon\": -74.1717581, \"country\": \"US\", \"state\": \"New York\" }, { \"name\": \"Northampton\", \"lat\": 40.6871831, \"lon\": -75.4896654, \"country\": \"US\", \"state\": \"Pennsylvania\" } ]");

                    return _mockResponse;
                });

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            var apiOptions = Options.Create(new ApiConfig
            {
                ApiKey = "7d89fu9fjeu9238jf98",
                BaseUrl = "http://api.openweathermap.org"
            });

            _locationController = new LocationController(apiOptions, _httpClient);
        }

        [Test]
        public async Task Get_ShouldReturnJsonResponse()
        {
            var response = await _locationController.Get("Paris");

            var json = JsonConvert.DeserializeObject<List<Location>>(_mockResponse.Content.ToString());

            Assert.That(response, Is.EqualTo(json));
        }
    }
}