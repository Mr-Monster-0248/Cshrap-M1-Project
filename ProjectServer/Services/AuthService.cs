using System;
using System.IO;
using System.Linq;
using ProjectServer.Models;

namespace ProjectServer.Services
{
    public class AuthService
    {
        public static User LogUserIn(string username, string password)
        {
            var dbContext = DbServices.Instance.Context;
            var dbUser = dbContext.Users.FirstOrDefault(user => user.Username == username);

            if (dbUser == null) return null;

            return BCrypt.Net.BCrypt.Verify(password, dbUser.Password) ? dbUser : null;
        }

        public static User RegisterUser(User newUser)
        {
            var dbContext = DbServices.Instance.Context;

            try
            {
                dbContext.Users.Add(newUser); // TODO: handle error
                dbContext.SaveChanges();
                return newUser;
            }
            catch
            {
                throw;
            }

        }


        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}