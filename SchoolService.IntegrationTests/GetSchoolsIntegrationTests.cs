using Microsoft.Extensions.DependencyInjection;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Domain.Entities;
using SchoolService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SchoolService.IntegrationTests
{
    public class GetSchoolsIntegrationTests : IClassFixture<SchoolApiFactory>
    {
        private readonly SchoolApiFactory _factory;
        public GetSchoolsIntegrationTests(SchoolApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetSchools_ReturnGetSchoolResponse_WhenSucess()
        {
            //Arrange
            using var scope = _factory.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
            var schools = new List<School>
            {
                new School()
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
                },
              new School()
                {
                      Id = 2,
                SchoolCode = "002",
                Name = "School",
                Address = "Address",
                City = "City",
                Country = "Country",
                Email = "Email@gmail.com",
                PhoneNumber = "PhoneNumber",
                Region = "Region",
                PostalCode = "71000",
                }
            };

            context.Schools.AddRangeAsync(schools);
            await context.SaveChangesAsync();

            //Act
            var response = await _factory.CreateClient().GetAsync("api/schools?page=1&pageSize=10");

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<PagedResult<GetSchoolsResponse>>();

            Assert.Equal(2, result.Items.Count());
        }

        [Fact]
        public async Task Get_ReturnsBadRequest_WhenPageIsInvalid()
        {
            var response = await _factory
                .CreateClient()
                .GetAsync("api/schools?page=hello&pageSize=20");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
