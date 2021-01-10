using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ProjectServer.Models
{
    [Table("TopicMessage")]
    public class TopicMessage
    {
        [Key] public int TopicMessageId { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTimeOffset CreatedAt { get; set; }

        [Required] public string Text { get; set; }

        [Required] public int UserId { get; set; }

        [Required] public int TopicsId { get; set; }

        [ForeignKey(nameof(TopicsId))]
        [InverseProperty(nameof(Topic.TopicMessages))]
        public virtual Topic Topics { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("TopicMessages")]
        public virtual User User { get; set; }
    }
}