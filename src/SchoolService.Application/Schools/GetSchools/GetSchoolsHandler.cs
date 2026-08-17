using SchoolService.Application.Interfaces;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.GetSchools
{
    public class GetSchoolsHandler
    {
        private readonly ISchoolRepository _schoolRepository;

        public GetSchoolsHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<List<GetSchoolsResponse>> HandleAsync()
        {
            var schools = await _schoolRepository.GetAllAsync();
            var responses = new List<GetSchoolsResponse>();
            foreach (var item in schools)
            {
                var school = new GetSchoolsResponse()
                {
                    SchoolCode = item.SchoolCode,
                    Name = item.Name,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    City = item.City,
                    Country = item.Country,
                    IsActive = item.IsActive
                };
                responses.Add(school);
            }
            return responses;
        }
    }
}
