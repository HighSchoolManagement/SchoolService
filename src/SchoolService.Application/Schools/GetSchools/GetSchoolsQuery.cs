using SchoolService.Application.Common.Mediator;
using SchoolService.Application.Schools.GetSchoolById;

namespace SchoolService.Application.Schools.GetSchools;

public class GetSchoolsQuery : IRequest<PagedResult<GetSchoolsResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}