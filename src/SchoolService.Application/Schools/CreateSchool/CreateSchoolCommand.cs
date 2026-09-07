using SchoolService.Application.Common.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.CreateSchool
{
    public class CreateSchoolCommand : IRequest<CreateSchoolResponse>, IValidatableRequest
    {
        public CreateSchoolRequest? createSchoolRequest { get; set; }
        public object GetValidationTarget() => createSchoolRequest;
    }
}
