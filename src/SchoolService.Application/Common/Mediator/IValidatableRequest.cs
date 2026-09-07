using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Common.Mediator
{
    public interface IValidatableRequest
    {
        object GetValidationTarget();
    }
}
