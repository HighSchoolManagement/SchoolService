using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools
{
    public static class SchoolMappingExtensions
    {
        public static CreateSchoolResponse ToCreateResponse(this School school) => new()
        {
            Id = school.Id,
            SchoolCode = school.SchoolCode,
            Name = school.Name,
            Email = school.Email,
            PhoneNumber = school.PhoneNumber,
            IsActive = school.IsActive
        };
    }
}
