using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Project.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please provide a First Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide a Last Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please provide your email address")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Please ensure Password confirmation matches")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Message> MessagesWritten { get; set; }

        public List<Comment> CommentsWritten { get; set; }
    }
}