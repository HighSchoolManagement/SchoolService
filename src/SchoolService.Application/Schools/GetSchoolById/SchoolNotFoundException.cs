using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.GetSchoolById
{
    public class SchoolNotFoundException: Exception
    {
        public SchoolNotFoundException(): base("School not found")
        {

        }
    }
}
