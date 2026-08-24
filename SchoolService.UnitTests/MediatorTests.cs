using Moq;
using SchoolService.Application.Common.Mediator;

namespace SchoolService.UnitTests;

public class MediatorTests
{
    //Tạo 1 handler giả thật sự tồn tại trong bộ nhớ — đây là cái mà bạn muốn Mediator gọi tới.
    public class FakeResquest : IRequest<string>
    {}

    public class FakeHandler : IRequestHandler<FakeResquest, string>
    {
        public async Task<string> Handle(FakeResquest request, CancellationToken cancellationToken)
        {
            return "handled-by-fake";
        }
    }

    [Fact]
    public async Task Send_Should_CallCorrectHandler_And_ReturnItsResult()
    {
        // Arrange
        var fakeHandler = new FakeHandler();
        //Tạo 1 "DI container giả" — không kết nối gì tới Program.cs hay DI thật của ứng dụng cả, hoàn toàn độc lập, chỉ tồn tại trong test này.
        var serviceProvider = new Mock<IServiceProvider>();
        //Dạy cho cái container giả đó 1 quy tắc: "nếu sau này có ai hỏi mày .GetService(typeof(IRequestHandler<FakeResquest, string>)), hãy trả lời bằng fakeHandler." Đây chính là chỗ giả lập hành vi mà lẽ ra AddScoped<...>() thật trong Program.cs sẽ làm.
        serviceProvider.Setup(x => x.GetService(typeof(IRequestHandler<FakeResquest, string>))).Returns(fakeHandler);
        //Đây là điểm mấu chốt: Mediator ở đây là thật (class thật bạn viết, không hề giả) — chỉ có cái IServiceProvider nó nhận vào là giả. Nên test này đang test đúng logic thật của Mediator.
        var mediator = new Mediator(serviceProvider.Object);
        //Gọi Send — bên trong Mediator sẽ tự chạy: lấy request.GetType() → dựng handlerType → gọi _serviceProvider.GetRequiredService(handlerType) (khớp đúng Setup ở trên → nhận về fakeHandler) → gọi fakeHandler.Handle(request, ct) qua dynamic → trả về "handled-by-fake".
        var request = new FakeResquest();
        
        //Act
        var result = await mediator.Send(request);
        Assert.NotNull(result);
        Assert.Equal("handled-by-fake", result);
    }
}