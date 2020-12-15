using System;

namespace SharedProject.CommandUtils
{
    public class LoginData : ICommandType
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}