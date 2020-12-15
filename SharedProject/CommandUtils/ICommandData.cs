using System;
using System.Reflection;
using System.Text.Json;

namespace SharedProject.CommandUtils
{
    public interface ICommandData
    {
        public virtual string ToJsonString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}