using AutoMapper;
using SchoolService.Application.Schools.Models;
using SchoolService.Domain.Entities;

namespace SchoolService.Infrastructure.Mapping
{
    public class SchoolMappingProfile : Profile
    {
        public SchoolMappingProfile()
        {
            CreateMap<School, SchoolReadModel>();
            CreateMap<SchoolCreateModel, School>();
        }
    }
}
