namespace SchoolService.Application.Common.Mediator
{
    /// <summary>
    /// Hình dạng chung mà MỌI handler phải tuân theo.
    /// Ràng buộc "where TRequest : IRequest&lt;TResponse&gt;" đảm bảo chỉ những class
    /// đã được khai báo là request hợp lệ mới có thể có handler tương ứng.
    /// </summary>
    /// <typeparam name="TRequest">Kiểu request đầu vào (Query/Command).</typeparam>
    /// <typeparam name="TResponse">Kiểu dữ liệu trả về, phải khớp với TResponse của TRequest.</typeparam>
    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
