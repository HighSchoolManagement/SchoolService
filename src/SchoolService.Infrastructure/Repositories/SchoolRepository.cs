using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.Models;
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

        public async Task<SchoolReadModel> AddAsync(SchoolCreateModel model)
        {
            var school = new School
            {
                SchoolCode = model.SchoolCode,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
                Region = model.Region,
                PostalCode = model.PostalCode,
                Country = model.Country,
                IsActive = true
            };
            _context.Add(school);
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
            return new SchoolReadModel
            {
                Id = school.Id,
                SchoolCode = school.SchoolCode,
                Name = school.Name,
                Email = school.Email,
                PhoneNumber = school.PhoneNumber,
                Address = school.Address,
                City = school.City,
                Region = school.Region,
                PostalCode = school.PostalCode,
                Country = school.Country,
                IsActive = school.IsActive
            };
        }

        public async Task<(List<SchoolReadModel> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Schools.AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SchoolReadModel
                {
                    Id = s.Id,
                    SchoolCode = s.SchoolCode,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    City = s.City,
                    Region = s.Region,
                    PostalCode = s.PostalCode,
                    Country = s.Country,
                    IsActive = s.IsActive
                })
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<SchoolReadModel?> GetByIdAsync(int id)
        {
            var school = await _context.Schools
            .Where(s => s.Id == id )
            .Select(s => new SchoolReadModel
            {
                Id = s.Id,
                SchoolCode = s.SchoolCode,
                Name = s.Name,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                City = s.City,
                Region = s.Region,
                PostalCode = s.PostalCode,
                Country = s.Country,
                IsActive = s.IsActive
            })
            .FirstOrDefaultAsync();

            return school;
        }

        public async Task<SchoolReadModel?> GetBySchoolCodeAsync(string code)
        {
            var school = await _context.Schools
                 .Where(s => s.SchoolCode == code)
                .Select(s => new SchoolReadModel
                {
                    Id = s.Id,
                    SchoolCode = s.SchoolCode,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    City = s.City,
                    Region = s.Region,
                    PostalCode = s.PostalCode,
                    Country = s.Country,
                    IsActive = s.IsActive
                })
                .FirstOrDefaultAsync();
                return school;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
