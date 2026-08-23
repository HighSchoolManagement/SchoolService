using SchoolService.Application.Common.Mediator;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchoolById;


namespace SchoolService.Application.Schools.DeleteSchool
{
    public class DeleteSchoolHandler : IRequestHandler<DeleteSchoolByIdCommand, Unit>
    {
        private readonly ISchoolRepository _schoolRepository;
        public DeleteSchoolHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }
        public async Task<Unit> Handle(DeleteSchoolByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var existingSchool = await _schoolRepository.GetByIdTrackedAsync(request.Id);
            if (existingSchool == null)
            {
                throw new SchoolNotFoundException();
            }
            if (!existingSchool.IsActive)
            {
                throw new SchoolAlreadyInactiveException();
            }
            existingSchool.IsActive = false;
            await _schoolRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
