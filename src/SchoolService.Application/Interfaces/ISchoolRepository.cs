using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.Models;
using SchoolService.Domain.Entities;


namespace SchoolService.Application.Interfaces
{
    public interface ISchoolRepository
    {
        Task<(List<SchoolReadModel> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<SchoolReadModel> AddAsync(SchoolCreateModel school);
        Task<SchoolReadModel?> GetBySchoolCodeAsync(string code);
        Task<SchoolReadModel?> GetByIdAsync(int id);
        Task<School?> GetByIdTrackedAsync(int id);
        Task SaveChangesAsync();
    }
}
