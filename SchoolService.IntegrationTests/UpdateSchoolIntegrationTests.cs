using Microsoft.Extensions.DependencyInjection;
using SchoolService.Application.Schools.UpdateSchool;
using SchoolService.Domain.Entities;
using SchoolService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SchoolService.IntegrationTests
{
    public class UpdateSchoolIntegrationTests : IClassFixture<SchoolApiFactory>
    {
        private readonly SchoolApiFactory _factory;
        public UpdateSchoolIntegrationTests(SchoolApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task UpdateSchool_ReturnUpdateSchoolResponse_WhenSuccess()
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

            var schoolUpdateRequest = new UpdateSchoolRequest
            {
                Name = "School123",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };

            //Act
            var response = await _factory.CreateClient().PatchAsJsonAsync("api/schools/1", schoolUpdateRequest);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
        }


        [Fact]
        public async Task UpdateSchool_ReturnBadRequest_WhenIdSmallerThanZero()
        {
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

            var schoolUpdateRequest = new UpdateSchoolRequest
            {
                Name = "School123",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };
            //Act
            var response = await _factory.CreateClient().PatchAsJsonAsync("api/schools/0",schoolUpdateRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateSchool_ReturnNotFound_WhenIdSmallerThanZero()
        {
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

            var schoolUpdateRequest = new UpdateSchoolRequest
            {
                Name = "School123",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
            };
            //Act
            var response = await _factory.CreateClient().PatchAsJsonAsync("api/schools/3", schoolUpdateRequest);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
