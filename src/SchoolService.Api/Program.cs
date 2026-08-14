using Microsoft.EntityFrameworkCore;
using SchoolService.Api.Exceptions;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.DeleteSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Application.Schools.UpdateSchool;
using SchoolService.Infrastructure.Persistence;
using SchoolService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<GetSchoolsHandler>();
builder.Services.AddScoped<CreateSchoolHandler>();
builder.Services.AddScoped<GetSchoolByIdHandler>();
builder.Services.AddScoped<UpdateSchoolHandler>();
builder.Services.AddScoped<DeleteSchoolHandler>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnectionString")));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
