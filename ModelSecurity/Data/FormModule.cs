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
//     public class FormModuleData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         private readonly ILogger<FormModuleData> _logger;

//         public FormModuleData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<FormModuleData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }

//     //CONSULTA POR MEDIO DE LINQ
//         //consulta completa
//         public async Task<IEnumerable<FormModule>> GetAllAsync()
//         {
//             return await _context.FormModule.Include(fm => fm.Module)
//                                             .Include(fm => fm.Form)
//                                             .ToListAsync(); //especie de Join con LINQ
//         }

//         //consulta por ID
//         public async Task<FormModule?> GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.FormModule.Include(fm => fm.Module)
//                                                 .Include(fm => fm.Form)
//                                                 .FirstOrDefaultAsync(fm => fm.Id == id); //consulta con las llaves foraneas
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, $"Error al obtener el FormModule con ID {id}");
//                 throw;
//             }

//         }

//         public async Task<FormModule> CreateAsync(FormModule formModule)
//         {
//             try
//             {
//                 _context.FormModule.Add(formModule);
//                 await _context.SaveChangesAsync();
//                 return formModule;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear el FormModule: {ex.Message}");
//                 throw;
//             }
//         }

//         public async Task<bool> UpdateAsync(FormModule formModule)
//         {
//             try
//             {
//                 _context.Set<FormModule>().Update(formModule);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al actualizar el FormModule : {ex.Message}");
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var formModule = await _context.Set<FormModule>().FindAsync(id);
//                 if (formModule == null)
//                     return false;
//                 _context.Set<FormModule>().Remove(formModule);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el FormModule {ex.Message}");
//                 return false;
//             }
//         }

//     //CONSULTA, CREA, ACTUALIZA, ELIMINA DE LA BASE DE DATOS CON SQL
//         //consulta completa
//         public async Task<IEnumerable<FormModule>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT fm.Id, 
//                     fm.FormId, f.Name AS FormName, 
//                     fm.ModuleId, m.Name AS ModuleName
//                 FROM FormModule fm
//                 INNER JOIN Form f ON fm.FormId = f.Id
//                 INNER JOIN Module m ON fm.ModuleId = m.Id";

//             return await _dapperConnection.QueryAsync<FormModule>(query);
//         }

//         //consulta por ID
//         public async Task<FormModule?> GetByIdAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT fm.Id, 
//                         fm.FormId, f.Name AS FormName, 
//                         fm.ModuleId, m.Name AS ModuleName
//                     FROM FormModule fm
//                     INNER JOIN Form f ON fm.FormId = f.Id
//                     INNER JOIN Module m ON fm.ModuleId = m.Id
//                     WHERE fm.Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<FormModule>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el FormModule con ID {FormModuleId}", id);
//                 throw;
//             }
//         }

//         // crea un nuevo campo en la base de datos con SQL
//         public async Task<int> CreateAsyncSql(FormModule fm)
//         {
//             try
//             {
//                 string sql = @"INSERT INTO FromModule (FormId, ModuleId)
//                                 VALUES(@FormId, @ModuleId);
//                                 SELECT CAST(SCOPE_IDENTITY() as int)";
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, new{
//                     fm.FormId,
//                     fm.ModuleId
//                 });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex,"Error al crear un nuevo FormModule con SQL");
//                 throw;
//             }
//         }

//         // actualiza un campo en la base de datos con SQL
//         public async Task<bool> UpdateAsyncSql(FormModule fm)
//         {
//             try
//             {
//                 string sql = @"
//                             UPDATE FormModule 
//                             SET 
//                                 FormId = @RoleId,
//                                 ModuleId = @UserId
//                             WHERE Id = @Id";
//                 var affectedRows = await  _dapperConnection.ExecuteAsync(sql, new{
//                     fm.FormId,
//                     fm.ModuleId
//                 });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar FormModule con SQL");
//                 throw;
//             }
//         }

//         // elimina un campo de la entidad en la base de datos con SQL
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 string sql = "DELETE FROM FormModule WHERE Id = @Id";

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
//         /// Elimina un FormModule de manera logica de la base  de datos SQL
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE FormModule 
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
