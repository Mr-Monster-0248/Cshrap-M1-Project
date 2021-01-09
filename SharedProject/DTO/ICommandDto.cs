using System;
using System.Reflection;
using System.Text.Json;

namespace SharedProject.DTO
{
    public interface ICommandDto
    {
        public virtual string ToJsonString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}