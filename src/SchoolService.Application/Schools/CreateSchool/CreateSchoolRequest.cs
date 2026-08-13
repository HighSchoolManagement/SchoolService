using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolService.Application.Schools.CreateSchool
{
    public class CreateSchoolRequest
    {
        [Required]
        public string SchoolCode { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; } 
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

    }
}
