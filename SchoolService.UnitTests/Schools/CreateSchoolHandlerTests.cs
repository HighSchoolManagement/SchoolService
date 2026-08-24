using Moq;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SchoolService.UnitTests.Schools
{
    public class CreateSchoolHandlerTests
    {
        private readonly CreateSchoolHandler classUnderTest;
        private readonly Mock<ISchoolRepository> repository;
        private readonly School schoolEntity;
        private CreateSchoolRequest schoolRequest;
        public CreateSchoolHandlerTests()
        {
            repository = new Mock<ISchoolRepository>();
            // classUnderTest = new CreateSchoolHandler(repository.Object);
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
        }

        // [Fact]
        // public async Task HandleAsync_Should_CreateSchool()
        // {
        //     //Arrange
        //     var schoolCode = "C001";
        //
        //     repository.Setup(x => x.GetBySchoolCodeAsync(schoolCode)).ReturnsAsync((School?)null);
        //     repository.Setup(x => x.AddAsync(It.IsAny<School>())).ReturnsAsync(schoolEntity);
        //     //Act
        //     var response = await classUnderTest.HandleAsync(schoolRequest);
        //   
        //     //Assert
        //     Assert.Equal(schoolRequest.SchoolCode, response.SchoolCode);
        //     Assert.Equal(schoolRequest.Name, response.Name);
        //     Assert.Equal(schoolRequest.Email, response.Email);
        //     Assert.Equal(schoolRequest.PhoneNumber, response.PhoneNumber);
        //     Assert.True(response.IsActive);
        //     repository.Verify(x => x.GetBySchoolCodeAsync(schoolRequest.SchoolCode), Times.Once);
        //     repository.Verify(
        //     x => x.AddAsync(It.Is<School>(s =>
        //         s.SchoolCode == schoolRequest.SchoolCode &&
        //         s.Name == schoolRequest.Name &&
        //         s.Email == schoolRequest.Email &&
        //         s.IsActive)),
        //     Times.Once);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_ThrowDuplicateSchoolException_When_SchoolCodeAlreadyExists()
        // {
        //     var schoolCode = "C001";
        //     repository.Setup(x => x.GetBySchoolCodeAsync(schoolCode)).ReturnsAsync(schoolEntity);
        //     await Assert.ThrowsAsync<DuplicateSchoolCodeException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_SchoolCodeIsWhiteSpace()
        // {
        //     var schoolCode = "    ";
        //     schoolRequest.SchoolCode = schoolCode;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never); 
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_NameIsWhiteSpace()
        // {
        //     var name = "    ";
        //     schoolRequest.Name = name;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_TrimFields_When_RequestContainsWhitespace()
        // {
        //     var schoolCode = "C001";
        //     var name = "   School    ";
        //     var email = "Email@gmail.com   ";
        //     schoolRequest.Name = name;
        //     schoolRequest.Email = email;
        //     repository.Setup(x => x.GetBySchoolCodeAsync(schoolCode)).ReturnsAsync((School?)null);
        //     repository.Setup(x => x.AddAsync(It.IsAny<School>())).ReturnsAsync(schoolEntity);
        //     var response = await classUnderTest.HandleAsync(schoolRequest);
        //
        //     Assert.Equal("School", response.Name);
        //     Assert.Equal("Email@gmail.com", response.Email);
        //     repository.Verify(x => x.GetBySchoolCodeAsync(schoolCode), Times.Once);
        //     repository.Verify(
        //     x => x.AddAsync(It.Is<School>(s =>
        //         s.Name == "School" &&
        //         s.Email == "Email@gmail.com")),
        //     Times.Once);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_EmailIsInvalid()
        // {
        //     var email = "Emailgmail.com";
        //     schoolRequest.Email = email;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_SchoolCodeLengthIsInvalid()
        // {
        //     var schoolCode = new string('s', 21);
        //     schoolRequest.SchoolCode = schoolCode;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_RegionLengthIsInvalid()
        // {
        //     var region = new string('s', 51);
        //     schoolRequest.Region = region;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_NameLengthIsInvalid()
        // {
        //     var name = new string('s', 101);
        //     schoolRequest.Name = name;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_EmailLengthIsInvalid()
        // {
        //     var str = new string('s', 151);
        //     var email = str + "@gmail.com";
        //     schoolRequest.Email = email;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_PhoneNumberLengthIsInvalid()
        // {
        //     var phoneNumber = new string('s', 31);
        //     schoolRequest.PhoneNumber = phoneNumber;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_AddressLengthIsInvalid()
        // {
        //     var address = new string('s', 251);
        //     schoolRequest.Address = address;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_CityLengthIsInvalid()
        // {
        //     var city = new string('s', 51);
        //     schoolRequest.City = city;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_PostalCodeLengthIsInvalid()
        // {
        //     var postalCode = new string('s', 11);
        //     schoolRequest.PostalCode = postalCode;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
        //
        // [Fact]
        // public async Task HandleAsync_Should_ThrowArgumentException_When_CountryLengthIsInvalid()
        // {
        //     var country = new string('s', 51);
        //     schoolRequest.Country = country;
        //     await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.HandleAsync(schoolRequest));
        //     repository.Verify(x => x.GetBySchoolCodeAsync(It.IsAny<string>()), Times.Never);
        //     repository.Verify(x => x.AddAsync(It.IsAny<School>()), Times.Never);
        // }
    }
}
