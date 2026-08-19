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
            SchoolValidator.ValidateSchoolCodeRequired(request.SchoolCode);
            SchoolValidator.ValidateNameRequired(request.Name);
            var schoolCode = request.SchoolCode.Trim();
            var name = request.Name.Trim();
            var email = request.Email?.Trim();
            var phoneNumber = request.PhoneNumber?.Trim();
            var address = request.Address?.Trim();
            var city = request.City?.Trim();
            var postalCode = request.PostalCode?.Trim();
            var region = request.Region?.Trim();
            var country = request.Country?.Trim();

            SchoolValidator.ValidateEmailFormat(email);

            SchoolValidator.ValidateSchoolCodeLength(schoolCode);
            SchoolValidator.ValidateNameLength(name);
            SchoolValidator.ValidateEmailLength(email);
            SchoolValidator.ValidatePhoneNumberLength(phoneNumber);
            SchoolValidator.ValidateAddressLength(address);
            SchoolValidator.ValidateCityLength(city);
            SchoolValidator.ValidatePostalCodeLength(postalCode);
            SchoolValidator.ValidateRegionLength(region);
            SchoolValidator.ValidateCountryLength(country);

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
            return school.ToCreateResponse();
        }

       

    }
}
