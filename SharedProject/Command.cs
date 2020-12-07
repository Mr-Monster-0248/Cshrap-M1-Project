using System;
using System.Text.Json;
using SharedProject.CommandUtils;
using SharedProject.Exceptions;

namespace SharedProject
{
    public class Command
    {
        private CommandString _prefix;

        public string Prefix
        {
            get
            {
                return _prefix switch
                {
                    CommandString.Register => CommandString.Register.ToString().ToLower(),
                    CommandString.Join => CommandString.Join.ToString().ToLower(),
                    CommandString.Login => CommandString.Login.ToString().ToLower(),
                    CommandString.Send => CommandString.Send.ToString().ToLower(),
                    CommandString.List => CommandString.List.ToString().ToLower(),
                    CommandString.CreateTopic => CommandString.CreateTopic.ToString().ToLower(),
                    _ => "error",
                };
            }

            set
            {
                try
                {
                    _prefix = Enum.Parse<CommandString>(value);
                }
                catch
                {
                    Console.WriteLine("Wrong prefix command, assigning Error type to the command");
                    _prefix = CommandString.Error;
                }
            }
        }

        public CommandString Type => _prefix;

        public string Data { get; private set; }

        public Command(string message)
        {
            try
            {
                message = Parser.Sanitize(message);
                Prefix = Parser.GetPrefix(message);
                Data = Parser.GetData(message);
            }
            catch (BadRequestException e)
            {
                _prefix = CommandString.Error;
                Data = $"{{\"message\": \"{e.Message}\"}}";
            }

        }
    }
}