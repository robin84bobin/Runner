namespace Commands
{
    public class CommandManager
    {
        public static void Execute(Command command)
        {
            command.Execute();
        }

    }
}