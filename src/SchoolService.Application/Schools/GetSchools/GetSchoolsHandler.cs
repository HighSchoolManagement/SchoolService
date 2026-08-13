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

        public async Task<List<School>> HandleAsync()
        {
            var schools = await _schoolRepository.GetAllAsync();
            return schools;
        }
    }
}
