using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

            SchoolValidator.ValidateNameLength(name);
            SchoolValidator.ValidateEmailLength(email);
            SchoolValidator.ValidatePhoneNumberLength(phoneNumber);
            SchoolValidator.ValidateAddressLength(address);
            SchoolValidator.ValidateCityLength(city);
            SchoolValidator.ValidatePostalCodeLength(postalCode);
            SchoolValidator.ValidateRegionLength(region);
            SchoolValidator.ValidateCountryLength(country);

            var existingSchool =await _schoolRepository.GetByIdAsync(id);
            if (existingSchool == null)
            {
                throw new SchoolNotFoundException();
            }
            if (email != null)
            {
                SchoolValidator.ValidateEmailFormat(email);
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
