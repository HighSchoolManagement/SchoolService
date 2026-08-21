using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchoolById;


namespace SchoolService.Application.Schools.DeleteSchool
{
    public class DeleteSchoolHandler
    {
        private readonly ISchoolRepository _schoolRepository;
        public DeleteSchoolHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }
        public async Task HandleAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
            var existingSchool = await _schoolRepository.GetByIdTrackedAsync(id);
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
        }
    }
}
