using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDocuments.Models
{
    public class UsersModel
    {
        [Key]
        public int Id { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Email address too long")]
        [Required(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name cannot be empty.")]
        public string FirstName { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name cannot be empty.")]
        
        public string LastName { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

    }
}
