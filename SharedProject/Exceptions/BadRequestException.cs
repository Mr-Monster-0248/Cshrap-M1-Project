using System;

namespace SharedProject.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(): base("Request do not match the required syntax ") {}
    }
}