using System;

namespace SharedProject.DTO
{
    [Serializable]
    public class InfoDto : CommandDto
    {
        public string Message { get; set; }
    }
}