// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Entity.Context;
// using Entity.DTOs;
// using Dapper;
// using Entity.Model;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using System.Data;

// namespace Data
// {
//     public class PermissionData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         private readonly ILogger<PermissionData> _logger;

//         public PermissionData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<PermissionData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }

//     //CONSULTA POR MEDIO DE LINQ
//         //consulta completa
//         public async Task<IEnumerable<Permission>> GetAllAsync()
//         {
//             return await _context.Set<Permission>().ToListAsync();
//         }

//         //consulta por ID
//         public async Task<Permission?> GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.Set<Permission>().FindAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el permiso con ID {PermissionId}", id);
//                 throw;
//             }

//         }

//         public async Task<Permission> CreateAsync(Permission permission)
//         {
//             try
//             {
//                 await _context.Set<Permission>().AddAsync(permission);
//                 await _context.SaveChangesAsync();
//                 return permission;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear el permiso: {ex.Message}");
//                 throw;
//             }
//         }

//         public async Task<bool> UpdateAsync(Permission permission)
//         {
//             try
//             {
//                 _context.Set<Permission>().Update(permission);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al actualizar el permiso : {ex.Message}");
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var permission = await _context.Set<Permission>().FindAsync(id);
//                 if (permission == null)
//                     return false;
//                 _context.Set<Permission>().Remove(permission);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el permiso {ex.Message}");
//                 return false;
//             }
//         }

//     //CONSULTA POR MEDIO DE SQL
//         //consulta completa
//         public async Task<IEnumerable<Permission>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT Id, Name, Description
//                 FROM Permission";

//             return await _dapperConnection.QueryAsync<Permission>(query);
//         }
//         //consulta por ID
//         public async Task<Permission?> GetByIdAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT Id, Name, Description
//                     FROM Permission
//                     WHERE Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<Permission>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el permiso con ID {PermissionId}", id);
//                 throw;
//             }
//         }

//         // crea un nuevo campo en la entidad con SQL
//         public async Task<int> CreateAsyncSql(Permission permission)
//         {
//             try
//             {
//                 string sql = @"INSERT INTO Permission (Name, Description)
//                                 VALUES(@Name, @Description);
//                                 SELECT CAST(SCOPE_IDENTITY() as int)";
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, new{
//                     permission.Name,
//                     permission.Description,
//                     permission.Active
                    
//                 });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex,"Error al crear un nuevo permission con SQL");
//                 throw;
//             }
//         }

//         // actualiza un campo de la entidad en la base de datos con SQL
//         public async Task<bool> UpdateAsyncSql(Permission permission)
//         {
//             try
//             {
//                 string sql = @"
//                             UPDATE Permission 
//                             SET 
//                                 Name = @Name,
//                                 Description = @Description
//                             WHERE Id = @Id";
//                 var affectedRows = await  _dapperConnection.ExecuteAsync(sql, new{
//                     permission.Name,
//                     permission.Description,
//                     permission.Active
//                 });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar permission con SQL");
//                 throw;
//             }
//         }

//         // elimina un campo de la entidad en la base de datos con SQL
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 string sql = "DELETE FROM Permission WHERE Id = @Id";

//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, new {Id = id});
//                 return affectedRows >0;
//             }
//             catch(Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar un campo con SQL");
//                 throw;
//             }
            
//         }

//         /// <summary>
//         /// Elimina un Permission de manera logica de la base  de datos SQL
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE Permission 
//                     SET Active = 0
//                     WHERE Id = @Id;
//                     SELECT CAST(@@ROWCOUNT AS int);";

//                 int rowsAffected = await _dapperConnection.QuerySingleAsync<int>(query, new { Id = id });

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error al eliminar logicamente permission: {ex.Message}");
//                 return false;
//             }
//         }
//     }
// }
