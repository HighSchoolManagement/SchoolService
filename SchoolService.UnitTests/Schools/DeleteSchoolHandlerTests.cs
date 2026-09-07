using SchoolService.Application.Interfaces;
using Moq;
using SchoolService.Application.Schools.DeleteSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Domain.Entities;
using SchoolService.Application.Schools.Models;

namespace SchoolService.UnitTests.Schools;

public class DeleteSchoolHandlerTests
{
    private readonly  Mock<ISchoolRepository> _schoolRepository;
    private readonly DeleteSchoolHandler _classUnderTest;
    private readonly School _school;
    private readonly SchoolReadModel schoolReadModel;
    public DeleteSchoolHandlerTests()
    {
        _schoolRepository = new Mock<ISchoolRepository>();
        _classUnderTest = new DeleteSchoolHandler(_schoolRepository.Object);
        _school = new School
        {
            Id = 1,
            Name = "School Name",
            IsActive = true
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
            IsActive = true
        };
    }
    [Fact]
    public async Task HandleAsync_Should_DeactivateSchool_And_SaveChanges_WhenSchoolIsActive()
    {
        //Arrange
        var schoolId = 1;
        _schoolRepository.Setup(x => x.GetByIdTrackedAsync(schoolId)).ReturnsAsync(_school);
        //Act
        await _classUnderTest.Handle(new DeleteSchoolByIdCommand { Id = schoolId}, CancellationToken.None);

        //Assert
        Assert.False(_school.IsActive);
        _schoolRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task HandleTask_Should_ReturnArgumentException_IfIdIsInvalid()
    {
        //Arrange
        var schoolId = 0;

        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _classUnderTest.Handle(new DeleteSchoolByIdCommand { Id = schoolId}, CancellationToken.None));
        _schoolRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowSchoolNotFoundException_IfSchoolDoesNotExist()
    {
        //Arrange
        var schoolId = 1;
        _schoolRepository.Setup(x => x.GetByIdTrackedAsync(schoolId)).ReturnsAsync((School?)null);
        //Act & Assert
        await Assert.ThrowsAsync<SchoolNotFoundException>(() => _classUnderTest.Handle(new DeleteSchoolByIdCommand { Id = schoolId }, CancellationToken.None));
        _schoolRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ThrowSchoolAlreadyInactiveException_IfSchoolIsInActive()
    {
        //Arrange
        var schoolId = 1;
        var schoolIsInactive = new School
        {
            Id = schoolId,
            Name = "School Name",
            IsActive = false
        };
        _schoolRepository.Setup(x => x.GetByIdTrackedAsync(schoolId)).ReturnsAsync(schoolIsInactive);
        //Act & Assert
        await Assert.ThrowsAsync<SchoolAlreadyInactiveException>(() => _classUnderTest.Handle(new DeleteSchoolByIdCommand { Id = schoolId }, CancellationToken.None));
        _schoolRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}