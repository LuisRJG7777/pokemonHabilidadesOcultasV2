using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Services;
using Infrastructure.Repositories;
using Core.Interfaces;
using log4net;
using log4net.Config;
using System.Reflection;
using System.Xml;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using pokemon.Application.DTOs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Título (Diseño)
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Pokemon Oculto", Version = "v1" });

    // Botón Authorize (Swagger)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Configurar la documentación de los parámetros del modelo
    c.MapType<LoginRequest>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["usuario"] = new OpenApiSchema { Type = "string" },
            ["password"] = new OpenApiSchema { Type = "string" }
        }
    });
});



// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://example.com") // Cambia a tu origen permitido
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
 
// Register dependencies
builder.Services.AddHttpClient<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<PokemonService>();

// TOKEN 2
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


var app = builder.Build();
 
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// LOG 3
XmlDocument log4netConfig = new XmlDocument();
log4netConfig.Load(File.OpenRead("log4net.config"));
var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
XmlConfigurator.Configure(repo, log4netConfig["log4net"]);


app.UseCors("AllowSpecificOrigin"); // Aplicar la política de CORS
// TOKEN 3
app.UseAuthentication();
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();