using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.DeleteSchool;
using SchoolService.Application.Schools.GetSchoolById;

namespace SchoolService.Api.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                DuplicateSchoolCodeException => StatusCodes.Status409Conflict,
                ArgumentException => StatusCodes.Status400BadRequest,
                SchoolNotFoundException => StatusCodes.Status404NotFound,
                SchoolAlreadyInactiveException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
            
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = exception.Message,
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
