namespace SchoolService.Application.Common.Mediator
{
    /// <summary>
    /// Marker interface: một request "tự khai báo" kiểu dữ liệu nó sẽ trả về (TResponse)
    /// sau khi được xử lý xong. Không chứa logic, chỉ là hợp đồng (contract) cho compiler
    /// suy luận kiểu trả về khi gọi IMediator.Send().
    /// </summary>
    /// <typeparam name="TResponse">Kiểu dữ liệu trả về sau khi xử lý request này.</typeparam>
    public interface IRequest<TResponse>
    {
    }
}
