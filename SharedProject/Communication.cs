﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading;
using SharedProject.DTO;

namespace SharedProject
{
    public static class Communication
    {
        public static async void SendResponse<T, U>(WebSocket webSocket, T response) 
            where T : ServerResponse<U> 
            where U : CommandDto
        {
            byte[] responseBytes = response.ToByte();
            await webSocket.SendAsync(new ArraySegment<byte>(responseBytes, 0, responseBytes.Length),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
        //
        // public static string ReceiveMsg(Stream s)
        // {
        //     var reader = new StreamReader(s);
        //     return reader.ReadLine();
        // }
    }
}
