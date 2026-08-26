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
           
            if (request.updateSchoolRequest.Name == null &&
                request.updateSchoolRequest.Email == null &&
                request.updateSchoolRequest.PhoneNumber == null &&
                request.updateSchoolRequest.Address == null &&
                request.updateSchoolRequest.Region == null &&
                request.updateSchoolRequest.City == null &&
                request.updateSchoolRequest.PostalCode == null &&
                request.updateSchoolRequest.Country == null && !request.updateSchoolRequest.IsActive.HasValue)
            {
                throw new ArgumentException("At least one field must be provided.");
            }
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }

            var name = request.updateSchoolRequest.Name?.Trim();
            var email = request.updateSchoolRequest.Email?.Trim();
            var phoneNumber = request.updateSchoolRequest.PhoneNumber?.Trim();
            var address = request.updateSchoolRequest.Address?.Trim();
            var city = request.updateSchoolRequest.City?.Trim();
            var postalCode = request.updateSchoolRequest.PostalCode?.Trim();
            var region = request.updateSchoolRequest.Region?.Trim();
            var country = request.updateSchoolRequest.Country?.Trim();

            var existingSchool =await _schoolRepository.GetByIdTrackedAsync(request.Id);
            if (existingSchool == null)
            {
                throw new SchoolNotFoundException();
            }
            if (email != null)
            {
                existingSchool.Email = email;
            }
            if (name != null)
            {
                SchoolValidator.ValidateNameRequired(name);
                existingSchool.Name = name;
            }
            if (phoneNumber != null)
            {
                existingSchool.PhoneNumber = phoneNumber;
            }
            if (address != null)
            {
                existingSchool.Address = address;
            }
            if (city != null)
            {
                existingSchool.City = city;
            }
            if (region != null)
            {
                existingSchool.Region = region;
            }
            if (postalCode != null)
            {
                existingSchool.PostalCode = postalCode;
            }
            if (country != null)
            {
                existingSchool.Country = country;
            }
            if (request.updateSchoolRequest.IsActive.HasValue)
            {
                existingSchool.IsActive = request.updateSchoolRequest.IsActive.Value;
            }
            await _schoolRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
