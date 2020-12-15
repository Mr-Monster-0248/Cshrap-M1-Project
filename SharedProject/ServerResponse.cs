using System;
using System.Text;
using System.Text.Json;

namespace SharedProject
{
    [Serializable]
    public class ServerResponse
    {
        public string Status { get; set; } // TODO: make it as an enum
        public string Data { get; set; }

        public ServerResponse()
        {
            Status = "success";
            Data = "";
        }

        public ServerResponse(string data)
        {
            Status = "success";
            Data = data;
        }

        public ServerResponse(string status, string data)
        {
            Status = status;
            Data = data;
        }

        public void SetSuccessNoData()
        {
            Status = "success";
            Data = "";
        }

        public override string ToString()
        {
            return Status == "error" ? $"{{\"Error\": {Data}}}" : Data;
        }

        public byte[] ToByte()
        {
            return JsonSerializer.SerializeToUtf8Bytes(this);
        }
    }
}