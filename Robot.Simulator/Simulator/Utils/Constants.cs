namespace Simulator.Utils
{
    public static class Constants
    {
        public static class Boundaries
        {
            public const int X_LOWER_LIMIT = 0;
            public const int X_UPPER_LIMIT = 5;
            public const int Y_LOWER_LIMIT = 0;
            public const int Y_UPPER_LIMIT = 5;
        }

        public static class Commands
        {
            public const string LEFT = "left";
            public const string MOVE = "move";
            public const string PLACE = "place";
            public const string REPORT = "report";
            public const string RIGHT = "right";
        }

        public static class Directions
        {
            public const string EAST = "east";
            public const string NORTH = "north";
            public const string SOUTH = "south";
            public const string WEST = "west";
        }

        public static class Expressions
        {
            public const string PLACE_COMMAND_PATTERN = @"^(place)\s([0-9]{1},)([0-9]{1},)(east|west|north|south)$";
            public const string COMMANDS_PATTERN = "^(move|report|left|right)$";
        }

        public static class Messages
        {
            public const string COMMAND_EXECUTED = "Command executed successfuly.\n";
            public const string COMMAND_FAILED = "Failed to execute the command.\n";
            public const string CANNOT_MOVE = "Can not move, Robot will fall down.\n";
            public const string INVALID_COMMAND_SHOW_COMMAND_DETAILS = "\nInvalid command\n" +
                "\n+-------------------------------------------+" +
                "\n|  Valid commands                           |" +
                "\n|-------------------------------------------|" +
                "\n|  PLACE X,Y,DIRECTION (eg: PLACE 0,0,EAST) |" +
                "\n|  MOVE\t\t\t\t\t    |" +
                "\n|  LEFT\t\t\t\t\t    |" +
                "\n|  RIGHT\t\t\t\t    |" +
                "\n|  REPORT\t\t\t\t    |" +
                "\n+-------------------------------------------+\n";

            public const string PLACE_ROBOT_FIRST = "Please place the robot on board first.\n";
            public const string POSITION_OUTSIDE_BOARD = "Cannot place robot outside of the board, please provide a valid input\n";
            public const string REPORT_MESSAGE = "Robot is located at: {0},{1},{2}{3}";
            public const string ROBOT_MOVED = "Command executed, robot moved one step ahead.\n";
            public const string ROBOT_PLACED = "Robot is successfuly placed on the board.\n";
        }
    }
}

