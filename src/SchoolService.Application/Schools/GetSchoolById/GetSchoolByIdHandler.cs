using AutoMapper;
using SchoolService.Application.Common.Mediator;
using SchoolService.Application.Interfaces;


namespace SchoolService.Application.Schools.GetSchoolById
{
    public class GetSchoolByIdHandler : IRequestHandler<GetSchoolByIdQuery, GetSchoolByIdResponse>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;

        public GetSchoolByIdHandler(ISchoolRepository schoolRepository, IMapper mapper)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
        }

        public async Task<GetSchoolByIdResponse> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var school = await _schoolRepository.GetByIdAsync(request.Id);
            if (school == null)
            {
                throw new SchoolNotFoundException();
            }

            return _mapper.Map<GetSchoolByIdResponse>(school);
        }
    }
}
