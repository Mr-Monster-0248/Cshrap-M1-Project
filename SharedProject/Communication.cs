using System;
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
        private static async void SendByte(WebSocket webSocket, byte[] resp)
        {
            await webSocket.SendAsync(new ArraySegment<byte>(resp, 0, resp.Length),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
        
        
        public static void SendResponse<T, U>(WebSocket webSocket, T response)
            where T : ServerResponse<U>
            where U : CommandDto
        {
            byte[] responseBytes = response.ToByte();
            SendByte(webSocket, responseBytes);
        }


        public static void SendSuccess(WebSocket webSocket)
        {
            byte[] responseBytes = ServerSimpleResponse.SuccessNoData().ToByte();
            SendByte(webSocket, responseBytes);
        }


        public static void SendError(WebSocket webSocket, string errorMessage)
        {
            var errorResp = new ServerSimpleResponse<InfoDto>(
                "error",
                new InfoDto(errorMessage)
            );
            SendByte(webSocket, errorResp.ToByte());
        }

        //
        // public static string ReceiveMsg(Stream s)
        // {
        //     var reader = new StreamReader(s);
        //     return reader.ReadLine();
        // }
    }
}