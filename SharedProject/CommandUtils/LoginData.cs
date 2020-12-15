using System;

namespace SharedProject.CommandUtils
{
    public class LoginData : ICommandData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}