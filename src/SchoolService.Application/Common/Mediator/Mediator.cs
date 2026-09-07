using Microsoft.Extensions.DependencyInjection;

namespace SchoolService.Application.Common.Mediator
{

    public struct Unit
    {
        public static readonly Unit Value = new Unit();
    }
    /// <summary>
    /// Cài đặt IMediator: nhận request, tự tìm ra handler tương ứng qua reflection + DI
    /// container, rồi gọi Handle() trên handler đó. Đây là "trung gian" duy nhất mà
    /// Controller phụ thuộc vào, thay vì phụ thuộc trực tiếp vào N handler.
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            // 1. Lấy runtime type thực sự của request (vd: GetSchoolByIdQuery)
            var requestType = request.GetType();

            // 2. Ghép ra type đóng (closed generic) của handler cần tìm:
            //    IRequestHandler<GetSchoolByIdQuery, GetSchoolByIdResponse>
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            // 3. Nhờ DI container lấy ra instance handler tương ứng.
            //    Nếu handler chưa được đăng ký trong Program.cs, dòng này sẽ throw
            //    InvalidOperationException ngay lập tức - rất dễ debug.
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            // 4. Gọi Handle() trên đúng handler đó.
            return await handler.Handle((dynamic)request, cancellationToken);
        }
    }
}
