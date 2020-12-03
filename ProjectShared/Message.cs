using ProjectShared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectShared
{
    class Message
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
                else _text = value;
            }
        }

        public Message(int id, string text)
        {
            Id = id;
            Text = text;
            CreatedTime = DateTime.UtcNow;
        }

    }
}
