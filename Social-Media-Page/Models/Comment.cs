using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Project.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public int MessageId { get; set; }

        public int UserId { get; set; }

        public string Comments { get; set; }

        public string Name { get; set; } //added in later

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Message Messages { get; set; }

        public User Users { get; set; }

    }
}