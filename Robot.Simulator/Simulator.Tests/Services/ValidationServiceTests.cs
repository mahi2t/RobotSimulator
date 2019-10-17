using NUnit.Framework;
using Simulator.Services;
namespace Simulator.Tests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        private ValidationService _validationService;
        [SetUp]
        public void Setup()
        {
            _validationService = new ValidationService();
        }

        [TestCase("south", 0, 0, true)]
        [TestCase("south", 5, 0, true)]
        [TestCase("north", 0, 5, true)]
        [TestCase("north", 5, 5, true)]
        [TestCase("west", 0, 5, true)]
        [TestCase("west", 0, 0, true)]
        [TestCase("east", 5, 0, true)]
        [TestCase("east", 5, 5, true)]
        [TestCase("east", 4, 0, false)]
        [TestCase("west", 4, 0, false)]
        [TestCase("west", 5, 5, false)]
        [TestCase("north", 0, 4, false)]
        [TestCase("south", 0, 1, false)]
        [TestCase("south", 5, 1, false)]
        [TestCase("south", 5, 5, false)]
        public void IsRobotAboutToFall_Should_return_true_or_false_based_on_robot_current_position(string direction, int xPosition, int yPosition, bool expectedResult)
        {
            // Arrange

            // Act
            var actualResult = _validationService.IsRobotAboutToFall(direction, xPosition, yPosition);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestCase("place 0,0,east", true)]
        [TestCase("place 9,9,east", true)]
        [TestCase("place 5,5,west", true)]
        [TestCase("place 0,0", false)]
        [TestCase("move", true)]
        [TestCase("left", true)]
        [TestCase("right", true)]
        [TestCase("report", true)]
        [TestCase("placed 0,0,east", false)]
        [TestCase("place0,0,east", false)]
        [TestCase("place, 0,0,east", false)]
        [TestCase("place 0,0,east,", false)]
        [TestCase("place 10,0,east", false)]
        [TestCase("place 0,10,east", false)]
        [TestCase("move 0,10,east", false)]
        [TestCase(" ", false)]
        [TestCase("moved", false)]
        [TestCase("something", false)]
        [TestCase("repo", false)]
        [TestCase("left move", false)]
        [TestCase("move right", false)]
        public void IsValidCommand_Should_return_true_or_false_based_on_input(string command, bool expectedResult)
        {
            // Arrange

            // Act
            var actualResult = _validationService.IsValidCommand(command);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(0,0, true)]
        [TestCase(5, 5, true)]
        [TestCase(0,5, true)]
        [TestCase(0,6, false)]
        [TestCase(6,0,false)]
        [TestCase(9, 9, false)]
        public void IsValidPosition_Should_return_true_or_false_based_on_input(int xPosition, int yPosition, bool expectedResult)
        {
            // Arrange

            // Act
            var actualResult = _validationService.IsValidPosition(xPosition, yPosition);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}