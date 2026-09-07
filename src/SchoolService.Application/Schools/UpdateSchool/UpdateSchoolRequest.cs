
using System.ComponentModel.DataAnnotations;

namespace SchoolService.Application.Schools.UpdateSchool
{
    public class UpdateSchoolRequest
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }
        [StringLength(30)]
        public string? PhoneNumber { get; set; }
        [StringLength(250)]
        public string? Address { get; set; }
        [StringLength(50)]
        public string? City { get; set; }
        [StringLength(50)]
        public string? Region { get; set; }
        [StringLength(10)]
        public string? PostalCode { get; set; }
        [StringLength(50)]
        public string? Country { get; set; }
        public bool? IsActive { get; set; }
    }
}
