using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchoolById;

using System.ComponentModel.DataAnnotations;


namespace SchoolService.Application.Schools.UpdateSchool
{
    public class UpdateSchoolHandler
    {
        private readonly ISchoolRepository _schoolRepository;
        public UpdateSchoolHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }
        public async Task HandleAsync (int id ,UpdateSchoolRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Request is null");
            }
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, validateAllProperties: true))
            {
                throw new ArgumentException(string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }
           
            if (request.Name == null &&
                request.Email == null &&
                request.PhoneNumber == null &&
                request.Address == null &&
                request.Region == null &&
                request.City == null &&
                request.PostalCode == null &&
                request.Country == null && !request.IsActive.HasValue)
            {
                throw new ArgumentException("At least one field must be provided.");
            }
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }

            var name = request.Name?.Trim();
            var email = request.Email?.Trim();
            var phoneNumber = request.PhoneNumber?.Trim();
            var address = request.Address?.Trim();
            var city = request.City?.Trim();
            var postalCode = request.PostalCode?.Trim();
            var region = request.Region?.Trim();
            var country = request.Country?.Trim();

            var existingSchool =await _schoolRepository.GetByIdTrackedAsync(id);
            if (existingSchool == null)
            {
                throw new SchoolNotFoundException();
            }
            if (email != null)
            {
                existingSchool.Email = email;
            }
            if (name != null)
            {
                SchoolValidator.ValidateNameRequired(name);
                existingSchool.Name = name;
            }
            if (phoneNumber != null)
            {
                existingSchool.PhoneNumber = phoneNumber;
            }
            if (address != null)
            {
                existingSchool.Address = address;
            }
            if (city != null)
            {
                existingSchool.City = city;
            }
            if (region != null)
            {
                existingSchool.Region = region;
            }
            if (postalCode != null)
            {
                existingSchool.PostalCode = postalCode;
            }
            if (country != null)
            {
                existingSchool.Country = country;
            }
            if (request.IsActive.HasValue)
            {
                existingSchool.IsActive = request.IsActive.Value;
            }
            await _schoolRepository.SaveChangesAsync();           
        }
    }
}
