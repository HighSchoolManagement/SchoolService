using AutoMapper;
using Moq;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Application.Schools.Models;
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
        private readonly Mock<IMapper> mapper;
        private readonly GetSchoolsHandler handler;
        private List<School> schools;
        private List<SchoolReadModel> schoolReadModels;
        public GetSchoolHandlerTests()
        {
            repository = new Mock<ISchoolRepository>();
            mapper = new Mock<IMapper>();
            handler = new GetSchoolsHandler(repository.Object, mapper.Object);
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
            schoolReadModels = new List<SchoolReadModel>
            {
                 new SchoolReadModel
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
                new SchoolReadModel
                {
                      Id = 2,
                    SchoolCode = "001",
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
          
        }

        [Fact]
        public async Task HandleAsync_Should_ReturnAllSchools()
        {

             var getSchoolResponse = new List<GetSchoolsResponse> {
                new GetSchoolsResponse
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
                new GetSchoolsResponse
                {
                      Id = 2,
                    SchoolCode = "001",
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

            mapper.Setup(x => x.Map<List<GetSchoolsResponse>>(schoolReadModels)).Returns(getSchoolResponse);
            repository.Setup(x => x.GetPagedAsync(1, 10)).ReturnsAsync((schoolReadModels,2));

            var response = await handler.Handle(new GetSchoolsQuery { PageNumber =1, PageSize = 10}, CancellationToken.None);
            Assert.Equal(2, response.TotalCount);
            repository.Verify(x => x.GetPagedAsync(1, 10), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_ReturnEmptyList()
        {
            repository.Setup(x => x.GetPagedAsync(1, 10)).ReturnsAsync((new List<SchoolReadModel>(),0));
            var response = await handler.Handle(new GetSchoolsQuery { PageNumber = 1, PageSize = 10 }, CancellationToken.None);
            Assert.Null(response.Items);
            repository.Verify(x => x.GetPagedAsync(1, 10), Times.Once);
        }
    }
}
