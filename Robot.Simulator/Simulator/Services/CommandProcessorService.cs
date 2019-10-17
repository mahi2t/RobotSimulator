using Simulator.Models;
using Simulator.Utils;

namespace Simulator.Services
{
    public interface ICommandProcessorService
    {
        string ProcessCommand(string input);
    }
    public class CommandProcessorService : ICommandProcessorService
    {
        private readonly IRobotSimulatorService _robotSimulatorService;
        private readonly ICommandAnalyserService _commandAnalyserService;
      
        public Robot Robot { get; set; }

        public CommandProcessorService(IRobotSimulatorService robotSimulatorService,
            ICommandAnalyserService commandAnalyserService)
        {
            _robotSimulatorService = robotSimulatorService;
            _commandAnalyserService = commandAnalyserService;
        }

        public string ProcessCommand(string input)
        {           
            Response response = null;
           
            var commandDetails = _commandAnalyserService.GetCommandDetails(input);
            if (commandDetails == null) { 
                return Constants.Messages.INVALID_COMMAND_SHOW_COMMAND_DETAILS;
            }

            var commandName = commandDetails.CommandName;

            // Robot is not placed and command is not to 'PLACE' the robot, then invalid command.
            if ((Robot == null || !Robot.IsPlacedOnBoard) && !commandName.Equals(Constants.Commands.PLACE))
            {
                return Constants.Messages.PLACE_ROBOT_FIRST;
            }

            switch (commandName)
            {
                case Constants.Commands.MOVE:
                    response = _robotSimulatorService.MoveRobotForward(Robot);
                    break;
                case Constants.Commands.PLACE:
                    response = _robotSimulatorService.PlaceRobotOnBoard(commandDetails);
                    break;
                case Constants.Commands.REPORT:
                    return _robotSimulatorService.ReportRobotLocation(Robot);
                case Constants.Commands.LEFT:
                case Constants.Commands.RIGHT:
                    Robot = _robotSimulatorService.TurnRobotLeftOrRight(Robot, commandName);
                    return Robot != null ? Constants.Messages.COMMAND_EXECUTED : Constants.Messages.COMMAND_FAILED;               
                default:
                    break ;
            }

            Robot = response?.Robot;
            return response?.Message;
        }
    }
}
