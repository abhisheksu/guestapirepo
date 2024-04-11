using System;
using System.ComponentModel.DataAnnotations;

namespace GuestApi.Commands
{
    public class AddPhoneCommand
    {
        public Guid GuestId { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        public string PhoneNumber { get; set; }
    }
}