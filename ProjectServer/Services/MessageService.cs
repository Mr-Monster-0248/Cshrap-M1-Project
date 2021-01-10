using System.Collections.Generic;
using System.Net.WebSockets;
using ProjectServer.Models;
using Serilog;
using SharedProject;
using SharedProject.DTO;

namespace ProjectServer.Services
{
    public static class MessageService
    {
        public static bool SaveDirectMessage(DirectMessage newDirectMessage)
        {
            var context = DbServices.Instance.Context;

            try
            {
                context.DirectMessages.Add(newDirectMessage);
                context.SaveChanges();
                Log.Information(
                    $"User {newDirectMessage.Sender} sent direct message to user {newDirectMessage.Receiver}");
                return true;
            }
            catch
            {
                Log.Error("Could not save direct message");
                return false;
            }
        }

        public static void SendDirectMessage(DirectMessage directMessage, WebSocket receiverWebsocket)
        {
            var receiver = AuthService.GetUserFromId(directMessage.Receiver);
            var sender = AuthService.GetUserFromId(directMessage.Sender);
            var message = new DirectMessageDto
            {
                Sender = sender.Username,
                Receiver = receiver.Username,
                Text = directMessage.Text
            };
            
            Communication.SendResponse(receiverWebsocket, message);
        }
    }
}