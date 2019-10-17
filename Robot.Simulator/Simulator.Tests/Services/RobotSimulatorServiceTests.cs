using Moq;
using NUnit.Framework;
using Simulator.Models;
using Simulator.Services;

namespace Simulator.Tests
{
    [TestFixture]
    public class RobotSimulatorServiceTests
    {
        private RobotSimulatorService _robotSimulatorService;
        private Mock<IValidationService> _validationService;
        private Robot _request;
        private Response _response;

        [SetUp]
        public void Setup()
        {
            _validationService = new Mock<IValidationService>();
            _robotSimulatorService = new RobotSimulatorService(_validationService.Object);
        }

        [TestCase(0, 0, "west")]
        [TestCase(5, 5, "east")]
        [TestCase(0, 0, "south")]
        [TestCase(0, 5, "north")]
        public void MoveRobotForward_Should_not_move_robot_if_invalid_command(int xPosition, int yPosition, string direction)
        {
            // Arrange
            _validationService.Setup(x => x.IsRobotAboutToFall(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            _response = new Response
            {
                Message = "Can not move, Robot will fall down.\n"
            };

            // Act
            var actualResult = _robotSimulatorService.MoveRobotForward(_request);

            // Assert
            _validationService.Verify(x => x.IsRobotAboutToFall(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual(_response.Message, actualResult.Message);
        }


        [TestCase(0, 0, "east")]
        [TestCase(1, 5, "west")]
        [TestCase(0, 4, "north")]
        [TestCase(1, 2, "south")]
        public void MoveRobotForward_Should_move_robot_if_valid_command(int xPosition, int yPosition, string direction)
        {
            // Arrange
            _validationService.Setup(x => x.IsRobotAboutToFall(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            _request = new Robot()
            {
                Direction = direction,
                Position = new Coordinates
                {
                    X = xPosition,
                    Y = yPosition
                }
            };

            _response = new Response
            {
                Message = "Command executed, robot moved one step ahead.\n"
            };

            // Act
            var actualResult = _robotSimulatorService.MoveRobotForward(_request);

            // Assert
            switch (direction)
            {
                case "east":
                    Assert.AreEqual(actualResult.Robot.Position.X, xPosition + 1);
                    break;
                case "west":
                    Assert.AreEqual(actualResult.Robot.Position.X, xPosition - 1);
                    break;
                case "north":
                    Assert.AreEqual(actualResult.Robot.Position.Y, yPosition + 1);
                    break;
                case "south":
                    Assert.AreEqual(actualResult.Robot.Position.Y, yPosition - 1);
                    break;
                default:
                    break;
            }
            _validationService.Verify(x => x.IsRobotAboutToFall(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual(_response.Message, actualResult.Message);
        }

        [Test]
        public void PlaceRobotOnBoard_Should_place_robot_on_board_if_valid_command()
        {
            // Arrange
            _validationService.Setup(x => x.IsValidPosition(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            var request = new CommandDetails()
            {
                Coordinates = new Coordinates
                {
                    X = 1,
                    Y = 2
                },
                Direction = "east"
            };

            var robot = new Robot()
            {
                Direction = "east",
                Position = new Coordinates
                {
                    X = 1,
                    Y = 2
                }
            };

            _response = new Response
            {
                Robot = robot,
                Message = "Robot is successfuly placed on the board.\n"
            };

            // Act
            var actualResult = _robotSimulatorService.PlaceRobotOnBoard(request);

            // Assert
            _validationService.Verify(x => x.IsValidPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(_response.Message, actualResult.Message);
            Assert.AreEqual(_response.Robot.Direction, actualResult.Robot.Direction);
            Assert.AreEqual(_response.Robot.Position.X, actualResult.Robot.Position.X);
            Assert.AreEqual(_response.Robot.Position.Y, actualResult.Robot.Position.Y);
        }

        [Test]
        public void PlaceRobotOnBoard_Should_not_place_robot_on_board_if_invalid_command()
        {
            // Arrange
            _validationService.Setup(x => x.IsValidPosition(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            _response = new Response
            {
                Message = "Cannot place robot outside of the board, please provide a valid input\n"
            };

            var request = new CommandDetails()
            {
                Coordinates = new Coordinates
                {
                    X = 10,
                    Y = 3
                },
                Direction = "east"
            };
            // Act
            var actualResult = _robotSimulatorService.PlaceRobotOnBoard(request);

            // Assert
            _validationService.Verify(x => x.IsValidPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.IsNull(actualResult.Robot);
            Assert.AreEqual(_response.Message, actualResult.Message);
        }

        [TestCase(0, 0, "east")]
        [TestCase(2, 3, "west")]
        [TestCase(5, 1, "north")]
        public void ReportRobotLocation_Should_return_robot_coordications_and_direction(int xPosition, int yPosition, string direction)
        {
            // Arrange
            _request = new Robot()
            {
                Direction = direction,
                Position = new Coordinates
                {
                    X = xPosition,
                    Y = yPosition
                }
            };
            // Act
            var actualResult = _robotSimulatorService.ReportRobotLocation(_request);

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("Robot is located at: " + xPosition + "," + yPosition + "," + direction.ToUpper() + "\r\n", actualResult);
        }

        [TestCase("east", "left", "north")]
        [TestCase("north", "left", "west")]
        [TestCase("west", "left", "south")]
        [TestCase("south", "left", "east")]
        [TestCase("east", "right", "south")]
        [TestCase("south", "right", "west")]
        [TestCase("west", "right", "north")]
        [TestCase("north", "right", "east")]
        public void TurnRobotLeftOrRight_Should_turn_robot_left_or_right_based_on_input(string direction, string command, string expectedDirection)
        {
            // Arrange
            _request = new Robot()
            {
                Direction = direction
            };
            // Act
            var actualResult = _robotSimulatorService.TurnRobotLeftOrRight(_request, command);

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedDirection, actualResult.Direction);
        }
    }
}