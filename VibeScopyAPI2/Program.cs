using System.Text;
using System.Text.Json.Serialization;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI2.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //opts.JsonSerializerOptions.Converters.Add(new GeoJsonConverterFactory());
        opts.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => cfg.AddCollectionMappers(),
                typeof(MappingProfiles));

var test = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<VibeScopUnitOfWork>(options =>
    options.UseNpgsql(test,
    x => x.UseNetTopologySuite()
    )
);

#region Sécurité

var test3 = builder.Configuration["Token"];
var test2 = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(test3));

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = test2,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#endregion //Sécurité

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Sécurité
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();