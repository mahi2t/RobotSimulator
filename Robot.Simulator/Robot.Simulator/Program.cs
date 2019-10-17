using Microsoft.Extensions.DependencyInjection;
using Simulator.Services;
using System;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Dependcy Injection setup
            var services = new ServiceCollection();
            services.AddScoped<ICommandProcessorService, CommandProcessorService>();
            services.AddScoped<ICommandAnalyserService, CommandAnalyserService>();
            services.AddScoped<IRobotSimulatorService, RobotSimulatorService>();
            services.AddScoped<IValidationService, ValidationService>();
            var serviceProvider = services.BuildServiceProvider();
           
            //Read and process the command
            var commandProcessorService = serviceProvider.GetService<ICommandProcessorService>();
            Console.WriteLine("--------------------- ROBOT SIMULATOR -----------------------------" +
                "\nPlease place the robot on the 5 X 5 board\n");
            ReadAndProcessCommands(commandProcessorService);
        }

        private static void ReadAndProcessCommands(ICommandProcessorService commandProcessorService)
        {
            var input = Console.ReadLine();
            var response = commandProcessorService.ProcessCommand(input);
            Console.WriteLine(response);
            ReadAndProcessCommands(commandProcessorService);
        }
    }
}
