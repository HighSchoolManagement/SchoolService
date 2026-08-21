using AutoMapper;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.Models;
using SchoolService.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace SchoolService.Application.Schools.CreateSchool
{
    public class CreateSchoolHandler
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public CreateSchoolHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<CreateSchoolResponse> HandleAsync(CreateSchoolRequest request)
        {
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, validateAllProperties: true))
            {
                throw new ArgumentException(string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
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

            var existingSchool = await _schoolRepository.GetBySchoolCodeAsync(schoolCode);

            if (existingSchool != null)
            {
                throw new DuplicateSchoolCodeException();
            }
            var createModel = new SchoolCreateModel
            {
                SchoolCode = schoolCode,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                City = city,
                Region = region,
                PostalCode = postalCode,
                Country = country
            };
            var school = await _schoolRepository.AddAsync(createModel);
            return _mapper.Map<CreateSchoolResponse>(school);
        }

    }
}
