using System;
using ProjectServer.Models;
using ProjectServer.Services;
using Serilog;
using SharedProject.DTO;
using SharedProject;

namespace ProjectServer.Handlers
{
    internal partial class WebSocketHandler    
    {
        private void HandleLogin (LoginDto dto)
        {
            var dbUser = AuthService.LogUserIn(dto.Username, dto.Password);
            
            

            if (dbUser == null)
            {
                Communication.SendError(_webSocket, "Wrong username or password");
                return;
            }
            
            // Set the state of the user
            _user = dbUser;

            // Add the user to the Dictionary
            _connectedClient.TryAdd(_user, _webSocket); //TODO: handle error
            Log.Information($"{_user.Username} logged in");
            Communication.SendSuccess(_webSocket);
        }

        private void HandleRegister(LoginDto dto)
        {
            var newUser = new User
            {
                Username = dto.Username,
                Password = AuthService.HashPassword(dto.Password)
            };

            try
            {
                var user = AuthService.RegisterUser(newUser);

                // Logging in the user after registration
                _user = user;
                _connectedClient.TryAdd(_user, _webSocket);
                
                Log.Information($"{_user.Username} registered and logged in");
                Communication.SendResponse<ServerSimpleResponse<InfoDto>, InfoDto>(_webSocket, ServerSimpleResponse.SuccessNoData());
            }
            catch
            {
                Log.Error("A registration failed");
                Communication.SendError(_webSocket, "Error while registering user");
            }
        }
    }
}