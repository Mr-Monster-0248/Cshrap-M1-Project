using System;

namespace SharedProject
{
    public class User
    {
        public int Id { get; private set; }
        public String Username { get; private set; }
        public String Password { private get; set; }


        public User(int id, String username, String password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public User GetUserfromUsernameAndPassword(String username, String password)
        {
            // TODO: get user from DB
            return new User(0, username, password);
        }
        
    }
}
