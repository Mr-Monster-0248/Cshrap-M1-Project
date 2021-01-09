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
                var resp = new ServerSimpleResponse<InfoDto>("error", new InfoDto{ Message = "Wrong username or password" });
                Communication.SendResponse<ServerSimpleResponse<InfoDto>, InfoDto>(_webSocket, resp);
                // TODO: send response here
                return;
            }
            
            // Set the state of the user
            _user = dbUser;

            // Add the user to the Dictionary
            _connectedClient.TryAdd(_user, _webSocket); //TODO: handle error
            Log.Information($"{_user.Username} logged in");
            Communication.SendResponse<ServerSimpleResponse<InfoDto>, InfoDto>(_webSocket, ServerSimpleResponse.SuccessNoData());
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
                AuthService.RegisterUser(newUser);
                Log.Information($"{_user.Username} registered");
                Communication.SendResponse<ServerSimpleResponse<InfoDto>, InfoDto>(_webSocket, ServerSimpleResponse.SuccessNoData());
            }
            catch
            {                
                var resp = new ServerSimpleResponse<InfoDto>("error", new InfoDto{ Message = "Error while registering user" });

                Log.Error("A registration failed");
                Communication.SendResponse<ServerSimpleResponse<InfoDto>, InfoDto>(_webSocket, resp);
            }
        }
    }
}