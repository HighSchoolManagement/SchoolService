using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.GetSchools
{
    public class GetSchoolsResponse
    {
        public int Id { get; set; }
        public string SchoolCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public bool IsActive { get; set; }
    }
}
