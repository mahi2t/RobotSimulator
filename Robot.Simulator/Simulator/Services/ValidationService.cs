using Simulator.Utils;
using System.Text.RegularExpressions;

namespace Simulator.Services
{
    public interface IValidationService
    {
        bool IsRobotAboutToFall(string direction, int xPosition, int yPosition);
        bool IsValidCommand(string command);
        bool IsValidPosition(int xPosition, int yPosition);
    }

    public class ValidationService : IValidationService
    {
        public bool IsRobotAboutToFall(string direction, int xPosition, int yPosition)
        {
            return (direction.Equals(Constants.Directions.EAST) && xPosition == Constants.Boundaries.X_UPPER_LIMIT) ||
                 (direction.Equals(Constants.Directions.WEST) && xPosition == Constants.Boundaries.X_LOWER_LIMIT) ||
                 (direction.Equals(Constants.Directions.NORTH) && yPosition == Constants.Boundaries.Y_UPPER_LIMIT) ||
                 (direction.Equals(Constants.Directions.SOUTH) && yPosition == Constants.Boundaries.Y_LOWER_LIMIT);
        }

        public bool IsValidCommand(string command)
        {
            return Regex.IsMatch(command, Constants.Expressions.PLACE_COMMAND_PATTERN) ||
                   Regex.IsMatch(command, Constants.Expressions.COMMANDS_PATTERN);
        }

        public bool IsValidPosition(int xPosition, int yPosition)
        {
            return xPosition <= Constants.Boundaries.X_UPPER_LIMIT &&
                  xPosition >= Constants.Boundaries.X_LOWER_LIMIT &&
                  yPosition <= Constants.Boundaries.Y_UPPER_LIMIT &&
                  yPosition >= Constants.Boundaries.Y_LOWER_LIMIT;
        }
    }
}
