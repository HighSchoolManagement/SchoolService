using SchoolService.Application.Interfaces;
using SchoolService.Domain.Entities;
using System.Net.Mail;

namespace SchoolService.Application.Schools.CreateSchool
{
    public class CreateSchoolHandler
    {
        private readonly ISchoolRepository _schoolRepository;

        public CreateSchoolHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<School> HandleAsync(CreateSchoolRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SchoolCode))
            {
                throw new ArgumentException("SchoolCode is required");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required");
            }

            if (!string.IsNullOrWhiteSpace(request.Email)
                    && !IsValidEmail(request.Email))
            {
                throw new ArgumentException("Email is invalid");
            }
            var existingSchool = await _schoolRepository.GetBySchoolCodeAsync(request.SchoolCode);

            if (existingSchool != null)
            {
                throw new DuplicateSchoolCodeException();
            }
            var schoolEntity = new School
            {
                SchoolCode = request.SchoolCode,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                Region = request.Region,
                PostalCode = request.PostalCode,
                Country = request.Country,
                IsActive = true
            };
            var school = await _schoolRepository.AddAsync(schoolEntity);
            return school;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
