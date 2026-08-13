using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.Application.Schools.CreateSchool
{
    public class DuplicateSchoolCodeException : Exception
    {
        public DuplicateSchoolCodeException() : base("A school with the same code already exists.")
        {
        }
    }
}
