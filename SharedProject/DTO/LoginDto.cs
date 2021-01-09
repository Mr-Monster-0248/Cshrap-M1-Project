using System;

namespace SharedProject.DTO
{
    public class LoginDto : ICommandDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}