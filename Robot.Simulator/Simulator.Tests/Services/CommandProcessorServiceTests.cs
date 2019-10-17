using NUnit.Framework;
using Simulator.Models;
using Simulator.Services;
using Moq;
namespace Simulator.Tests
{
    [TestFixture]
    public class CommandProcessorServiceTests
    {
        private CommandProcessorService _commandProcessorService;
        private Mock<IRobotSimulatorService> _robotSimulatorService;
        private Mock<ICommandAnalyserService> _commandAnalyserService;
        private CommandDetails commandDetailsResponse;
        private Response _response;

        [SetUp]
        public void Setup()
        {
            _robotSimulatorService = new Mock<IRobotSimulatorService>();
            _commandAnalyserService = new Mock<ICommandAnalyserService>();
            _commandProcessorService = new CommandProcessorService(_robotSimulatorService.Object, _commandAnalyserService.Object);
        }

        [TestCase("")]
        [TestCase("place0,0,east")]
        [TestCase("plACe 0,0,invalid-direction")]
        [TestCase("PLACE 10,5,WEST")]
        [TestCase("invalidcommand")]
        public void ProcessCommand_Should_return_invalid_message_if_invalid_command_provided(string actualCommand)
        {
            // Arrange
            commandDetailsResponse = null;
            _commandAnalyserService.Setup(x => x.GetCommandDetails(It.IsAny<string>())).Returns(commandDetailsResponse);

            // Act
            var actualResult = _commandProcessorService.ProcessCommand(actualCommand);

            // Assert
            _commandAnalyserService.Verify(x => x.GetCommandDetails(It.IsAny<string>()), Times.Once);
            _robotSimulatorService.Verify(x => x.PlaceRobotOnBoard(It.IsAny<CommandDetails>()), Times.Never);
            _robotSimulatorService.Verify(x => x.MoveRobotForward(It.IsAny<Robot>()), Times.Never);
            _robotSimulatorService.Verify(x => x.ReportRobotLocation(It.IsAny<Robot>()), Times.Never);
            _robotSimulatorService.Verify(x => x.TurnRobotLeftOrRight(It.IsAny<Robot>(), It.IsAny<string>()), Times.Never);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Contains("Invalid command"));
        }

        [TestCase("move")]
        [TestCase("LEFt")]
        [TestCase("RIGHT")]
        [TestCase("RepORT")]

        public void ProcessCommand_Should_not_execute_any_command_if_robot_is_not_placed_on_board(string command)
        {
            // Arrange
            commandDetailsResponse = new CommandDetails()
            {
                CommandName = command,
            };
            _commandAnalyserService.Setup(x => x.GetCommandDetails(It.IsAny<string>())).Returns(commandDetailsResponse);

            // Act
            var actualResult = _commandProcessorService.ProcessCommand(command);

            // Assert
            _commandAnalyserService.Verify(x => x.GetCommandDetails(It.IsAny<string>()), Times.Once);
            _robotSimulatorService.Verify(x => x.PlaceRobotOnBoard(It.IsAny<CommandDetails>()), Times.Never);
            _robotSimulatorService.Verify(x => x.MoveRobotForward(It.IsAny<Robot>()), Times.Never);
            _robotSimulatorService.Verify(x => x.ReportRobotLocation(It.IsAny<Robot>()), Times.Never);
            _robotSimulatorService.Verify(x => x.TurnRobotLeftOrRight(It.IsAny<Robot>(), It.IsAny<string>()), Times.Never);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("Please place the robot on board first.\n", actualResult);
        }

        [Test]
        public void ProcessCommand_Should_execute_command_if_robot_is_not_placed_and_command_is_to_place_robot()
        {
            // Arrange
            commandDetailsResponse = new CommandDetails()
            {
                CommandName = "place",
            };

            _response = new Response()
            {
                Message = "some message"
            };

            _commandAnalyserService.Setup(x => x.GetCommandDetails(It.IsAny<string>())).Returns(commandDetailsResponse);
            _robotSimulatorService.Setup(x => x.PlaceRobotOnBoard(It.IsAny<CommandDetails>())).Returns(_response);

            // Act
            var actualResult = _commandProcessorService.ProcessCommand("place");

            // Assert
            _commandAnalyserService.Verify(x => x.GetCommandDetails(It.IsAny<string>()), Times.Once);
            _robotSimulatorService.Verify(x => x.PlaceRobotOnBoard(It.IsAny<CommandDetails>()), Times.Once);

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(_response.Message, actualResult);
        }

        [TestCase("move")]
        [TestCase("LEFT")]
        [TestCase("rIGht")]
        [TestCase("REPORT")]
        public void ProcessCommand_Should_execute_command_if_robot_is_placed_and_command_isvalid(string command)
        {
            // Arrange
            commandDetailsResponse = new CommandDetails()
            {
                CommandName = command.ToLower(),
            };

            _response = new Response()
            {
                Message = "success"
            };

            _commandAnalyserService.Setup(x => x.GetCommandDetails(It.IsAny<string>())).Returns(commandDetailsResponse);
            _commandProcessorService.Robot = new Robot() { IsPlacedOnBoard = true };

            switch (command.ToLower())
            {
                case "move":
                    _robotSimulatorService.Setup(x => x.MoveRobotForward(It.IsAny<Robot>())).Returns(_response);
                    break;
                case "left":
                case "right":
                    _robotSimulatorService.Setup(x => x.TurnRobotLeftOrRight(It.IsAny<Robot>(), It.IsAny<string>())).Returns(new Robot() { IsPlacedOnBoard = true });
                    break;
                case "report":
                    _robotSimulatorService.Setup(x => x.ReportRobotLocation(It.IsAny<Robot>())).Returns("robot location");
                    break;
                default:
                    break;
            }

            // Act
            var actualResult = _commandProcessorService.ProcessCommand(command);

            // Assert
            _commandAnalyserService.Verify(x => x.GetCommandDetails(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(actualResult);
            switch (command)
            {
                case "move":
                    _robotSimulatorService.Verify(x => x.MoveRobotForward(It.IsAny<Robot>()),Times.Once);
                    Assert.AreEqual(_response.Message, actualResult);
                    break;
                case "left":
                case "right":
                    _robotSimulatorService.Verify(x => x.TurnRobotLeftOrRight(It.IsAny<Robot>(), It.IsAny<string>()), Times.Once);
                    Assert.AreEqual(_response.Message, actualResult);
                    break;
                case "report":
                    _robotSimulatorService.Verify(x => x.ReportRobotLocation(It.IsAny<Robot>()), Times.Once);
                    Assert.AreEqual("robot location", actualResult);
                    break;
                default:
                    break;
            }
        }
    }
}