using Business;
using Data;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using Entity.Model;
using Npgsql;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Obtener la configuración y el proveedor de base de datos seleccionado
var configuration = builder.Configuration;
string databaseProvider = configuration["DatabaseProvider"];

switch (databaseProvider)
{
    case "DefaultConnection":
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IDbConnection>(sp =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
        break;

    case "PostgreSQL":
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));
        builder.Services.AddScoped<IDbConnection>(sp =>
            new NpgsqlConnection(configuration.GetConnectionString("PostgreSQL")));
        break;

    case "MySQL":
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("MySQL"),
                new MySqlServerVersion(new Version(9,2,0))
            ));
        builder.Services.AddScoped<IDbConnection>(sp =>
            new MySqlConnection(configuration.GetConnectionString("MySQL")));
        break;


    default:
        throw new Exception("Proveedor de base de datos no soportado en la configuración.");
}

// Registrar los demás servicios de negocio y de datos
builder.Services.AddScoped<FormBusiness>();
builder.Services.AddScoped<FormData>();

builder.Services.AddScoped<PersonBusiness>();
builder.Services.AddScoped<PersonData>();

builder.Services.AddScoped<RolBusiness>();
builder.Services.AddScoped<RolData>();

builder.Services.AddScoped<ModuleBusiness>();
builder.Services.AddScoped<ModuleData>();

builder.Services.AddScoped<PermissionBusiness>();
builder.Services.AddScoped<PermissionData>();

builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<UserData>();

builder.Services.AddScoped<RolUserBusiness>();
builder.Services.AddScoped<RolUserData>();

builder.Services.AddScoped<RolFormPermissionBusiness>();
builder.Services.AddScoped<RolFormPermissionData>();

builder.Services.AddScoped<FormModuleBusiness>();
builder.Services.AddScoped<FormModuleData>();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy  .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();
