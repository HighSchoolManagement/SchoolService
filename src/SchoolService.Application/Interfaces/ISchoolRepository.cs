using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.Models;
using SchoolService.Domain.Entities;


namespace SchoolService.Application.Interfaces
{
    public interface ISchoolRepository
    {
        /// <summary>
        /// Retrieves schools using pagination
        /// </summary>
        /// <param name="page">The page number, starting from 1</param>
        /// <param name="pageSize">The maximum number of schools per page</param>
        /// <returns>A collection of schools for the requested page and the total number</returns>
        Task<(List<SchoolReadModel> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<SchoolReadModel> AddAsync(SchoolCreateModel school);
        Task<SchoolReadModel?> GetBySchoolCodeAsync(string code);
        /// <summary>
        /// Retrieves a school by its unique identifier without checking its active status 
        /// </summary>
        /// <param name="id">The unique identifier of the school.</param>
        /// <returns>The school if found; otherwise, <c>null</c>.</returns>
        Task<SchoolReadModel?> GetByIdAsync(int id);

        Task<School?> GetByIdTrackedAsync(int id);
        Task SaveChangesAsync();
    }
}
