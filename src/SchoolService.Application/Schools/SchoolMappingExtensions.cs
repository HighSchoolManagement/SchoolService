using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Application.Schools.Models;
using SchoolService.Application.Schools.UpdateSchool;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools
{
    public static class SchoolMappingExtensions
    {
        public static CreateSchoolResponse ToCreateResponse(this SchoolReadModel school) => new()
        {
            Id = school.Id,
            SchoolCode = school.SchoolCode,
            Name = school.Name,
            Email = school.Email,
            PhoneNumber = school.PhoneNumber,
            IsActive = school.IsActive
        };

        public static GetSchoolByIdResponse ToGetByIdResponse(this SchoolReadModel school) => new()
        {
            Id = school.Id,
            SchoolCode = school.SchoolCode,
            Name = school.Name,
            Email = school.Email,
            PhoneNumber = school.PhoneNumber,
            IsActive = school.IsActive
        };

        public static GetSchoolsResponse ToGetSchoolsResponse(this SchoolReadModel school) => new()
        {
            Id = school.Id,
            SchoolCode = school.SchoolCode,
            Name = school.Name,
            Email = school.Email,
            PhoneNumber = school.PhoneNumber,
            PostalCode = school.PostalCode,
            Address = school.Address,
            Region = school.Region,
            City = school.City,
            Country = school.Country,
            IsActive = school.IsActive
        };

        public static List<GetSchoolsResponse> ToGetSchoolsResponseList(this List<SchoolReadModel> schools)
            => schools.Select(s => s.ToGetSchoolsResponse()).ToList();
    }
}
