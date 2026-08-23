using SchoolService.Application.Common.Mediator;

namespace SchoolService.Application.Schools.GetSchoolById
{
    /// <summary>
    /// Query cho use case "lấy thông tin 1 trường theo Id".
    /// Chỉ là data thuần (không logic) — implement IRequest&lt;GetSchoolByIdResponse&gt;
    /// để "tự khai báo" kiểu trả về, cho phép IMediator.Send() suy luận kiểu lúc compile.
    /// </summary>
    public class GetSchoolByIdQuery : IRequest<GetSchoolByIdResponse>
    {
        public int Id { get; set; }
    }
}
