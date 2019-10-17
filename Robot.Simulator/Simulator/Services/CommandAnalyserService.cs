using Simulator.Models;

namespace Simulator.Services
{
    public interface ICommandAnalyserService
    {
        CommandDetails GetCommandDetails(string command);
    }
    public class CommandAnalyserService : ICommandAnalyserService
    {
        private readonly IValidationService _validationService;

        public CommandAnalyserService(IValidationService validationService) 
        {
            _validationService = validationService;
        }
        public CommandDetails GetCommandDetails(string command)
        {
            command = command?.Trim().ToLower();
            if (!_validationService.IsValidCommand(command))
            {
                return null;
            }

            if (command.IndexOf(',') == -1)
            {
                return new CommandDetails { CommandName = command };
            }

            var commands = command.Split(',');
            
            var subCommandSplit = commands[0].Split(' ');
            var commandDetails = new CommandDetails
            {
                CommandName = subCommandSplit[0],
                Coordinates = new Coordinates
                {
                    X = int.Parse(subCommandSplit[1]),
                    Y = int.Parse(commands[1])
                },
                Direction = commands[2]
            };

            return commandDetails;
        }
    }
}
