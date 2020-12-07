using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Serilog;
using SharedProject.Utils;

namespace ProjectServer
{
    internal class Server
    {
        private int _count;
        private readonly string _uri = $"http://{Constants.LocalHost}:{Constants.Port}/";

        public Server()
        {
            _count = 0;
        }

        public void Start()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(_uri);
            listener.Start();
            // Console.WriteLine("Listening...");

            Log.Information("Listening...");

            while (true)
            {
                HttpListenerContext listenerContext = listener.GetContext();
                if (listenerContext.Request.IsWebSocketRequest)
                {
                    ProcessRequest(listenerContext);
                }
                else
                {
                    listenerContext.Response.StatusCode = 400;
                    listenerContext.Response.Close();
                }
            }
        }

        public async void ProcessRequest(HttpListenerContext listenerContext)
        {
            WebSocketContext webSocketContext = null;
            try
            {
                webSocketContext = await listenerContext.AcceptWebSocketAsync(null);
                Interlocked.Increment(ref _count);
                Log.Information($"New connection from: {webSocketContext.Origin}");
            }
            catch (Exception e)
            {
                listenerContext.Response.StatusCode = 500;
                listenerContext.Response.Close();
                Log.Error("Exception: {0}", e);
            }

            WebSocket webSocket = webSocketContext.WebSocket;

            try
            {
                byte[] receiveBuffer = new byte[Constants.MaxByte];

                while (webSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult receiveResult =
                        await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        await webSocket.SendAsync(new ArraySegment<byte>(receiveBuffer, 0, receiveResult.Count),
                            WebSocketMessageType.Text, receiveResult.EndOfMessage, CancellationToken.None);
                    }
                    else
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept text frame",
                            CancellationToken.None);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Exception: {0}", e);
            }
            finally
            {
                Log.Information($"Connection ended with: {webSocketContext.Origin}");
                webSocket.Dispose();
                _count--;
            }
        }

    }
}