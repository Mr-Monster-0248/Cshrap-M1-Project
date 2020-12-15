using System;
using ProjectServer.Models;
using ProjectServer.Services;
using Serilog;
using SharedProject.CommandUtils;
using SharedProject;

namespace ProjectServer.Handlers
{
    public partial class WebSocketHandler    
    {
        private void HandleLogin (LoginData data)
        {
            var dbUser = AuthService.LogUserIn(data.Username, data.Password);

            if (dbUser == null)
            {
                _response.Status = "error"; // TODO: change status here
                _response.Data = "Wrong username or password";
                // TODO: send response here
                return;
            }
            
            // Set the state of the user
            _user = dbUser;

            // Add the user to the Dictionary
            _connectedClient.TryAdd(_user, _webSocket); //TODO: handle error
            Log.Information($"{_user.Username} logged in");
            _response.SetSuccessNoData();
            Communication.SendResponse(_webSocket, _response);
        }

        private void HandleRegister(LoginData data)
        {
            var newUser = new User
            {
                Username = data.Username,
                Password = AuthService.HashPassword(data.Password)
            };

            try
            {
                AuthService.RegisterUser(newUser);
                Log.Information($"{_user.Username} registered");
                _response.SetSuccessNoData();
                Communication.SendResponse(_webSocket, _response);
            }
            catch
            {
                _response.Status = "error"; // TODO: change status here
                _response.Data = "Error while registering user";
                Log.Error("A registration failed");
                Communication.SendResponse(_webSocket, _response);
            }
        }
    }
}