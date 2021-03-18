using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Project.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }

        public DateTime Sent { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public List<Comment> Comments { get; set; } // Just added 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}