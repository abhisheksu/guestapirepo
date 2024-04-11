using System;
using System.Collections.Generic;

namespace GuestApi.Models
{
    public class Guest
    {
        public Guid Id { get; set; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }

    public enum Title
    {
        Mr,
        Mrs,
        Miss,
        Ms
    }
}
