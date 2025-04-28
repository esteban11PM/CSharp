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
//     public class RolUserData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         private readonly ILogger<RolUserData> _logger;

//         public RolUserData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<RolUserData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }

//     /// <summary>
//     /// CONSULTA POR MEDIO DE LINQ
//     /// </summary>
//     /// 
//     ///     consulta completa
//         public async Task<IEnumerable<RolUser>> GetAllAsync()
//         {
//             return await _context.RolUser.Include(ru => ru.User)
//                                             . Include(ru => ru.Role)
//                                             .ToListAsync(); //hace Un tipo JOIN con LINQ
//         }

//         // consulta por ID
//         public async Task<RolUser?> GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.RolUser.Include(ru => ru.User)
//                                                 .Include(ru => ru.Role)
//                                                 .FirstOrDefaultAsync(ru => ru.Id == id); //hace el mismo join pero lo muestra de forma ordenada
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, $"Error al obtener el RolUser con ID {id}");
//                 throw;
//             }

//         }

//         public async Task<RolUser> CreateAsync(RolUser rolUser)
//         {
//             try
//             {
//                 _context.RolUser.Add(rolUser);
//                 await _context.SaveChangesAsync();
//                 return rolUser;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear el RolUser: {ex.Message}");
//                 throw;
//             }
//         }

//         public async Task<bool> UpdateAsync(RolUser rolUser)
//         {
//             try
//             {
//                 _context.Set<RolUser>().Update(rolUser);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al actualizar el RolUser : {ex.Message}");
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var rolUser = await _context.Set<RolUser>().FindAsync(id);
//                 if (rolUser == null)
//                     return false;
//                 _context.Set<RolUser>().Remove(rolUser);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el RolUser {ex.Message}");
//                 return false;
//             }
//         }

//     /// COLSULTA, CREA, ACTUALIZA Y ELIMINA DE LA BASE DE DATOS EN SQL
//         /// <summary>
//         /// CONSULTA POR MEDIO DE SQL
//         /// </summary>

//          ///consulta completa
//         public async Task<IEnumerable<RolUserDTO>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT 
//                     ru.Id, 
//                     u.Id AS UserId, u.Username AS UserName, 
//                     r.Id AS RoleId, r.Name AS RoleName
//                 FROM RolUser ru
//                 INNER JOIN User u ON ru.UserId = u.Id
//                 INNER JOIN Rol r ON ru.RoleId = r.Id";

//             return await _dapperConnection.QueryAsync<RolUserDTO>(query);
//         }

//         // consulta por ID
//         public async Task<RolUser?> GetByIdAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT 
//                         ru.Id, 
//                         u.Id AS UserId, u.Username AS UserName, 
//                         r.Id AS RoleId, r.Name AS RoleName
//                     FROM RolUser ru
//                     INNER JOIN User u ON ru.UserId = u.Id
//                     INNER JOIN Rol r ON ru.RoleId = r.Id
//                     WHERE ru.Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<RolUser>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, $"Error al obtener el RolUser con ID {id}");
//                 throw;
//             }
//         }

//         //crear un nuevo campo en la base de datos
//         public async Task<int> CreateAsyncSql(RolUser ru)
//         {
//             try
//             {
//                 string sql = @"INSERT INTO RolUser (UserId, RolId)
//                                 VALUES(@UserId, @RolId);
//                                 SELECT CAST(SCOPE_IDENTITY() as int)";
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, new{
//                     ru.UserId,
//                     ru.RoleId
//                 });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex,"Error al crear un nuevo RolUser con SQL");
//                 throw;
//             }
//         }

//         //actualiza un campo en la base de datos con SQL
//         public async Task<bool> UpdateAsyncSql(RolUser ru)
//         {
//             try
//             {
//                 string sql = @"
//                             UPDATE RolUser 
//                             SET 
//                                 RoleId = @RoleId,
//                                 UserId = @UserId
//                             WHERE Id = @Id";
//                 var affectedRows = await  _dapperConnection.ExecuteAsync(sql, new{
//                     ru.RoleId,
//                     ru.UserId
//                 });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar RolUser con SQL");
//                 throw;
//             }
//         }

//         //elimina un campo en la base de datos con SQL
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 string sql = "DELETE FROM RolUser WHERE Id = @Id";

//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, new {Id = id});
//                 return affectedRows >0;
//             }
//             catch(Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar un RolUser con SQL");
//                 throw;
//             }
            
//         }

//         /// <summary>
//         /// Elimina un RolUser de manera logica de la base  de datos SQL
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE RolUser 
//                     SET Active = 0
//                     WHERE Id = @Id;
//                     SELECT CAST(@@ROWCOUNT AS int);";

//                 int rowsAffected = await _dapperConnection.QuerySingleAsync<int>(query, new { Id = id });

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error al eliminar logicamente FormModule: {ex.Message}");
//                 return false;
//             }
//         }
        
//     }
// }
