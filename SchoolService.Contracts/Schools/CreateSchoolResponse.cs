namespace SchoolService.Contracts.Schools
{
    public class CreateSchoolResponse
    {
        public int Id { get; set; }
        public string SchoolCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
    }
}
