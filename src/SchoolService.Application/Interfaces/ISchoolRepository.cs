using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Domain.Entities;


namespace SchoolService.Application.Interfaces
{
    public interface ISchoolRepository
    {
        Task<List<School>> GetAllAsync();
        Task<School> AddAsync(School school);
        Task<School?> GetBySchoolCodeAsync(string code);
        Task<School?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
