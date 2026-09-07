using AutoMapper;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Application.Schools.Models;

namespace SchoolService.Application.Schools
{
    public class SchoolProfile : Profile
    {
        public SchoolProfile()
        {
            CreateMap<SchoolReadModel, CreateSchoolResponse>();
            CreateMap<SchoolReadModel, GetSchoolByIdResponse>();
            CreateMap<SchoolReadModel, GetSchoolsResponse>();
        }
    }
}
