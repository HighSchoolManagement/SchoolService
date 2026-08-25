using Moq;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SchoolService.Application.Schools.Models;

namespace SchoolService.UnitTests.Schools
{
    public class GetSchoolByIdHandlerTests
    {
        private readonly Mock<ISchoolRepository> repository;
        private readonly Mock<IMapper> mapper;
        
        private readonly GetSchoolByIdHandler handler;
        private School school;
        private SchoolReadModel schoolReadModel;
        public GetSchoolByIdHandlerTests()
        {
            repository = new Mock<ISchoolRepository>();
            mapper = new Mock<IMapper>();
            handler = new GetSchoolByIdHandler(repository.Object, mapper.Object);
            school = new School()
            {
                Id = 1,
                SchoolCode = "001",
                Name = "School",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };
            schoolReadModel = new SchoolReadModel()
            {
                Id = 1,
                SchoolCode = "001",
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
        public async Task Handle_Should_GetSchoolById()
        {
            //Arrange
            var schoolId = 1;
            var getSchoolByIdResponse = new GetSchoolByIdResponse()
            {
                Id = schoolId,
                SchoolCode = "001",
                Name = "School",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                IsActive = true
            };
            repository.Setup(x => x.GetByIdAsync(schoolId)).ReturnsAsync(schoolReadModel);
            mapper.Setup(x => x.Map<GetSchoolByIdResponse>(schoolReadModel)).Returns(getSchoolByIdResponse);

            //Act
            var response = await handler.Handle(new GetSchoolByIdQuery{ Id = schoolId }, CancellationToken.None);

            //Assert
            Assert.Equal(school.Id, response.Id);
            Assert.Equal(school.Name, response.Name);
            Assert.Equal(school.PhoneNumber, response.PhoneNumber);
            Assert.Equal(school.Email, response.Email);
        }

        [Fact]
        public async Task Handle_Should_ThrowArgumentException_When_IdIsInvalid()
        {
            var schoolId = 0;
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new GetSchoolByIdQuery { Id = schoolId}, CancellationToken.None));
            repository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowSchoolNotFoundException_When_SchoolNotFound()
        {
            var schoolId = 1;
            repository.Setup(x => x.GetByIdAsync(schoolId)).ReturnsAsync((SchoolReadModel?)null);

            await Assert.ThrowsAsync<SchoolNotFoundException>(() => handler.Handle(new GetSchoolByIdQuery { Id = schoolId}, CancellationToken.None));
            repository.Verify(x => x.GetByIdAsync(schoolId), Times.Once);
        }
    }
}
