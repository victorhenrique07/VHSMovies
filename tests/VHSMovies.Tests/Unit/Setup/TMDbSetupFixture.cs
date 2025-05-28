using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Moq.Protected;

using Moq;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Services.Responses;
using System.Text.Json;

namespace VHSMovies.Tests.Unit.Setup
{
    public class TMDbSetupFixture
    {
        public HttpClient CreateMockedHttpClient(TitleDetailsTMDB responseContent, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            var json = JsonSerializer.Serialize(responseContent);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = httpContent
                });

            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://mocked.api/")
            };
        }
    }
}
