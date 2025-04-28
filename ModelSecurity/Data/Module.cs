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
//     public class ModuleData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         private readonly ILogger<ModuleData> _logger;

//         public ModuleData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<ModuleData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }

//     /// CONSULTA, CREA, ACTUALIZA Y ELIMINA POR MEDIO DE LINQ
//         // Consulta general
//         public async Task<IEnumerable<Module>> GetAllAsync()
//         {
//             return await _context.Set<Module>().ToListAsync();
//         }

//         // Consulta por Id
//         public async Task<Module?> GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.Set<Module>().FindAsync(id);
//             }
//             catch(Exception ex)
//             {
//                 _logger.LogError($"Error al obtener el formulario: {ex.Message}");
//                 throw;//Re-lanza la excepcion para que sea manejada en capas superiores
//             }
//         }

//         public async Task<Module> CreateAsync(Module module)
//         {
//             try
//             {
//                 await _context.Set<Module>().AddAsync(module);
//                 await _context.SaveChangesAsync();
//                 return module;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear el modulo: {ex.Message}");
//                 throw;
//             }
//         }

//         public async Task<bool> UpdateAsync(Module module)
//         {
//             try
//             {
//                 _context.Set<Module>().Update(module);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al actualizar el modulo : {ex.Message}");
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var module = await _context.Set<Module>().FindAsync(id);
//                 if (module == null)
//                     return false;
//                 _context.Set<Module>().Remove(module);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el modulo {ex.Message}");
//                 return false;
//             }
//         }


//     /// CONSULTA, CREA, ACTUALIZA Y ELIMINA POR MEDIO DE SQL
//         // consulta completa
//         public async Task<IEnumerable<Module>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT Id, Name, Description
//                 FROM Module";

//             return await _dapperConnection.QueryAsync<Module>(query);
//         }
        
//         //consulta por Id
//         public async Task<Module?> GetByIdAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT Id, Name, Description
//                     FROM Module
//                     WHERE Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<Module>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el módulo con ID {ModuleId}", id);
//                 throw;
//             }
//         }

//         /// <summari> Se inserta un nuevo module en la base de datos </summari>
//         /// 
//         public async Task<int> CreateAsyncSql(Module module)
//         {
//             try
//             {
//                 const string sql = @"
//                     INSERT INTO Module (Name, Description)
//                     VALUES (@Name, @Description);
//                     SELECT CAST(SCOPE_IDENTITY() as int);";
                
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, module);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear rol con SQL");
//                 throw;
//             }
//         }

//         /// <summari> Se actualiza un el module en la base de datos </summari>
//         /// 
//         public async Task<bool> UpdateAsyncSql(Module module)
//         {
//             try
//             {
//                 const string sql = @"
//                     UPDATE Module
//                     SET Name = @Name,
//                         Description = @Description
//                     WHERE Id = @Id";
                
//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, module);
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar module con SQL");
//                 throw;
//             }
//         }

//         /// <summari> Se Elimina un module en la base de datos </summari>
//         /// 
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 const string sql = "DELETE FROM Module WHERE Id = @Id";
//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, new { Id = id });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el campo en module con SQL");
//                 throw;
//             }
//         }

//         /// <summary>
//         /// Elimina un Module de manera logica de la base  de datos SQL
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE Module 
//                     SET Active = 0
//                     WHERE Id = @Id;
//                     SELECT CAST(@@ROWCOUNT AS int);";

//                 int rowsAffected = await _dapperConnection.QuerySingleAsync<int>(query, new { Id = id });

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error al eliminar logicamente module: {ex.Message}");
//                 return false;
//             }
//         }
//     }
// }
