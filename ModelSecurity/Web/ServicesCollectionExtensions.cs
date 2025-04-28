using System.Data;
using Entity;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Web
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDataAccessFactory(this IServiceCollection services,string dbString, IConfiguration configuration)
        {
            // Leer la configuración del motor, por ejemplo, "Postgres", "Mysql", "SqlServer", etc.
            string databaseEngine = dbString;

            switch (databaseEngine)
            {
                case "PgAdmin":
                    //services.AddScoped<IDataAccessFactory, PostgrestDataFactory>();

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));


                    break;
                case "MySQL":
                        services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(
                            configuration.GetConnectionString("MySQL"),
                            new MySqlServerVersion(new Version(9, 2, 0))
                        ));
                        services.AddScoped<IDbConnection>(sp =>
                        new MySqlConnection(configuration.GetConnectionString("MySQL")));
                    break;
                case "SQLServer":

                    //services.AddScoped<IDataAccessFactory, SqlServerDataFactory>();

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

                    break;
                default:
                    throw new InvalidOperationException("Motor de base de datos no soportado o no configurado.");
            }

            return services;
        }
    }
}
