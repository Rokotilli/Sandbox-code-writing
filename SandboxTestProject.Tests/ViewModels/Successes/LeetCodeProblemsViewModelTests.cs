using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SandboxTestProject;
using SandboxTestProject.Models.LeetCodeModels;
using SandboxTestProject.Services.Interfaces;
using SandboxTestProject.ViewModels;
using SandboxTestProject.ViewModels.Core.Interfaces;

namespace SandboxTestProjectTests.Tests.ViewModels.Successes
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
        public async Task InitializeData_ShouldReturnAllProblems()
        {
            // Arrage
            var problems = new GetProblemsModel()
            {
                stat_status_pairs = new List<StatStatusPairs>()
                {
                    new StatStatusPairs()
                    {
                        stat = new Stat(){ question__title = "Problem 1" }
                    },
                    new StatStatusPairs()
                    {
                        stat = new Stat(){ question__title = "Problem 2" }
                    }
                }
            };

            _mockLeetCodeService
                .Setup(s => s.GetProblemsAsync())
                .ReturnsAsync(problems);

            // Act
            await _viewModel.InitializeData();

            // Assert
            Assert.Equal(1, _viewModel.TotalPages);
            Assert.Equal(2, _viewModel.DisplayedProblems.Count);
        }

        [Fact]
        public void ListItemClicked_Problem_ShouldReturnDetailsViewModel()
        {
            // Arrage
            var problem = new StatStatusPairs()
            {
                stat = new Stat()
                {
                    question_id = "1"                    
                }                
            };

            var detailsProblem = new DetailsProblemModel()
            {
                data = new Data()
                {
                    question = new Question()
                    {
                        questionId = "1",
                        content = "1"
                    }
                }
            };

            _mockLeetCodeService
                .Setup(s => s.GetDetailsProblemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(detailsProblem);

            // Act
            _viewModel.ListItemClickedCommand.Execute(problem);

            // Assert
            Assert.IsAssignableFrom<IDetailsProblemViewModel>(_viewModel.CurrentViewModel);
        }

        [Fact]
        public void ListItemClicked_Problem_ShouldReturnEnteringDataViewModel()
        {
            // Arrage
            var problem = new StatStatusPairs()
            {
                stat = new Stat()
                {
                    question_id = "1"
                }
            };

            var detailsProblem = new DetailsProblemModel()
            {
                data = new Data()
                {
                    question = new Question()
                    {
                        questionId = "1"
                    }
                }
            };

            _mockLeetCodeService
                .Setup(s => s.GetDetailsProblemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(detailsProblem);

            // Act
            _viewModel.ListItemClickedCommand.Execute(problem);

            // Assert
            Assert.IsAssignableFrom<IEnteringPersonalDataViewModel>(_viewModel.CurrentViewModel);
        }
    }
}
