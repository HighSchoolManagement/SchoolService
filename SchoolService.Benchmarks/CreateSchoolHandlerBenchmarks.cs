using AutoMapper;
using BenchmarkDotNet.Attributes;
using Moq;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolService.Benchmarks
{
    [MemoryDiagnoser]
    [MinIterationCount(15)]
    [MaxIterationCount(30)]
    public class CreateSchoolHandlerBenchmarks
    {
        private CreateSchoolHandler _handler = null!;
        private CreateSchoolCommand _command = null!;
        private readonly Mock<ISchoolRepository> _repository = new();
        private readonly Mock<IMapper> _mapper = new();

        [GlobalSetup]
        public void Setup()
        {
            _repository
                .Setup(r => r.GetBySchoolCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((SchoolReadModel?)null);

            _repository
                .Setup(r => r.AddAsync(It.IsAny<SchoolCreateModel>()))
                .ReturnsAsync(new SchoolReadModel
                {
                    Id = 1,
                    SchoolCode = "SC001",
                    Name = "Truong THPT Le Loi",
                    IsActive = true
                });

            _mapper
                .Setup(m => m.Map<CreateSchoolResponse>(It.IsAny<SchoolReadModel>()))
                .Returns(new CreateSchoolResponse());

            _handler = new CreateSchoolHandler(_repository.Object, _mapper.Object);

            _command = new CreateSchoolCommand
            {
                createSchoolRequest = new CreateSchoolRequest
                {
                    SchoolCode = "SC001",
                    Name = "Truong THPT Le Loi",
                    Email = "contact@leloi.edu.vn",
                    PhoneNumber = "0123456789",
                    Address = "123 Nguyen Trai",
                    City = "Ha Noi",
                    Region = "Ha Noi",
                    PostalCode = "100000",
                    Country = "Vietnam"
                }
            };
        }

        [Benchmark]
        public Task<CreateSchoolResponse> CreateSchool_Handle()
            => _handler.Handle(_command, CancellationToken.None);
    }
}
