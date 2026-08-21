namespace SchoolService.Application.Common.Mediator
{
    /// <summary>
    /// Điểm duy nhất mà Controller (hoặc bất kỳ caller nào) phụ thuộc vào.
    /// Send() là generic nên dùng chung cho MỌI loại request, không cần biết
    /// trước handler cụ thể là gì.
    /// </summary>
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
