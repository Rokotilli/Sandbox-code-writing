using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using SandboxTestProject.Models.HackerearthModels;
using SandboxTestProject.Services.Interfaces;
using SandboxTestProject.ViewModels;
using SandboxTestProject;


namespace SandboxTestProjectTests.Tests.Services.Successes
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
        public async Task SendCodeEvaluation_PostCodeEvaluationModel_ShouldReturnNotNullModel()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new ResponseCodeEvaluationModel
                {
                    he_id = "123456"
                }))
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
            var result = await _codeEvaluationService.SendCodeEvaluation(new PostCodeEvaluationModel());

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123456", result.he_id);

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
        public async Task GetResultCodeEvaluation_HeId_ShouldReturnNotNullModel()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new ResultCodeEvaluationModel()
                {
                    he_id = "123456",
                }))
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
            var result = await _codeEvaluationService.GetResultCodeEvaluation(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123456", result.he_id);

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
        public async Task GetOutputStringFromFile_Url_ShouldReturnStringOutput()
        {
            // Arrane
            var url = "https://he-s3.s3.amazonaws.com/media/userdata/AnonymousUser/code/eb8abf1";

            var expectedOutput = "hello world!\n";

            // Act
            var result = await _codeEvaluationService.GetOutputStringFromFile(url);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOutput, result);
        }
    }
}
