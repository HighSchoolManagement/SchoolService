using Moq;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Application.Schools.UpdateSchool;
using SchoolService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace SchoolService.UnitTests.Schools
{
    public class GetSchoolHandlerTests
    {
        private readonly Mock<ISchoolRepository> repository;
        private readonly GetSchoolsHandler handler;
        private List<School> schools;
        public GetSchoolHandlerTests()
        {
            repository = new Mock<ISchoolRepository>();
            // handler = new GetSchoolsHandler(repository.Object);
            schools = new List<School>
            {

                    new School
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
                    new School
                    {
                        SchoolCode = "002",
                        Name = "School 2",
                        Address = "Address 2",
                        City = "City 2",
                        Country = "Country 2",
                        Email = "Email@gmail.com",
                        PhoneNumber = "PhoneNumber 2",
                        Region = "Region 2",
                        PostalCode = "71000",
                    }
            };
        }

    //     [Fact]
    //     public async Task HandleAsync_Should_ReturnAllSchools()
    //     {
    //         repository.Setup(x => x.GetAllAsync()).ReturnsAsync(schools);
    //
    //         var response = await handler.HandleAsync();
    //         Assert.Equal(2, response.Count());
    //         repository.Verify(x => x.GetAllAsync(), Times.Once);
    //     }
    //
    //     [Fact]
    //     public async Task HandleAsync_Should_ReturnEmptyList()
    //     {
    //         repository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<School>());
    //         var response = await handler.HandleAsync();
    //         Assert.Empty(response);
    //         repository.Verify(x => x.GetAllAsync(), Times.Once);
    //     }
    }
}
