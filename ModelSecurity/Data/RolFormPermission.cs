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
//     public class RolFormPermissionData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;

//         private readonly ILogger<RolFormPermissionData> _logger;

//         public RolFormPermissionData(ApplicationDbContext context, IDbConnection dapperConnection,ILogger<RolFormPermissionData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }
//     /// <summary>
//     /// CONSULTA POR MEDIO DE LINQ
//     /// </summary>
//     ///  consulta completa
//         public async Task<IEnumerable<RolFormPermission>> GetAllAsync()
//         {
//             return await _context.RolFormPermission.Include(rfp => rfp.Rol)
//                                                         .Include(rfp => rfp.Permission)
//                                                         .Include(rfp => rfp.Form)
//                                                         .ToListAsync(); // hace un tipo Join en LINQ
//         }

//         // consulta por ID
//         public async Task<RolFormPermission?> GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.RolFormPermission.Include(rfp => rfp.Rol)
//                                                         .Include(rfp => rfp.Permission)
//                                                         .Include(rfp => rfp.Form)
//                                                         .FirstOrDefaultAsync(rfp => rfp.Id == id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, $"Error al obtener el RolFormPermission con ID {id}");
//                 throw;
//             }

//         }

//         public async Task<RolFormPermission> CreateAsync(RolFormPermission rolFormPermission)
//         {
//             try
//             {
//                 _context.RolFormPermission.Add(rolFormPermission);
//                 await _context.SaveChangesAsync();
//                 return rolFormPermission;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear el RolFormPermission: {ex.Message}");
//                 throw;
//             }
//         }

//         public async Task<bool> UpdateAsync(RolFormPermission rolFormPermission)
//         {
//             try
//             {
//                 _context.Set<RolFormPermission>().Update(rolFormPermission);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al actualizar el RolFormPermission : {ex.Message}");
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var rolFormPermission = await _context.Set<RolFormPermission>().FindAsync(id);
//                 if (rolFormPermission == null)
//                     return false;
//                 _context.Set<RolFormPermission>().Remove(rolFormPermission);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el RolFormPermission {ex.Message}");
//                 return false;
//             }
//         }


//     /// CONSULTA, CREA, ACTUALIZA Y ELIMINA POR MEDIO DE LINQ
//         /// <summary>
//         /// CONSULTA POR MEDIO DE SQL
//         /// </summary>
//         /// consulta completa
//         public async Task<IEnumerable<RolFormPermission>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT
//                     rfp.Id, 
//                     r.Id AS RoleId, r.Name AS RoleName, 
//                     p.Id AS PermissionId, p.Name AS PermissionName, 
//                     f.Id AS FormId, f.Name AS FormName
//                 FROM RolFormPermission rfp
//                 INNER JOIN Rol r ON rfp.RoleId = r.Id
//                 INNER JOIN Permission p ON rfp.PermissionId = p.Id
//                 INNER JOIN Form f ON rfp.FormId = f.Id";

//             return await _dapperConnection.QueryAsync<RolFormPermission>(query);
//         }

//         /// <summary>
//         /// consulta por ID
//         /// </summary>
//         public async Task<RolFormPermission?> GetByIdAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT 
//                         rfp.Id, 
//                         r.Id AS RoleId, r.Name AS RoleName, 
//                         p.Id AS PermissionId, p.Name AS PermissionName, 
//                         f.Id AS FormId, f.Name AS FormName
//                     FROM RolFormPermission rfp
//                     INNER JOIN Rol r ON rfp.RoleId = r.Id
//                     INNER JOIN Permission p ON rfp.PermissionId = p.Id
//                     INNER JOIN Form f ON rfp.FormId = f.Id
//                     WHERE rfp.Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<RolFormPermission>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, $"Error al obtener el RolFormPermission con ID {id}");
//                 throw;
//             }
//         }

//         /// <summary>Se crea un RolFormPermission en la base de datos</summary>
//         /// 
//         public async Task<int> CreateAsyncSql(RolFormPermission rolFormPermission)
//         {
//             try
//             {
//                 string sql = @"
//                             INSERT INTO RolFormPermission(RoleId, PermissionId, FormId)
//                             VALUES(@RoleId, @PermissionId, @FormId);
//                             SELECT CAST(SCOPE_IDENTITY() as int)";
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, new {
//                     rolFormPermission.Id, 
//                     rolFormPermission.FormId,
//                     rolFormPermission.PermissionId});
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex,"Error al crear un nuevo RolFormPermission con SQL");
//                 throw;
//             }
//         }
//         /// <summary>Se actualiza un RolFormPermission en la base de datos</summary>
//         /// 
//         public async Task<bool> UpdateAsyncSql(RolFormPermission rfp)
//         {
//             try
//             {
//                 string sql = @"
//                             UPDATE RolFormPermission 
//                             SET 
//                                 RoleId = @RoleId,
//                                 PermissionId = @PermissionId,
//                                 FormId = @FormId
//                             WHERE Id = @Id";
//                 var affectedRows = await  _dapperConnection.ExecuteAsync(sql, new{
//                     rfp.Id,
//                     rfp.RolId,
//                     rfp.FormId,
//                     rfp.PermissionId
//                 });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar RolFormPermission con SQL");
//                 throw;
//             }
//         }

//         /// <summary>Se elimina un RolFormPermission en la base de datos</summary>
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 string sql = "DELETE FROM RolFormPermission WHERE Id = @Id";

//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, new {Id = id});
//                 return affectedRows >0;
//             }
//             catch(Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar un RolFormPermission con SQL");
//                 throw;
//             }
//         }

//         /// <summary>
//         /// Elimina un RolFormPermission de manera logica de la base  de datos SQL
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE RolFormPermission
//                     SET Active = 0
//                     WHERE Id = @Id;
//                     SELECT CAST(@@ROWCOUNT AS int);";

//                 int rowsAffected = await _dapperConnection.QuerySingleAsync<int>(query, new { Id = id });

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error al eliminar logicamente RolFormPermission: {ex.Message}");
//                 return false;
//             }
//         }
//     }
// }
