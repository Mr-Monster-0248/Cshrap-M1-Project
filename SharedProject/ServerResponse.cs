using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using SharedProject.DTO;

namespace SharedProject
{
    [Serializable]
    public abstract class ServerResponse<T> where T : CommandDto
    {
        public string Status { get; set; } // TODO: make it as an enum

        public abstract byte[] ToByte();
    }

    [Serializable]
    public class ServerSimpleResponse<T> : ServerResponse<T> where T : CommandDto
    {
        public T Data { get; set; }

        public ServerSimpleResponse()
        {
            Status = null;
            Data = null;
        }

        public ServerSimpleResponse(T data)
        {
            Status = "success";
            Data = data;
        }

        public ServerSimpleResponse(string status, T data)
        {
            Status = status;
            Data = data;
        }


        public static ServerSimpleResponse<InfoDto> SuccessNoData()
        {
            return new ServerSimpleResponse<InfoDto>(new InfoDto {Message = ""});
        }

        public override byte[] ToByte()
        {
            return JsonSerializer.SerializeToUtf8Bytes(this);
        }
    }

    public abstract class ServerSimpleResponse : ServerSimpleResponse<InfoDto>
    {
    }

    [Serializable]
    public class ServerListResponse<T> : ServerResponse<T> where T : CommandDto
    {
        public List<T> Data { get; set; }

        public ServerListResponse()
        {
            Status = null;
            Data = null;
        }

        public ServerListResponse(List<T> data)
        {
            Status = "success";
            Data = data;
        }

        public ServerListResponse(string status, List<T> data)
        {
            Status = status;
            Data = data;
        }
        
        public override byte[] ToByte()
        {
            return JsonSerializer.SerializeToUtf8Bytes(this);
        }
    }
}