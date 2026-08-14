using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.DeleteSchool
{
    public class SchoolAlreadyInactiveException : Exception
    {
        public SchoolAlreadyInactiveException() : base("School Already Inactive")
        {

        }
    }
}
