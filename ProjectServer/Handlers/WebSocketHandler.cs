using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using ProjectServer.Models;
using SharedProject.Utils;
using SharedProject;
using Serilog;
using SharedProject.CommandUtils;

namespace ProjectServer.Handlers
{
    public partial class WebSocketHandler
    {
        private static readonly ConcurrentDictionary<User, WebSocket> _connectedClient = new ConcurrentDictionary<User, WebSocket>();
        private User _user;
        private readonly WebSocket _webSocket;
        private readonly ServerResponse _response;

        public WebSocketHandler(WebSocketContext webSocketContext)
        {
            _webSocket = webSocketContext.WebSocket;
            _user = null;
            _response = new ServerResponse();
        }

        public async void Receive()
        {
            try
            {
                byte[] receiveBuffer = new byte[Constants.MaxByte];

                while (_webSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult receiveResult =
                        await _webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Binary)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept binary frame",
                            CancellationToken.None);
                    }
                    else
                    {
                        var message = Encoding.Default.GetString(receiveBuffer).Replace("\0", String.Empty); // receiveBuffer.ToString();

                        Command command = new Command(message);

                        Log.Information($"Received {command.Type} request");

                        Dispatch(command);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Exception: {0}", e);
                await _webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "", CancellationToken.None);
            }
            finally
            {
                Log.Information($"One connection ended");
                _webSocket.Dispose();
            }
        }

        private void Dispatch(Command command)
        {
            switch (command.Type)
            {
                case CommandString.Login:
                    HandleLogin(command.GetDeserializedData<LoginData>());
                    break;

                case CommandString.Register:
                    HandleRegister(command.GetDeserializedData<LoginData>());
                    break;

                case CommandString.DirectMessage:
                    HandleDirectMessage(command.GetDeserializedData<DirectMessageData>());
                    break;

                case CommandString.Send:
                    HandleTopicMessage(command.GetDeserializedData<TopicMessageData>());
                    break;

                case CommandString.List:
                    break;
                case CommandString.CreateTopic:
                    break;
                case CommandString.Join:
                    break;
                case CommandString.Error:
                    break;
                default: throw new NotImplementedException();
            }
        }
    }
}