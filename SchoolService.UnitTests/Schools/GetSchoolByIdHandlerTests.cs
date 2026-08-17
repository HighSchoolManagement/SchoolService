using Moq;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolService.UnitTests.Schools
{
    public class GetSchoolByIdHandlerTests
    {
        private readonly Mock<ISchoolRepository> repository;
        private readonly GetSchoolByIdHandler handler;
        private School school;

        public GetSchoolByIdHandlerTests()
        {
            repository = new Mock<ISchoolRepository>();
            handler = new GetSchoolByIdHandler(repository.Object);
            school = new School
            {

                Name = "School",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };
        }

        [Fact]
        public async Task HandleAsync_Should_GetSchoolById()
        {
            //Arrange
            var schoolId = 1;
            repository.Setup(x => x.GetByIdAsync(schoolId)).ReturnsAsync(school);

            //Act
            var response = await handler.HandleAsync(schoolId);

            //Assert
            Assert.Equal(school.Id, response.Id);
            Assert.Equal(school.Name, response.Name);
            Assert.Equal(school.PhoneNumber, response.PhoneNumber);
            Assert.Equal(school.Email, response.Email);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_IdIsInvalid()
        {
            var schoolId = 0;
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(schoolId));
            repository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowSchoolNotFoundException_When_SchoolNotFound()
        {
            var schoolId = 1;
            repository.Setup(x => x.GetByIdAsync(schoolId)).ReturnsAsync((School?)null);

            await Assert.ThrowsAsync<SchoolNotFoundException>(() => handler.HandleAsync(schoolId));
            repository.Verify(x => x.GetByIdAsync(schoolId), Times.Once);
        }
    }
}
