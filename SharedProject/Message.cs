using ProjectShared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    [Serializable]
    public class Message
    {
        private string _text;

        public DateTime CreatedTime { get; }
        public int Id { get; private set; }
        public string Text 
        { 
            get => _text;
            set
            {
                if (value.Length > 280) throw new MessageLengthException(value.Length - 280);
                _text = value;
            }
        }

        public Message(int id, string text)
        {
            Id = id;
            Text = text;
            CreatedTime = DateTime.UtcNow;
        }

        public override string ToString()
        {
            var localTime = DateTime.SpecifyKind(CreatedTime, DateTimeKind.Local);
            return $"{localTime.TimeOfDay} :: {Text}";
        }

    }
}
