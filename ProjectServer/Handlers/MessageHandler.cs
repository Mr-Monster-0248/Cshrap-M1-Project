using System;
using ProjectServer.Models;
using ProjectServer.Services;
using SharedProject;
using SharedProject.DTO;

namespace ProjectServer.Handlers
{
    internal partial class WebSocketHandler
    {
        private void HandleDirectMessage(DirectMessageDto directMessageDto)
        {
            if (!AuthService.IsLoggedIn(_user))
            {
                Communication.SendError(_webSocket, "You must be logged in to send direct message");
                return;
            }

            var receiver = AuthService.GetUserFromUsername(directMessageDto.Receiver);
            var directMessage = new DirectMessage()
            {
                CreatedAt = DateTimeOffset.Now,
                Sender = _user.UserId,
                Receiver = receiver.UserId,
                Text = directMessageDto.Text
            };

            // Tries to save the direct message in the database
            if (MessageService.SaveDirectMessage(directMessage))
            {
                // Check if the user is currently connected to the server
                if (_connectedClient.ContainsKey(receiver))
                {
                    MessageService.SendDirectMessage(
                        directMessage,
                        _connectedClient[receiver]
                    );
                }
                
                Communication.SendSuccess(_webSocket);
            }
        }

        private void HandleTopicMessage(TopicMessageDto dto)
        {
            throw new NotImplementedException();
        }
    }
}