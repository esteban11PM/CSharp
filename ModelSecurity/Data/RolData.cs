// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Entity.Context;
// using Dapper;
// using Entity.DTOs;
// using Entity.Model;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using System.Data;

// namespace Data
// {
//     ///<summary>
//     /// Repositorio encargador de la gestion de la entidad Rol en la base de datos
//     ///</summary>
//     public  class RolData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         private readonly ILogger<RolData> _logger;

//         public RolData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<RolData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }

//     /// CONSULTA, CREA, ACTUALIZA Y ELIMINA POR MEDIO DE LINQ
//         ///<summary>
//         ///Obtiene todos los roles almacenados en la base de datos
//         ///</summary>
//         ///<returns>Lista de roles</returns>
//         ///
//         /// consulta completa
//         public async Task<IEnumerable<Rol>> GetAllAsync()
//         {
//             return await _context.Set<Rol>().ToListAsync();
//         }

//         // por ID
//         public async Task<Rol?>GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.Set<Rol>().FindAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Errol al obtener rol con ID {RolId}", id);
//                 throw;//Re-lanza la excepcion para que sea manejada en capas superiores
//             }
//         }

//         ///<summary>
//         ///Crear un nuevo rol en la base de datos
//         ///</summary>
//         ///<param name="rol">Instancia del rol al crear</param>
//         ///<returns>El rol creado</returns>
//         public async Task<Rol> CreateAsync(Rol rol)
//         {
//             try
//             {
//                 await _context.Set<Rol>().AddAsync(rol);
//                 await _context.SaveChangesAsync();
//                 return rol;
//             }
//             catch (Exception ex) {
//                 _logger.LogError($"Error al crear el rol: {ex.Message}");
//                 throw;
//             }
//         }

//         /// <summary>
//         /// Actualiza un rol existente en la base de datos.
//         /// </summary>
//         /// <param name="rol">Objeto con la información actualizada.</param>
//         /// <returns>True si la o eración fue exitosa, False en caso contrario.</returns>
//         public async Task<bool> UpdateAsync(Rol rol)
//         {
//             try
//             {
//                 _context.Set<Rol>().Update(rol);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch(Exception ex) 
//             {
//                 _logger.LogError($"Error al actualizar el rol : {ex.Message}");
//                 return false;
//             }
//         }

//         /// <summary>
//         /// Elimina un rol de la base de datos.
//         /// </summary>
//         /// <param name="id">Identificador unico del rol a eliminar </param>
//         /// <returns>True si la eliminación fue exitosa, False en caso contrario.
//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var rol = await _context.Set<Rol>().FindAsync(id);
//                 if (rol == null)
//                     return false;
//                 _context.Set<Rol>().Remove(rol);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el rol {ex.Message}");
//                 return false;
//             }
//         }


//     /// CONSULTA, CREA, ACTUALIZA Y ELIMINA POR MEDIO DE SQL

//         ///<summary>
//         ///Obtiene toda la entidad rol
//         ///</summary>
//         /// Métodos con Dapper (SQL directo)
        
//         public async Task<IEnumerable<Rol>> GetAllAsyncSql()
//         {
//             try
//             {
//                 const string query = @"
//                     SELECT Id, Name, Description
//                     FROM Rol";
                
//                 return await _dapperConnection.QueryAsync<Rol>(query);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener roles con SQL");
//                 throw;
//             }
//         }

//         ///<summary>
//         ///Obtiene un rol especifico por su identificación
//         ///</summary>
//         public async Task<Rol?> GetByIdAsyncSql(int id)
//         {
//             try
//             {
//                 const string query = @"
//                     SELECT Id, Name, Description
//                     FROM Rol
//                     WHERE Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<Rol>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener rol con ID {RolId}", id);
//                 throw;
//             }
//         }

//         /// <summari> Se inserta un nuevo rol en la base de datos </summari>
//         /// 
//         public async Task<int> CreateAsyncSql(Rol rol)
//         {
//             try
//             {
//                 const string sql = @"
//                     INSERT INTO Rol (Name, Description)
//                     VALUES (@Name, @Description);
//                     SELECT CAST(SCOPE_IDENTITY() as int);";
                
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, rol);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear rol con SQL");
//                 throw;
//             }
//         }

//         /// <summari> Se actualiza un el rol en la base de datos </summari>
//         /// 
//         public async Task<bool> UpdateAsyncSql(Rol rol)
//         {
//             try
//             {
//                 const string sql = @"
//                     UPDATE Rol
//                     SET Name = @Name,
//                         Description = @Description
//                     WHERE Id = @Id";
                
//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, rol);
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar rol con SQL");
//                 throw;
//             }
//         }

//         /// <summari> Se Elimina un rol en la base de datos </summari>
//         /// 
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 const string sql = "DELETE FROM Rol WHERE Id = @Id";
//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, new { Id = id });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar rol con SQL");
//                 throw;
//             }
//         }

//         /// <summary>
//         /// Eliminacion logica de un Rol de la base de datos SQL
//         /// </summary>
//         public async Task<bool> SoftDeleteAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE Rol
//                     SET Active = 0
//                     WHERE Id = @Id";

//                 int rowsAffected = await _context.QueryFirstOrDefaultAsync<int>(query, new { Id = id });

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al elimianr logicamente el rol {ex.Message}");
//                 return false;
//             }
//         }
//     }
// }
