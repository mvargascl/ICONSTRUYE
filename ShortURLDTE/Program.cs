using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShortURLDTE.Api;
using ShortURLDTE.Application.Interfaces;
using ShortURLDTE.Application.UseCases;
using ShortURLDTE.Domain.Services;
using ShortURLDTE.Infrastructure.Persistence;
using ShortURLDTE.Infrastructure.ShortUrl;
using ShortURLDTE.Infrastructure.XmlValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IConstruye APi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autorizacion JWT esquema. \r\n\r\n Escribe 'Bearer' [espacio] y escribe el token proporcionado.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                        new List<string>()
                    }
                });

});

// Dependencias
builder.Services.AddSingleton<IFacturaValidator, X509FacturaValidator>();
builder.Services.AddSingleton<IFacturaRepository, InMemoryFacturaRepository>();
builder.Services.AddSingleton<IShortUrlService, Base62Shortener>();
builder.Services.AddScoped<ISubirFacturaUseCase, SubirFacturaUseCase>();
builder.Services.AddScoped<IConsultarFacturaUseCase, ConsultarFacturaUseCase>();

//Configurar JWT
var config = builder.Configuration;
var jwtkey = config["JWT:ClaveSecreta"];
var key = Encoding.UTF8.GetBytes(jwtkey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = config["JWT:Issuer"],
            ValidAudience = config["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JWT:ClaveSecreta"]))

        };
    }

    );
builder.Services.AddAuthorization();

var app = builder.Build();
//Endpoint para Login
app.MapPost("/Login", (LoginRequest request) =>
{
    //Validar Credenciales
    if (request.Username == "Iconstruye" && request.Password == "MVARGAS1")
    {
        var token = GenerateJwtToken(request.Username, jwtkey);
        return Results.Ok(new { Token = token });
    }

    return Results.Unauthorized();

});

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();




app.Run();

string GenerateJwtToken(string username, string jwtkey)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey));
    var credencial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
        issuer: "ShortURLDTEDemo",
        audience: "ShortURLDTEDemo",
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: credencial);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

public record LoginRequest(string Username, string Password);