using AutoMapper;
using SchoolService.Application.Interfaces;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SchoolService.Application.Common.Mediator;

namespace SchoolService.Application.Schools.GetSchools
{
    public class GetSchoolsHandler : IRequestHandler<GetSchoolsQuery, PagedResult<GetSchoolsResponse>>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetSchoolsHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

      

        public async Task<PagedResult<GetSchoolsResponse>> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
        {
            var (schools, totalCount) = await _schoolRepository.GetPagedAsync(request.PageNumber, request.PageNumber);
            return new PagedResult<GetSchoolsResponse>
            {
                Items = _mapper.Map<List<GetSchoolsResponse>>(schools),
                TotalCount = totalCount,
                Page = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
