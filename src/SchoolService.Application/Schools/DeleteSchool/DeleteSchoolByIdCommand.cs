using SchoolService.Application.Common.Mediator;

namespace SchoolService.Application.Schools.DeleteSchool;

public class DeleteSchoolByIdCommand : IRequest<Unit>
{
    public int Id { get; set; }
}