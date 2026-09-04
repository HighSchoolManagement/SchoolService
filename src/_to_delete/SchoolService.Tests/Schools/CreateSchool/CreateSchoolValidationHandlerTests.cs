using Moq;
using SchoolService.Application.Common.Mediator;
using SchoolService.Application.Schools.CreateSchool;
using Xunit;

namespace SchoolService.Tests.Schools.CreateSchool
{
    public class CreateSchoolValidationHandlerTests
    {
        // Helper dung chung: LUON gan createSchoolRequest day du.
        // Day chinh la buoc hay bi quen -> khien GetValidationTarget() tra ve null.
        private static CreateSchoolCommand BuildValidCommand()
        {
            return new CreateSchoolCommand
            {
                createSchoolRequest = new CreateSchoolRequest
                {
                    SchoolCode = "HCM-001",
                    Name = "Truong THPT Nguyen Du",
                    Email = "contact@nguyendu.edu.vn",
                    PhoneNumber = "0909123456",
                    Address = "123 Nguyen Trai",
                    City = "Ho Chi Minh",
                    Region = "Mien Nam",
                    PostalCode = "700000",
                    Country = "Vietnam"
                }
            };
        }

        [Fact]
        public async Task Handle_ValidRequest_PassesThrough_ToInnerHandler()
        {
            // Arrange
            var command = BuildValidCommand();
            var expectedResponse = new CreateSchoolResponse
            {
                Id = 1,
                SchoolCode = command.createSchoolRequest!.SchoolCode,
                Name = command.createSchoolRequest!.Name
            };

            var innerHandlerMock = new Mock<IRequestHandler<CreateSchoolCommand, CreateSchoolResponse>>();
            innerHandlerMock
                .Setup(h => h.Handle(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var sut = new ValidationHandler<CreateSchoolCommand, CreateSchoolResponse>(innerHandlerMock.Object);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert: validate pass -> khong nem exception, inner handler duoc goi dung 1 lan
            Assert.Equal(expectedResponse.SchoolCode, result.SchoolCode);
            innerHandlerMock.Verify(h => h.Handle(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_MissingRequiredField_ThrowsArgumentException_AndNeverCallsInnerHandler()
        {
            // Arrange: createSchoolRequest VAN duoc gan (khong null),
            // chi co field bat buoc (SchoolCode, Name) de trong -> phai fail o buoc validate.
            var command = new CreateSchoolCommand
            {
                createSchoolRequest = new CreateSchoolRequest
                {
                    SchoolCode = "",
                    Name = ""
                }
            };

            var innerHandlerMock = new Mock<IRequestHandler<CreateSchoolCommand, CreateSchoolResponse>>();
            var sut = new ValidationHandler<CreateSchoolCommand, CreateSchoolResponse>(innerHandlerMock.Object);

            // Act + Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => sut.Handle(command, CancellationToken.None));

            Assert.Contains("SchoolCode", ex.Message);
            innerHandlerMock.Verify(
                h => h.Handle(It.IsAny<CreateSchoolCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ForgottenCreateSchoolRequest_ThrowsArgumentException_ButNotAFieldValidationError()
        {
            // Day la "bay" thuong gap khi viet test/goi command:
            // quen gan createSchoolRequest -> GetValidationTarget() tra ve null
            // -> new ValidationContext(null) nem ArgumentNullException (la 1 dang ArgumentException),
            // KHONG PHAI loi validate field cu the.
            // Neu 1 test-helper dung chung mac loi nay, TOAN BO test dung helper do se
            // dong loat bi ArgumentException, bat ke data truyen vao co hop le hay khong.
            var command = new CreateSchoolCommand(); // createSchoolRequest con NULL!

            var innerHandlerMock = new Mock<IRequestHandler<CreateSchoolCommand, CreateSchoolResponse>>();
            var sut = new ValidationHandler<CreateSchoolCommand, CreateSchoolResponse>(innerHandlerMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(
                () => sut.Handle(command, CancellationToken.None));

            innerHandlerMock.Verify(
                h => h.Handle(It.IsAny<CreateSchoolCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
