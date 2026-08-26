using SchoolService.Application.Common.Extensions;
using SchoolService.Application.Common.Mediator;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchoolById;

using System.ComponentModel.DataAnnotations;


namespace SchoolService.Application.Schools.UpdateSchool
{
    public class UpdateSchoolHandler : IRequestHandler<UpdateSchoolCommand, Unit>
    {
        private readonly ISchoolRepository _schoolRepository;
        public UpdateSchoolHandler(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }
        public async Task<Unit> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {

            if (request == null || request.updateSchoolRequest == null)
            {
                throw new ArgumentException("Request is null");
            }
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request.updateSchoolRequest, new ValidationContext(request.updateSchoolRequest), validationResults, validateAllProperties: true))
            {
                throw new ArgumentException(string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }

            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }

            var existingSchool = await _schoolRepository.GetByIdTrackedAsync(request.Id);
            if (existingSchool == null)
            {
                throw new SchoolNotFoundException();
            }

            // Field-specific validation that can't be expressed generically stays explicit here.
            if (request.updateSchoolRequest.Name != null)
            {
                SchoolValidator.ValidateNameRequired(request.updateSchoolRequest.Name.Trim());
            }

            // Copies every non-null property from the request onto the entity by name match.
            // New fields added to UpdateSchoolRequest + School will flow through automatically -
            // no new "if" needed here.
            var applied = ObjectPatchExtensions.ApplyNonNullProperties(request.updateSchoolRequest, existingSchool);

            if (!applied)
            {
                throw new ArgumentException("At least one field must be provided.");
            }

            await _schoolRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
