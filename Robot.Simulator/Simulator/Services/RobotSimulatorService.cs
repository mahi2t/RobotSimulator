using Simulator.Models;
using Simulator.Utils;
using System;

namespace Simulator.Services
{
    public interface IRobotSimulatorService
    {
        Response MoveRobotForward(Robot robot);
        Response PlaceRobotOnBoard(CommandDetails commandDetails);
        string ReportRobotLocation(Robot robot);
        Robot TurnRobotLeftOrRight(Robot robot, string command);
    }

    public class RobotSimulatorService : IRobotSimulatorService
    {
        private Response _response = new Response();
        private readonly IValidationService _validationService;


        public RobotSimulatorService(IValidationService validationService)
        {
            _validationService = validationService;
        }

        /// <summary>
        /// Moves robot one step forward in the direction it faces, if the move is safe.
        /// </summary>
        /// <param name="robot">Robot</param>
        public Response MoveRobotForward(Robot robot)
        {
            var direction = robot.Direction.ToLower();
            // validate if Robot is on board boundary and if it is facing towards empty space.
            if (_validationService.IsRobotAboutToFall(direction, robot.Position.X, robot.Position.Y))
            {
                _response.Message = Constants.Messages.CANNOT_MOVE;
                return _response;
            }

            switch (direction)
            {
                case Constants.Directions.EAST:
                    robot.Position.X += 1;
                    break;
                case Constants.Directions.WEST:
                    robot.Position.X -= 1;
                    break;
                case Constants.Directions.NORTH:
                    robot.Position.Y += 1;
                    break;
                case Constants.Directions.SOUTH:
                    robot.Position.Y -= 1;
                    break;
                default:
                    break;
            }
            _response.Message = Constants.Messages.ROBOT_MOVED;
            _response.Robot = robot;

            return _response;
        }

        /// <summary>
        /// Performs placing of robot in requested position if its valid.
        /// </summary>
        /// <param name="commandDetails">CommandDetails</param>
        /// <returns></returns>
        public Response PlaceRobotOnBoard(CommandDetails commandDetails)
        {
            // Validate if position is out of board boundary.
            if (!_validationService.IsValidPosition(commandDetails.Coordinates.X, commandDetails.Coordinates.Y))
            {
                _response.Message = Constants.Messages.POSITION_OUTSIDE_BOARD;
                return _response;
            }

            var robot = new Robot
            {
                Direction = commandDetails.Direction,
                Position = new Coordinates
                {
                    X = commandDetails.Coordinates.X,
                    Y = commandDetails.Coordinates.Y
                },
                IsPlacedOnBoard = true
            };
            _response.Message = Constants.Messages.ROBOT_PLACED;
            _response.Robot = robot;

            return _response;
        }

        /// <summary>
        /// Reports the position of the robot on board.
        /// </summary>
        /// <param name="robot">Robot</param>
        public string ReportRobotLocation(Robot robot)
        {
            return string.Format(Constants.Messages.REPORT_MESSAGE, robot?.Position?.X, robot?.Position?.Y, robot?.Direction?.ToUpper(), Environment.NewLine);
        }

        /// <summary>
        /// Changes the robot direction as requested.
        /// </summary>
        /// <param name="robot">Robot</param>
        /// <param name="command">Command</param>
        public Robot TurnRobotLeftOrRight(Robot robot, string command)
        {
            switch (robot.Direction)
            {
                case Constants.Directions.EAST:
                    robot.Direction = command.Equals(Constants.Commands.LEFT) ? Constants.Directions.NORTH : Constants.Directions.SOUTH;
                    break;
                case Constants.Directions.NORTH:
                    robot.Direction = command.Equals(Constants.Commands.LEFT) ? Constants.Directions.WEST : Constants.Directions.EAST;
                    break;
                case Constants.Directions.SOUTH:
                    robot.Direction = command.Equals(Constants.Commands.LEFT) ? Constants.Directions.EAST : Constants.Directions.WEST;
                    break;
                case Constants.Directions.WEST:
                    robot.Direction = command.Equals(Constants.Commands.LEFT) ? Constants.Directions.SOUTH : Constants.Directions.NORTH;
                    break;
                default:
                    break;
            }
            return robot;
        }
    }
}
