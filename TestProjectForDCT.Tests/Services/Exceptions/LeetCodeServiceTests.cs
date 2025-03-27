using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using TestProjectForDCT.Services;
using TestProjectForDCT.Services.Interfaces;

namespace TestProjectForDCT.Tests.Services.Exceptions
{
    public class LeetCodeServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHanlder;
        private Mock<ILogger<ILeetCodeService>> _mockLogger;
        private LeetCodeService _leetCodeService;

        public LeetCodeServiceTests()
        {
            _mockHttpMessageHanlder = new(MockBehavior.Strict);
            _mockLogger = new();

            var config = new Config()
            {
                LeetCode = new LeetCodeConfig()
                {
                    url = "https://leetcode.com/"
                },
            };

            _mockHttpMessageHanlder = new(MockBehavior.Strict);

            var httpClient = new HttpClient(_mockHttpMessageHanlder.Object);
            httpClient.BaseAddress = new Uri(config.LeetCode.url);

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _leetCodeService = new LeetCodeService(httpClientFactory.Object, Options.Create(config), _mockLogger.Object);
        }

        [Fact]
        public async Task GetProblemsAsync_ShouldReturnException()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
            };

            _mockHttpMessageHanlder
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                        )
                .ReturnsAsync(httpResponseMessage)
                .Verifiable();

            // Act
            var result = _leetCodeService.GetProblemsAsync;

            // Assert
            await Assert.ThrowsAsync<Exception>(result);

            _mockHttpMessageHanlder
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.AtLeastOnce(),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Fact]
        public async Task GetDetailsProblemAsync_ProblemSlugSessionTokenCsrfToken_ShouldReturnException()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
            };

            _mockHttpMessageHanlder
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage)
                .Verifiable();

            // Act
            var result = async () => await _leetCodeService.GetDetailsProblemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<Exception>(result);

            _mockHttpMessageHanlder
                .Protected()
                .Verify
                (
                    "SendAsync",
                    Times.AtLeastOnce(),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}
