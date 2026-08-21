using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper _mapper;

        public SchoolRepository(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SchoolReadModel> AddAsync(SchoolCreateModel model)
        {
            var school = _mapper.Map<School>(model);
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
            return _mapper.Map<SchoolReadModel>(school);
        }

        public async Task<(List<SchoolReadModel> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {

            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page or Page Size must be greater than 0");
            }
            var query = _context.Schools.AsQueryable();
                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderBy(s => s.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<SchoolReadModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return (items, totalCount);

        }

        public async Task<SchoolReadModel?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var school = await _context.Schools
            .Where(s => s.Id == id )
            .ProjectTo<SchoolReadModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            return school;
        }

        public async Task<School?> GetByIdTrackedAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var school = await _context.Schools
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

            return school;
        }

        public async Task<SchoolReadModel?> GetBySchoolCodeAsync(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code is required");
            }
            var school = await _context.Schools
                 .Where(s => s.SchoolCode == code)
                .ProjectTo<SchoolReadModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
                return school;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
