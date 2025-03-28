using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using SandboxTestProject.Models.HackerearthModels;
using SandboxTestProject.Services.Interfaces;
using SandboxTestProject.ViewModels;
using SandboxTestProject;

namespace SandboxTestProjectTests.Tests.Services.Exceptions
{
    public class CodeEvaluationServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHanlder;
        private Mock<ILogger<ICodeEvaluationService>> _mockLogger;
        private CodeEvaluationService _codeEvaluationService;

        public CodeEvaluationServiceTests()
        {
            _mockHttpMessageHanlder = new(MockBehavior.Strict);
            _mockLogger = new();

            var config = new Config()
            {
                HackerEarth = new HackerEarthConfig()
                {
                    client_secret = "somesecret",
                    url = "https://api.hackerearth.com/v4/partner/code-evaluation/submissions/",
                    httpClientName = "HackerEarth"
                },
            };

            _mockHttpMessageHanlder = new(MockBehavior.Strict);            

            var httpClient = new HttpClient(_mockHttpMessageHanlder.Object);
            httpClient.BaseAddress = new Uri(config.HackerEarth.url);

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _codeEvaluationService = new CodeEvaluationService(httpClientFactory.Object, Options.Create(config), _mockLogger.Object);
        }

        [Fact]
        public async Task SendCodeEvaluation_PostCodeEvaluationModel_ShouldReturnException()
        {
            // Arrane
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
            var action = async () => await _codeEvaluationService.SendCodeEvaluation(new PostCodeEvaluationModel());

            // Assert
            await Assert.ThrowsAsync<Exception>(action);

            _mockHttpMessageHanlder
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.AtLeastOnce(),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                );
        }        

        [Fact]
        public async Task GetResultCodeEvaluation_HeId_ShouldReturnException()
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
            var result = async () => await _codeEvaluationService.GetResultCodeEvaluation(It.IsAny<string>());

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
        public async Task GetOutputStringFromFile_Url_ShouldReturnException()
        {
            // Arrane
            var url = "https://he-s3.s3.amazonaws.com/media/userdata";

            // Act
            var result = async () => await _codeEvaluationService.GetOutputStringFromFile(url);

            // Assert
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task GetOutputStringFromFile_Url_ShouldReturnInvalidOperationException()
        {
            // Arrane
            var url = "";

            // Act
            var result = async () => await _codeEvaluationService.GetOutputStringFromFile(url);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(result);
        }
    }
}
