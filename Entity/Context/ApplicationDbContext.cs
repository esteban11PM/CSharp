using Dapper;
using Entity.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
using System.Reflection;
using Utilities.Enums;

namespace Entity.Contexts
{
    /// <summary>
    /// Representa el contexto de la base de datos de la aplicación, proporcionando configuraciones y métodos
    /// para la gestión de entidades y consultas personalizadas con Dapper.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Configuración de la aplicación.
        /// </summary>
        protected readonly IConfiguration _configuration;
        protected readonly DatabaseType _databaseType;

        /// <summary>
        /// Definition of DBSet
        /// </summary>
        /// <summary>
        /// para form
        /// </summary>
        // 
        public DbSet<Form> Form { get; set; }

        /// <summary>
        /// Para person
        /// </summary>
        public DbSet<Person> Person {get; set;}

        /// <summary>
        /// Para Rol
        /// </summary>
        public DbSet<Rol> Rol {get; set;}

        /// <summary>
        /// Para module
        /// </summary>
        public DbSet<Entity.Model.Module> Module {get; set;}

        /// <summary>
        /// Para Permission
        /// </summry>
        public DbSet<Permission> Permission {get; set;}

        /// <summary>
        /// para user
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// para RolUser 
        /// </summary>
        public DbSet<RolUser> RolUser {get; set; }

        /// <summary>
        /// para RolFormPermission
        /// </summary>
        public DbSet<RolFormPermission> RolFormPermission {get; set;}

        /// <summary>
        /// para FormModule
        /// </summary>
        public DbSet<FormModule> FormModule {get; set;}
        



        /// <summary>
        /// Constructor del contexto de la base de datos.
        /// </summary>
        /// <param name="options">Opciones de configuración para el contexto de base de datos.</param>
        /// <param name="configuration">Instancia de IConfiguration para acceder a la configuración de la aplicación.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
            string provider = _configuration["DatabaseProvider"];
            _databaseType = provider switch
            {
                "DefaultConnection"    => DatabaseType.DefaultConnection,
                "PostgreSQL"   => DatabaseType.PostgreSQL,
                "MySQL"        => DatabaseType.MySQL,
                _              => throw new ArgumentException("Proveedor de base de datos no válido en la configuración.")
            };
        }

        /// <summary>
        /// Configura los modelos de la base de datos aplicando configuraciones desde ensamblados.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo de base de datos.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(modelBuilder);
        }

        //conexión del dapper
        public IDbConnection CreateConnection()
        {
            // Se obtiene la cadena de conexión en dependiendo el motor que se utilice
            string connectionString = _databaseType switch
            {
                DatabaseType.DefaultConnection  => _configuration.GetConnectionString("DefaultConnection"),
                DatabaseType.PostgreSQL => _configuration.GetConnectionString("PostgreSQL"),
                DatabaseType.MySQL      => _configuration.GetConnectionString("MySQL"),
                _                       => throw new ArgumentException("Motor de BD no soportado")
            };

            return _databaseType switch
            {
                DatabaseType.DefaultConnection  => new SqlConnection(connectionString),
                DatabaseType.PostgreSQL => new NpgsqlConnection(connectionString),
                DatabaseType.MySQL      => new MySqlConnection(connectionString),
                _                       => throw new ArgumentException("Motor de BD no válido")
            };
        }
        /// <summary>
        /// Configura opciones adicionales del contexto, como el registro de datos sensibles.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de configuración del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Llama a la conexion segun el contexto
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString(_databaseType switch
                {
                    DatabaseType.DefaultConnection  => "DefaultConnection",
                    DatabaseType.PostgreSQL => "PostgreSQL",
                    DatabaseType.MySQL      => "MySQL",
                    _                       => throw new ArgumentException("Proveedor no soportado")
                });

                switch (_databaseType)
                {
                    case DatabaseType.DefaultConnection:
                        optionsBuilder.UseSqlServer(connectionString);
                        break;
                    case DatabaseType.PostgreSQL:
                        optionsBuilder.UseNpgsql(connectionString);
                        break;
                    case DatabaseType.MySQL:
                        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 2, 0)));
                        break;
                }
            }
            optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <summary>
        /// Configura convenciones de tipos de datos, estableciendo la precisión por defecto de los valores decimales.
        /// </summary>
        /// <param name="configurationBuilder">Constructor de configuración de modelos.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        /// <summary>
        /// Guarda los cambios en la base de datos, asegurando la auditoría antes de persistir los datos.
        /// </summary>
        /// <returns>Número de filas afectadas.</returns>
        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        /// <summary>
        /// Guarda los cambios en la base de datos de manera asíncrona, asegurando la auditoría antes de la persistencia.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indica si se deben aceptar todos los cambios en caso de éxito.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación.</param>
        /// <returns>Número de filas afectadas de forma asíncrona.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve una colección de resultados de tipo genérico.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="text">Consulta SQL a ejecutar.</param>
        /// <param name="parameters">Parámetros opcionales de la consulta.</param>
        /// <param name="timeout">Tiempo de espera opcional para la consulta.</param>
        /// <param name="type">Tipo opcional de comando SQL.</param>
        /// <returns>Una colección de objetos del tipo especificado.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string text, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryAsync<T>(command.Definition);
        }

        /// <summary>
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve un solo resultado o el valor predeterminado si no hay resultados.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="text">Consulta SQL a ejecutar.</param>
        /// <param name="parameters">Parámetros opcionales de la consulta.</param>
        /// <param name="timeout">Tiempo de espera opcional para la consulta.</param>
        /// <param name="type">Tipo opcional de comando SQL.</param>
        /// <returns>Un objeto del tipo especificado o su valor predeterminado.</returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string text, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        /// <summary>
        /// Método interno para garantizar la auditoría de los cambios en las entidades.
        /// </summary>
        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
        }

        /// <summary>
        /// Estructura para ejecutar comandos SQL con Dapper en Entity Framework Core.
        /// </summary>
        public readonly struct DapperEFCoreCommand : IDisposable
        {
            /// <summary>
            /// Constructor del comando Dapper.
            /// </summary>
            /// <param name="context">Contexto de la base de datos.</param>
            /// <param name="text">Consulta SQL.</param>
            /// <param name="parameters">Parámetros opcionales.</param>
            /// <param name="timeout">Tiempo de espera opcional.</param>
            /// <param name="type">Tipo de comando SQL opcional.</param>
            /// <param name="ct">Token de cancelación.</param>
            public DapperEFCoreCommand(DbContext context, string text, object parameters, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                Definition = new CommandDefinition(
                    text,
                    parameters,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: ct
                );
            }

            /// <summary>
            /// Define los parámetros del comando SQL.
            /// </summary>
            public CommandDefinition Definition { get; }

            /// <summary>
            /// Método para liberar los recursos.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}