using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SchoolService.Api.Exceptions;
using SchoolService.Application.Interfaces;
using SchoolService.Application.Schools;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.DeleteSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Application.Schools.UpdateSchool;
using SchoolService.Infrastructure.Mapping;
using SchoolService.Infrastructure.Persistence;
using SchoolService.Infrastructure.Repositories;
using SchoolService.Application.Common.Mediator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IMediator, Mediator>();

builder.Services.AddScoped<GetSchoolsHandler>();
builder.Services.AddScoped<CreateSchoolHandler>();
builder.Services.AddScoped<GetSchoolByIdHandler>();
builder.Services.AddScoped<UpdateSchoolHandler>();
builder.Services.AddScoped<DeleteSchoolHandler>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<IRequestHandler<CreateSchoolCommand, CreateSchoolResponse>, CreateSchoolHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateSchoolCommand, Unit>, UpdateSchoolHandler>();
// Quet 2 assembly (Application + Infrastructure) de tim tat ca cac Profile ke tren,
// roi dang ky IMapper dung chung cho toan bo ung dung (Scoped, tu dong DI).
builder.Services.AddAutoMapper(cfg => { },
    typeof(SchoolProfile).Assembly,
    typeof(SchoolMappingProfile).Assembly);
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnectionString")));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();
app.UseCors("AllowFrontend");
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
