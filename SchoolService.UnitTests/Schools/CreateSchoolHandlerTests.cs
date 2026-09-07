using AutoMapper;
using Moq;
using SchoolService.Application.Common.Mediator;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.Models;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SchoolService.UnitTests.Schools
{
    public class CreateSchoolHandlerTests
    {
        private readonly IRequestHandler<CreateSchoolCommand, CreateSchoolResponse> classUnderTest;
        private readonly Mock<ISchoolRepository> repository;
        private readonly Mock<IMapper> mapper;
        private readonly SchoolReadModel schoolReadModel;
        private CreateSchoolRequest schoolRequest;
        private readonly School schoolEntity;
        private readonly CreateSchoolResponse createSchoolResponse;
        public CreateSchoolHandlerTests()
        {
            repository = new Mock<ISchoolRepository>();
            mapper = new Mock<IMapper>();
            classUnderTest = new ValidationHandler<CreateSchoolCommand, CreateSchoolResponse>(
                new CreateSchoolHandler(repository.Object, mapper.Object));
            schoolRequest = new CreateSchoolRequest
            {
                SchoolCode = "C001",
                Name = "School",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };
            schoolReadModel = new SchoolReadModel
            {
                Id = 1,
                SchoolCode = "C001",
                Name = "School",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };

            schoolEntity = new School
            {
                Id = 1,
                SchoolCode = "C001",
                Name = "School",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };
            createSchoolResponse = new CreateSchoolResponse
            {
                Id = 1,
                SchoolCode = "C001",
                Name = "School",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                IsActive = true
            };
        }

        [Fact]
        public async Task HandleAsync_Should_CreateSchool()
        {
            //Arrange
            var schoolCode = "C002";

            repository.Setup(x => x.GetBySchoolCodeAsync(schoolCode)).ReturnsAsync((SchoolReadModel?)null);
            repository.Setup(x => x.AddAsync(It.IsAny<SchoolCreateModel>())).ReturnsAsync(schoolReadModel);
            mapper.Setup(x => x.Map<CreateSchoolResponse>(schoolReadModel)).Returns(createSchoolResponse);
            //Act
            var response = await classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None);

            //Assert
            Assert.Equal(schoolRequest.SchoolCode, response.SchoolCode);
            Assert.Equal(schoolRequest.Name, response.Name);
            Assert.Equal(schoolRequest.Email, response.Email);
            Assert.Equal(schoolRequest.PhoneNumber, response.PhoneNumber);
            Assert.True(response.IsActive);
            repository.Verify(x => x.GetBySchoolCodeAsync(schoolRequest.SchoolCode), Times.Once);
            repository.Verify(
            x => x.AddAsync(It.Is<SchoolCreateModel>(s =>
                s.SchoolCode == schoolRequest.SchoolCode &&
                s.Name == schoolRequest.Name &&
                s.Email == schoolRequest.Email )),
            Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowDuplicateSchoolException_When_SchoolCodeAlreadyExists()
        {
            var schoolCode = "C001";
            repository.Setup(x => x.GetBySchoolCodeAsync(schoolCode)).ReturnsAsync(schoolReadModel);
            await Assert.ThrowsAsync<DuplicateSchoolCodeException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_SchoolCodeIsWhiteSpace()
        {
            var schoolCode = "    ";
            schoolRequest.SchoolCode = schoolCode;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_NameIsWhiteSpace()
        {
            var name = "    ";
            schoolRequest.Name = name;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_TrimFields_When_RequestContainsWhitespace()
        {
            var schoolCode = "C001";
            var name = "   School    ";
            var email = "Email@gmail.com   ";
            schoolRequest.Name = name;
            schoolRequest.Email = email;
            repository.Setup(x => x.GetBySchoolCodeAsync(schoolCode)).ReturnsAsync((SchoolReadModel?)null);
            repository.Setup(x => x.AddAsync(It.IsAny<SchoolCreateModel>())).ReturnsAsync(schoolReadModel);
            mapper.Setup(x => x.Map<CreateSchoolResponse>(schoolReadModel)).Returns(createSchoolResponse);
            var response = await classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None);

            Assert.Equal("School", response.Name);
            Assert.Equal("Email@gmail.com", response.Email);
            repository.Verify(x => x.GetBySchoolCodeAsync(schoolCode), Times.Once);
            repository.Verify(
            x => x.AddAsync(It.Is<SchoolCreateModel>(s =>
                s.Name == "School" &&
                s.Email == "Email@gmail.com" &&
                s.SchoolCode == schoolRequest.SchoolCode)),
            Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_EmailIsInvalid()
        {
            var email = "Emailgmail.com";
            schoolRequest.Email = email;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_SchoolCodeLengthIsInvalid()
        {
            var schoolCode = new string('s', 21);
            schoolRequest.SchoolCode = schoolCode;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_RegionLengthIsInvalid()
        {
            var region = new string('s', 51);
            schoolRequest.Region = region;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_NameLengthIsInvalid()
        {
            var name = new string('s', 101);
            schoolRequest.Name = name;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_EmailLengthIsInvalid()
        {
            var str = new string('s', 151);
            var email = str + "@gmail.com";
            schoolRequest.Email = email;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_PhoneNumberLengthIsInvalid()
        {
            var phoneNumber = new string('s', 31);
            schoolRequest.PhoneNumber = phoneNumber;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_AddressLengthIsInvalid()
        {
            var address = new string('s', 251);
            schoolRequest.Address = address;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_CityLengthIsInvalid()
        {
            var city = new string('s', 51);
            schoolRequest.City = city;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_PostalCodeLengthIsInvalid()
        {
            var postalCode = new string('s', 11);
            schoolRequest.PostalCode = postalCode;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_ThrowArgumentException_When_CountryLengthIsInvalid()
        {
            var country = new string('s', 51);
            schoolRequest.Country = country;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
            repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
            repository.Verify(x => x.AddAsync(It.IsAny<SchoolCreateModel>()), Times.Never);
        }

        [Theory]
        [InlineData("", "School", "dat@gmail.com")]
        [InlineData("C001", "", "dat@gmail.com")]
        [InlineData("C001", "School", "datgmail.com")]
        public async Task CreateSchool_ThrowArgumentException_WhenRequestInvalid(string schoolCode, string name, string email)
        {
            schoolRequest.SchoolCode = schoolCode;
            schoolRequest.Name = name;
            schoolRequest.Email = email;
            await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new CreateSchoolCommand { createSchoolRequest = schoolRequest }, CancellationToken.None));
        }
    }
}
