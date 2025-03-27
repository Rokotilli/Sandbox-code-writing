using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TestProjectForDCT.Models.HackerearthModels;
using TestProjectForDCT.Models.HackerearthModels.GenericModels;
using TestProjectForDCT.Services.Interfaces;
using TestProjectForDCT.ViewModels;
using TestProjectForDCT.ViewModels.Core.Interfaces;

namespace TestProjectForDCT.Tests.ViewModels.Exceptions
{
    public class SandBoxViewModelTests
    {
        private Mock<ICodeEvaluationService> _mockCodeEvaluationService;
        private Mock<IServiceProvider> _mockServiceProvider;
        private IOptions<Config> _config;
        private Mock<ILogger<ISandBoxViewModel>> _mockLogger;
        private SandBoxViewModel _viewModel;

        public SandBoxViewModelTests()
        {
            _mockCodeEvaluationService = new(MockBehavior.Strict);
            _mockServiceProvider = new();
            _mockLogger = new();
            _config = Options.Create(new Config());

            _viewModel = new SandBoxViewModel
                (
                _mockCodeEvaluationService.Object,
                _mockServiceProvider.Object,
                _config,
                _mockLogger.Object
                );
        }

        [Fact]
        public void RunCodeAsync_PostCodeEvaluationModel_ShouldReturnResultText()
        {
            // Arrage
            var responseCodeEvaluationModel = new ResponseCodeEvaluationModel()
            {
                he_id = "1",
                result = new Result()
                {                    
                    compile_status = "compiling"
                }
            };

            _mockCodeEvaluationService
                .Setup(s => s.SendCodeEvaluation(It.IsAny<PostCodeEvaluationModel>()))
                .ReturnsAsync(responseCodeEvaluationModel);

            // Act
            _viewModel.RunCodeCommand.Execute(null);

            // Assert
            Assert.Equal("compiling", _viewModel.ResultText);
        }

        [Fact]
        public void CheckCodeEvaluationStatus_ShouldReturnResultTextExecuting()
        {
            // Arrage
            var resultCodeEvaluationModel = new ResultCodeEvaluationModel()
            {
                result = new Result()
                {
                    compile_status = "OK",
                    run_status = new RunStatus()
                    {
                        status = "NA",
                    }
                }
            };

            _mockCodeEvaluationService.
                Setup(s => s.GetResultCodeEvaluation(It.IsAny<string>()))
                .ReturnsAsync(resultCodeEvaluationModel);

            // Act
            _viewModel.CheckCodeEvaluationCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.ResultText);
            Assert.Equal("Compile status: OK\r\nExecuting...\r\n", _viewModel.ResultText);
        }
    }
}
