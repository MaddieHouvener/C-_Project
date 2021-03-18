using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Project.Models
{
    public class Login
    {
        [Key]

        [Required(ErrorMessage = "Please provide your email")]
        [EmailAddress(ErrorMessage = "Please provide a valid email")]
        [Display(Name = "Email")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "Please provide your password")]
        [DataType(DataType.Password)] // add in [DataType(DataType.Password)] to have it be hidden while writing in form
        [Display(Name = "Password")]
        public string LoginPassword { get; set; }
    }
}