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

        public async Task<CreateSchoolResponse> HandleAsync(CreateSchoolRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SchoolCode))
            {
                throw new ArgumentException("SchoolCode is required");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required");
            }
            var schoolCode = request.SchoolCode.Trim();
            var name = request.Name.Trim();
            var email = request.Email?.Trim();
            var phoneNumber = request.PhoneNumber?.Trim();
            var address = request.Address?.Trim();
            var city = request.City?.Trim();
            var postalCode = request.PostalCode?.Trim();
            var region = request.Region?.Trim();
            var country = request.Country?.Trim();

            if (!string.IsNullOrWhiteSpace(email)
                    && !IsValidEmail(email))
            {
                throw new ArgumentException("Email is invalid");
            }
          

            if (schoolCode.Length > 20)
            {
                throw new ArgumentException("Code is invalid length");
            }
            if (name.Length > 100)
            {
                throw new ArgumentException("Name is invalid length");
            }
            if (email?.Length > 150)
            {
                throw new ArgumentException("Email is invalid length");
            }
            if (phoneNumber?.Length > 30)
            {
                throw new ArgumentException("PhoneNumber is invalid length");
            }
            if (address?.Length > 250)
            {
                throw new ArgumentException("Address is invalid length");
            }
            if (city?.Length > 50)
            {
                throw new ArgumentException("City is invalid length");
            }
            if (postalCode?.Length > 10)
            {
                throw new ArgumentException("PostalCode is invalid length");
            }
            if (region?.Length > 50)
            {
                throw new ArgumentException("Region is invalid length");
            }
            if (country?.Length > 50)
            {
                throw new ArgumentException("Country is invalid length");
            }

            var existingSchool = await _schoolRepository.GetBySchoolCodeAsync(schoolCode);

            if (existingSchool != null)
            {
                throw new DuplicateSchoolCodeException();
            }
            var schoolEntity = new School
            {
                SchoolCode = schoolCode,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                City = city,
                Region = region,
                PostalCode = postalCode,
                Country = country,
                IsActive = true
            };
            var school = await _schoolRepository.AddAsync(schoolEntity);
            var response = new CreateSchoolResponse
            {
                Id = school.Id,
                SchoolCode = school.SchoolCode,
                Name = school.Name,
                Email = school.Email,
                PhoneNumber = school.PhoneNumber,
                IsActive = school.IsActive
            };
            return response;
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
