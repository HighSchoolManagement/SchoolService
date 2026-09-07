using Microsoft.Extensions.DependencyInjection;
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
    public class GetSchoolByIdIntegrationTests : IClassFixture<SchoolApiFactory>
    {
        private readonly SchoolApiFactory _factory;
        public GetSchoolByIdIntegrationTests(SchoolApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetSchoolById_ReturnsSchool_WhenExists()
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

            //Act
            var response = await _factory.CreateClient().GetAsync("api/schools/1");

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<GetSchoolByIdResponse>();
            Assert.Equal("001", result.SchoolCode);
        }

        [Fact]
        public async Task GetSchoolById_ReturnsNotFound_WhenSchoolDoesNotExist()
        {
            //Act
            var response = await _factory.CreateClient().GetAsync("api/schools/2");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetSchoolById_ReturnBadRequest_WhenIdSmallerThanZero()
        {
            //Act
            var response = await _factory.CreateClient().GetAsync("api/schools/0");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
