using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Mail;
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
            if (string.IsNullOrWhiteSpace(request.Name) &&
                 string.IsNullOrWhiteSpace(request.Email) &&
                 string.IsNullOrWhiteSpace(request.PhoneNumber) &&
                 string.IsNullOrWhiteSpace(request.Address) &&
                 string.IsNullOrWhiteSpace(request.City) &&
                 string.IsNullOrWhiteSpace(request.Region) &&
                 string.IsNullOrWhiteSpace(request.PostalCode) &&
                 string.IsNullOrWhiteSpace(request.Country))
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

            if (name?.Length > 100)
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

            var existingSchool =await _schoolRepository.GetByIdAsync(id);
            if (existingSchool == null)
            {
                throw new SchoolNotFoundException();
            }
            if (!string.Equals(existingSchool.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                var isValidMail = IsValidMail(email);
                if(!isValidMail)
                {
                    throw new ArgumentException("Mail is invalid");
                }
                existingSchool.Email = email;
            }
            if (!string.Equals(existingSchool.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                existingSchool.Name = name ?? "";
            }
            if (!string.Equals(existingSchool.PhoneNumber, phoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                existingSchool.PhoneNumber = phoneNumber;
            }
            if (!string.Equals(existingSchool.Address, address, StringComparison.OrdinalIgnoreCase))
            {
                existingSchool.Address = address;
            }
            if (!string.Equals(existingSchool.City, city, StringComparison.OrdinalIgnoreCase))
            {
                existingSchool.City = city;
            }
            if (!string.Equals(existingSchool.Region, region, StringComparison.OrdinalIgnoreCase))
            {
                existingSchool.Region = region;
            }
            if (!string.Equals(existingSchool.PostalCode, postalCode, StringComparison.OrdinalIgnoreCase))
            {
                existingSchool.PostalCode = postalCode;
            }
            if (!string.Equals(existingSchool.Country, country, StringComparison.OrdinalIgnoreCase))
            {
                existingSchool.Country = country;
            }
            if (!bool.Equals(existingSchool.IsActive, request.IsActive))
            {
                existingSchool.IsActive = request.IsActive;
            }
            await _schoolRepository.SaveChangesAsync();           
        }

        private bool IsValidMail(string? email)
        {
            try
            {
                if (email == null)
                {
                    return false;
                }
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
