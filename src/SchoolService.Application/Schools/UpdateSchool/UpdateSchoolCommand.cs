using SchoolService.Application.Common.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.UpdateSchool
{
    public class UpdateSchoolCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateSchoolRequest? updateSchoolRequest { get; set; }
    }
}
