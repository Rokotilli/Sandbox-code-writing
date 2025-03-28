using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SandboxTestProject;
using SandboxTestProject.Services.Interfaces;
using SandboxTestProject.ViewModels;
using SandboxTestProject.ViewModels.Core.Interfaces;

namespace SandboxTestProjectTests.Tests.ViewModels.Exceptions
{
    public class LeetCodeProblemsViewModelTests
    {
        private Mock<ILeetCodeService> _mockLeetCodeService;
        private Mock<IDetailsProblemViewModel> _mockDetailsProblemViewModel;
        private Mock<IEnteringPersonalDataViewModel> _mockEnteringPersonalDataViewModel;
        private Mock<ILogger<ILeetCodeProblemsViewModel>> _mockLogger;
        private IOptions<Config> _config;
        private LeetCodeProblemsViewModel _viewModel;

        public LeetCodeProblemsViewModelTests()
        {
            _mockLeetCodeService = new(MockBehavior.Strict);
            _mockDetailsProblemViewModel = new();
            _mockEnteringPersonalDataViewModel = new();
            _mockLogger = new();

            _config = Options.Create(new Config
            {
                LeetCode = new LeetCodeConfig
                {
                    session_token = "qwerty",
                    csrf_token = "qwerty"
                }
            });

            _viewModel = new LeetCodeProblemsViewModel(
                _mockLeetCodeService.Object,
                _mockDetailsProblemViewModel.Object,
                _mockEnteringPersonalDataViewModel.Object,
                _config,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task InitializeData_ShouldReturnNullArray()
        {
            // Arrage
            _mockLeetCodeService
                .Setup(s => s.GetProblemsAsync())
                .ThrowsAsync(new Exception());

            // Act
            await _viewModel.InitializeData();

            // Assert
            Assert.Equal(0, _viewModel.TotalPages);
            Assert.Null(_viewModel.DisplayedProblems);
        }

        [Fact]
        public void ListItemClicked_Problem_ShouldNotSwitchViewModel()
        {
            // Arrage
            _mockLeetCodeService
                .Setup(s => s.GetDetailsProblemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            _viewModel.ListItemClickedCommand.Execute(null);

            // Assert
            Assert.Null(_viewModel.CurrentViewModel);
        }
    }
}
