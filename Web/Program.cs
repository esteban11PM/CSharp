using Business;
using Data;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using Entity.Model;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión correctamente
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar DbContext para Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Agregar IDbConnection si `FormData` usa Dapper
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

// Agregar servicios de negocio y datos

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

// Configuración de CORS (opcional, si tu frontend lo necesita)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
    );
});

builder.Services.AddControllers();

// Configuración de Swagger mejorada
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilitar Swagger también en producción (pero con restricciones)
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
        options.RoutePrefix = string.Empty; // Hace que Swagger esté en la raíz (opcional)
    });
}

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Habilitar CORS si se agregó
app.UseAuthorization();
app.MapControllers();
app.Run();
