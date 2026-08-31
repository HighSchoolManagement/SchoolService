using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Application.Schools.UpdateSchool;
using SchoolService.Domain.Entities;

namespace SchoolService.UnitTests.Schools;

using AutoMapper;
using Moq;
using SchoolService.Application.Schools.CreateSchool;

public class UpdateSchoolHandlerTests
{
    private readonly UpdateSchoolHandler classUnderTest;
    private readonly Mock<IMapper> mapper;
    private readonly Mock<ISchoolRepository> repository;
    private readonly School school;
    private readonly UpdateSchoolRequest updateSchoolRequest;

    public UpdateSchoolHandlerTests()
    {
        repository = new Mock<ISchoolRepository>();
        mapper = new Mock<IMapper>();
        classUnderTest = new UpdateSchoolHandler(repository.Object);
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
        updateSchoolRequest = new UpdateSchoolRequest()
        {
            Name = "School Request",
            Address = "Address Request",
            City = "City Request",
            Country = "Country Request",
            Email = "EmailRequest@gmail.com",
            PhoneNumber = "PhoneNumber Request",
            Region = "Region Request",
            PostalCode = "71200",
        };
    }

    [Fact]
    public async Task HandleAsync_Should_UpdateSchool()
    {
        //Arrange
        var schoolId = 1;
        var schoolUpdateRequest = new UpdateSchoolRequest()
        {
            Name = "School Request",
            Address = "Address Request",
            City = "City Request",
            Country = "Country Request",
            Email = "EmailRequest@gmail.com",
            PhoneNumber = "PhoneNumber Request",
            Region = "Region Request",
            PostalCode = "71200",
        };
        repository.Setup(x => x.GetByIdTrackedAsync(schoolId)).ReturnsAsync(school);

        //Act
        await classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = schoolUpdateRequest}, CancellationToken.None);

        //Assert
        Assert.Equal(schoolUpdateRequest.Name, school.Name);
        Assert.Equal(schoolUpdateRequest.Address, school.Address);
        Assert.Equal(schoolUpdateRequest.PostalCode, school.PostalCode);
        Assert.Equal(schoolUpdateRequest.City, school.City);
        Assert.Equal(schoolUpdateRequest.Country, school.Country);
        Assert.Equal(schoolUpdateRequest.Region, school.Region);
        Assert.Equal(schoolUpdateRequest.PhoneNumber, school.PhoneNumber);
        Assert.Equal(schoolUpdateRequest.Email, school.Email);

        repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_UpdateOneFieldSchool()
    {
        //Arrange
        var schoolId = 1;
        var schoolUpdateRequest = new UpdateSchoolRequest() { Name = "School Request" };
        repository.Setup(x => x.GetByIdTrackedAsync(schoolId)).ReturnsAsync(school);

        //Act
        await classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = schoolUpdateRequest }, CancellationToken.None);

        //Assert
        Assert.Equal(schoolUpdateRequest.Name, school.Name);
        Assert.Equal("Address", school.Address);
        Assert.Equal("City", school.City);
        Assert.Equal("Country", school.Country);
        Assert.Equal("Email@gmail.com", school.Email);
        Assert.Equal("PhoneNumber", school.PhoneNumber);
        Assert.Equal("Region", school.Region);
        Assert.Equal("71000", school.PostalCode);
        repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowArgumentException_When_SchoolIdIsInvalid()
    {
        var schoolId = 0;
        var request = new UpdateSchoolRequest
        {
            Name = "New School Name"
        };
        await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = updateSchoolRequest }, CancellationToken.None));
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowArgumentException_When_NoFieldsAreProvided()
    {
        var schoolId = 1;
        var request = new UpdateSchoolRequest();
        await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = request }, CancellationToken.None));
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowArgumentException_When_RequestIsNull()
    {
        var schoolId = 1;
        UpdateSchoolRequest? request = null;
        await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = request }, CancellationToken.None));
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowSchoolNotFoundException_When_SchoolDoesNotExist()
    {
        var schoolId = 1;
        var request = new UpdateSchoolRequest
        {
            Name = "New School Name",
            Email = "email@gmail.com",
        };
        repository.Setup(x => x.GetByIdTrackedAsync(schoolId)).ReturnsAsync((School?)null);
        await Assert.ThrowsAsync<SchoolNotFoundException>(() => classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = request }, CancellationToken.None));
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowArgumentException_When_EmailIsInvalid()
    {
        var schoolId = 1;
        var request = new UpdateSchoolRequest
        {
            Name = "New School Name",
            Email = "emailgmail.com",
        };
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = request }, CancellationToken.None));
        Assert.Contains("Email", exception.Message);
        repository.Verify(x => x.GetByIdTrackedAsync(schoolId), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_TrimFields()
    {
        var schoolId = 1;
        var request = new UpdateSchoolRequest
        {
            Name = "  New School Name  ",
            Email = "email@gmail.com   ",
        };
        repository.Setup(x => x.GetByIdTrackedAsync(schoolId)).ReturnsAsync(school);
        await classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = request }, CancellationToken.None);

        Assert.Equal("New School Name", school.Name);
        Assert.Equal("email@gmail.com", school.Email);
        repository.Verify(x => x.GetByIdAsync(schoolId), Times.Once);
        repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowArgumentException_When_AllProvidedFieldsAreWhitespace()
    {
        var schoolId = 1;
        var request = new UpdateSchoolRequest
        {
            Name = "   ",
            Email = "   ",
        };
        await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new UpdateSchoolCommand { Id = schoolId, updateSchoolRequest = request }, CancellationToken.None));
        repository.Verify(x => x.GetByIdAsync(1), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Theory]
    [InlineData( "ddddddddddddddssssssssssssddd", "dat@gmail.com")]
    [InlineData( "91200", "datgmail.com")]
    public async Task UpdateSchool_ThrowArgumentException_WhenRequestInvalid( string PostalCode, string email)
    {
        updateSchoolRequest.PostalCode = PostalCode;
        updateSchoolRequest.Email = email;
        await Assert.ThrowsAsync<ArgumentException>(() => classUnderTest.Handle(new UpdateSchoolCommand { updateSchoolRequest = updateSchoolRequest }, CancellationToken.None));
    }
}