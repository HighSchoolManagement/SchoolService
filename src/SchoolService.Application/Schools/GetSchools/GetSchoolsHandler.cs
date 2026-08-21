using AutoMapper;
using SchoolService.Application.Interfaces;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.GetSchools
{
    public class GetSchoolsHandler
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetSchoolsHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public class PagedResult<T>
        {
            public List<T> Items { get; set; } = new();
            public int TotalCount { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        }

        public async Task<PagedResult<GetSchoolsResponse>> HandleAsync(int page, int pageSize)
        {
            var (schools, totalCount) = await _schoolRepository.GetPagedAsync(page, pageSize);
            return new PagedResult<GetSchoolsResponse>
            {
                Items = _mapper.Map<List<GetSchoolsResponse>>(schools),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
