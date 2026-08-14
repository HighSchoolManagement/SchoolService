using SchoolService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.GetSchoolById
{
    public class GetSchoolByIdHandler
    {
        private readonly ISchoolRepository _schoolRepository;
        public GetSchoolByIdHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<GetSchoolByIdResponse> HandleAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var school =await _schoolRepository.GetByIdAsync(id);
            if(school == null)
            {
                throw new SchoolNotFoundException(); 
            }
            var response = new GetSchoolByIdResponse
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
    }
}
