using AutoMapper;
using SchoolService.Application.Common.Mediator;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.Models;
using SchoolService.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace SchoolService.Application.Schools.CreateSchool
{
    public class CreateSchoolHandler : IRequestHandler<CreateSchoolCommand, CreateSchoolResponse>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public CreateSchoolHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<CreateSchoolResponse> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolCode = request.createSchoolRequest.SchoolCode.Trim();
            var name = request.createSchoolRequest.Name.Trim();
            var email = request.createSchoolRequest?.Email?.Trim();
            var phoneNumber = request.createSchoolRequest?.PhoneNumber?.Trim();
            var address = request.createSchoolRequest?.Address?.Trim();
            var city = request.createSchoolRequest?.City?.Trim();
            var postalCode = request.createSchoolRequest?.PostalCode?.Trim();
            var region = request.createSchoolRequest?.Region?.Trim();
            var country = request.createSchoolRequest?.Country?.Trim();

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
