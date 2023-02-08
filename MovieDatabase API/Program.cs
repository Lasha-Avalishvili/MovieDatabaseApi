using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;
using MovieDatabase_API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration["MovieDb"]));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

 