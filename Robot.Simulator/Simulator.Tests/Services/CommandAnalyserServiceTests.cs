using NUnit.Framework;
using Simulator.Models;
using Simulator.Services;
using Moq;
namespace Simulator.Tests
{
    [TestFixture]
    public class CommandAnalyserServiceTests
    {
        private CommandAnalyserService _commandAnalyserService;
        private Mock<IValidationService> _validationService;

        [SetUp]
        public void Setup()
        {
            _validationService = new Mock<IValidationService>();
            _commandAnalyserService = new CommandAnalyserService(_validationService.Object);
        }
        
        [TestCase("place 0,0,east", "place", 0, 0, "east")]
        [TestCase("plACe 0,0,eASt", "place", 0, 0, "east")]
        [TestCase("PLACE 3,5,WEST", "place", 3, 5, "west")]
        [TestCase("place 5,5,north", "place", 5, 5, "north")]
        public void GetCommandDetails_Should_return_commandDetails_when__valid_place_command_is_provided(string actualCommand,
            string expectedCommandName, int expectedXPosition, int expectedYPosition, string expectedDirection)
        {
            // Arrange
            _validationService.Setup(x => x.IsValidCommand(It.IsAny<string>())).Returns(true);

            var expectedResult = new CommandDetails
            {
                CommandName = expectedCommandName,
                Coordinates = new Coordinates { X = expectedXPosition, Y = expectedYPosition },
                Direction = expectedDirection
            };

            // Act
            var actualResult = _commandAnalyserService.GetCommandDetails(actualCommand);

            // Assert
            _validationService.Verify(x => x.IsValidCommand(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.CommandName, actualResult.CommandName);
            Assert.AreEqual(expectedResult.Coordinates.X, actualResult.Coordinates.X);
            Assert.AreEqual(expectedResult.Coordinates.Y, actualResult.Coordinates.Y);
            Assert.AreEqual(expectedResult.Direction, actualResult.Direction);
        }

        [TestCase("move", "move")]
        [TestCase("mOVe", "move")]
        [TestCase("MOVE", "move")]
        [TestCase("anySingleWordCommand", "anysinglewordcommand")]
        public void GetCommandDetails_Should_return_commandDetails_when__valid_single_command_is_provided(string actualCommand, string expectedCommandName)
        {
            // Arrange
            _validationService.Setup(x => x.IsValidCommand(It.IsAny<string>())).Returns(true);

            var expectedResult = new CommandDetails
            {
                CommandName = expectedCommandName
            };

            // Act
            var actualResult = _commandAnalyserService.GetCommandDetails(actualCommand);

            // Assert
            _validationService.Verify(x => x.IsValidCommand(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.CommandName, actualResult.CommandName);
            Assert.AreEqual(expectedResult.Coordinates, null);
            Assert.AreEqual(expectedResult.Direction, null);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("place0,0,west")]
        [TestCase("place0,0,west")]
        [TestCase("placed 0,0,east")]
        [TestCase("place0,0,east")]
        [TestCase("place, 0,0,east")]
        [TestCase("place 0,0,east,")]
        [TestCase("place 10,0,east")]
        [TestCase("place 0,10,east")]
        [TestCase("move 0,10,east")]
        [TestCase("repo")]
        [TestCase("left move")]
        [TestCase("move right")]
        public void GetCommandDetails_Should_return_null_when__invalid_command_is_provided(string actualCommand)
        {
            // Arrange
            _validationService.Setup(x => x.IsValidCommand(It.IsAny<string>())).Returns(false);

            // Act
            var actualResult = _commandAnalyserService.GetCommandDetails(actualCommand);

            // Assert
            _validationService.Verify(x => x.IsValidCommand(It.IsAny<string>()), Times.Once);
            Assert.IsNull(actualResult);
        }
    }
}