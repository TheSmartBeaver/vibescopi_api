using Microsoft.EntityFrameworkCore;
using VibeScopyAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var test = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UnitOfWorkToto>(options =>
    options.UseNpgsql(test,
    x => x.UseNetTopologySuite().UseFuzzyStringMatch()
    //, n => n.UseFuzzyStringMatch() // We add the Fuzzy to calculate Levensthein distances
    )
);

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

