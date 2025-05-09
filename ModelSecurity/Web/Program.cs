using System.Text;
using System.Text.Json.Serialization;
using Business;
using Business.Services;
using Data;
using Data.Factories;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;
using Web;

var builder = WebApplication.CreateBuilder(args);

// 1) CONFIGURACIÓN DE JWT Y JSON
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // Permite serializar enums como strings
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 2) SWAGGER + DEFINICIONES DE SEGURIDAD
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SecurityModel API", Version = "v1" });

    // 2a) Cabecera custom X-DB-Provider
    c.AddSecurityDefinition("X-DB-Provider", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-DB-Provider",
        Type = SecuritySchemeType.ApiKey,
        Description = "Provea el proveedor de base de datos: sqlserver, postgresql, mysql"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "X-DB-Provider"
                }
            },
            new string[] {}
        }
    });

    // 2b) Seguridad JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese 'Bearer {token}'",
        Name        = "Authorization",
        In          = ParameterLocation.Header,
        Type        = SecuritySchemeType.ApiKey,
        Scheme      = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 3) CORS
builder.Services.AddCors(o =>
    o.AddPolicy("AllowFrontend", p => p
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
    )
);

// 4) TU LÓGICA DE DATOS / BUSINESS / MAPPERS
builder.Services.AddScoped<IDataFactoryGlobal, GlobalFactory>();
builder.Services.AddDataAccessFactory("SQLServer", builder.Configuration);

builder.Services.AddScoped<PersonBusiness>();
builder.Services.AddScoped<RolBusiness>();
builder.Services.AddScoped<FormBusiness>();
builder.Services.AddScoped<ModuleBusiness>();
builder.Services.AddScoped<ModuleFormBusiness>();
builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<UserRolBusiness>();
builder.Services.AddScoped<RolFormPermissionBusiness>();
builder.Services.AddScoped<PermissionBusiness>();

builder.Services.AddAutoMapper(typeof(Helper.MappingProfile));

// 5) AUTENTICACIÓN + AUTORIZACIÓN
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken            = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidIssuer              = jwt["Issuer"],

        ValidateAudience         = true,
        ValidAudience            = jwt["Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(key),

        ValidateLifetime         = true,
        ClockSkew                = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        cfg.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// ¡MUY IMPORTANTE! primero autenticación, luego autorización
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
