using AutoMapper;
using SchoolService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.GetSchoolById
{
    public class GetSchoolByIdHandler
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetSchoolByIdHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<GetSchoolByIdResponse> HandleAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var school = await _schoolRepository.GetByIdAsync(id);
            if (school == null)
            {
                throw new SchoolNotFoundException();
            }

            return _mapper.Map<GetSchoolByIdResponse>(school);
        }
    }
}
