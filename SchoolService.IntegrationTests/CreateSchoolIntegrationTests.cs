using Microsoft.Extensions.DependencyInjection;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Domain.Entities;
using SchoolService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SchoolService.IntegrationTests
{
    public class CreateSchoolIntegrationTests : IClassFixture<SchoolApiFactory>
    {
        private readonly SchoolApiFactory _factory;
        public CreateSchoolIntegrationTests(SchoolApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateSchool_ReturnsCreateSchoolResponse_WhenSuccess()
        {
            //Arrange
            using var scope = _factory.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();

            var createSchoolRequest = new CreateSchoolRequest
            {
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

            //Act
            var response = await _factory.CreateClient().PostAsJsonAsync("api/schools", createSchoolRequest);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<CreateSchoolResponse>();
            Assert.Equal("001", result.SchoolCode);
        }

        [Fact]
        public async Task CreateSchool_ReturnConflict_WhenStudentCodeExist()
        {
            //Arrange
            using var scope = _factory.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
            var school = new School
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

            context.Schools.Add(school);
            await context.SaveChangesAsync();
            var createSchoolRequest = new CreateSchoolRequest
            {
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

            //Act
            var response = await _factory.CreateClient().PostAsJsonAsync("api/schools", createSchoolRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task CreateSchool_ReturnArgumentException_WhenRequestDataInValid()
        {
            var createSchoolRequest = new CreateSchoolRequest
            {
                SchoolCode = "001",
                Name = "",
                Address = "   ",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };
            //Act
            var response = await _factory.CreateClient().PostAsJsonAsync("api/schools", createSchoolRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
