using System;
using GuestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace GuestApi.Commands
{
    public class AddGuestCommand
    {
        public Guid Id { get; set; }
        
        public Title Title { get; set; }
        
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "At least one phone number is required.")]
        public List<string> PhoneNumbers { get; set; }
    }
}
