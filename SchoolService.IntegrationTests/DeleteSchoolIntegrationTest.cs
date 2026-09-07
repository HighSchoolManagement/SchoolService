using Microsoft.Extensions.DependencyInjection;
using SchoolService.Domain.Entities;
using SchoolService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SchoolService.IntegrationTests
{
    public class DeleteSchoolIntegrationTest: IClassFixture<SchoolApiFactory>
    {
        private readonly SchoolApiFactory _factory;
        public DeleteSchoolIntegrationTest(SchoolApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task DeleteSchool_ReturnsNoContent_WhenSuccess()
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
                IsActive = true
            };

            context.Schools.Add(school);
            await context.SaveChangesAsync();

            //Act
            var response = await _factory.CreateClient().DeleteAsync("api/schools/1");

            //Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteSchool_ReturnConflic_WhenSchoolStatusIsInActiveAlready()
        {
            using var scope = _factory.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
            var school = new School
            {
                Id = 3,
                SchoolCode = "003",
                Name = "School",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
                IsActive = false
            };

            context.Schools.Add(school);
            await context.SaveChangesAsync();

            //Act
            var response = await _factory.CreateClient().DeleteAsync("api/schools/3");

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSchool_ReturnNotFound_WhenSchoolDoesNotExist()
        {
            //Act
            var response = await _factory.CreateClient().DeleteAsync("api/schools/2");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSchool_ReturnBadRequest_WhenIdSmallerThanZero()
        {
            //Act
            var response = await _factory.CreateClient().DeleteAsync("api/schools/0");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

       
    }
}
