using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Domain.Entities;
using SchoolService.Infrastructure.Persistence;

namespace SchoolService.Infrastructure.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly SchoolDbContext _context;
        public SchoolRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<School> AddAsync(School school)
        {
            await _context.AddAsync(school);
            try{
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Number == 2601 ||
                        sqlException.Number == 2627)
                    {
                        throw new DuplicateSchoolCodeException();
                    }
                }
                throw;
            }
            return school;
        }

        public async Task<List<School>> GetAllAsync()
        {
            var schools = await _context.Schools.ToListAsync();
            return schools;
        }

        public async Task<School?> GetBySchoolCodeAsync(string code)
        {
            var school = await _context.Schools.FirstOrDefaultAsync(s => s.SchoolCode == code);
            return school;
        }
    }
}
