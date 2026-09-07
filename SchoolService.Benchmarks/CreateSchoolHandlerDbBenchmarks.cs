using AutoMapper;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging.Abstractions;
using SchoolService.Application.Schools;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Infrastructure.Mapping;
using SchoolService.Infrastructure.Persistence;
using SchoolService.Infrastructure.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolService.Benchmarks
{
    // Benchmark chạm SQL Server thật (không mock). Toàn bộ lần chạy được bọc trong
    // MỘT transaction duy nhất và rollback ở GlobalCleanup, nên KHÔNG có dòng nào
    // được ghi vĩnh viễn vào DB - an toàn để chạy trên DB dev.
    //
    // ĐỔI connection string bên dưới cho khớp máy bạn (giống appsettings.Development.json).
    [MemoryDiagnoser]
    public class CreateSchoolHandlerDbBenchmarks
    {
        private const string ConnectionString =
            "Server=.;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True;";

        private SchoolDbContext _context = null!;
        private IDbContextTransaction _transaction = null!;
        private CreateSchoolHandler _handler = null!;
        private int _counter;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseSqlServer(ConnectionString)
                .Options;
            _context = new SchoolDbContext(options);

            // Mở transaction bao trùm toàn bộ benchmark - rollback ở GlobalCleanup.
            _transaction = _context.Database.BeginTransaction();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SchoolProfile>();
                cfg.AddProfile<SchoolMappingProfile>();
            }, NullLoggerFactory.Instance);
            IMapper mapper = mapperConfig.CreateMapper();

            var repository = new SchoolRepository(_context, mapper);
            _handler = new CreateSchoolHandler(repository, mapper);
        }

        [Benchmark]
        public Task<CreateSchoolResponse> CreateSchool_Handle_RealDb()
        {
            // SchoolCode phải unique trong toàn bộ lần chạy (transaction chưa commit
            // nên các dòng đã insert trước đó vẫn tính là "đã tồn tại").
            var code = Interlocked.Increment(ref _counter);
            var command = new CreateSchoolCommand
            {
                createSchoolRequest = new CreateSchoolRequest
                {
                    SchoolCode = $"BM{code:D8}",
                    Name = "Benchmark School",
                    Email = "bench@example.com",
                    PhoneNumber = "0123456789",
                    Address = "123 Benchmark St",
                    City = "Ha Noi",
                    Region = "Ha Noi",
                    PostalCode = "100000",
                    Country = "Vietnam"
                }
            };
            return _handler.Handle(command, CancellationToken.None);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }
    }
}
