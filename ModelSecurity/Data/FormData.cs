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
//     public class FormData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         private readonly ILogger<FormData> _logger;

//         public FormData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<FormData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }
        
//     /// CONSULTA, CREA, ACTUALIZA Y ELIMINA POR MEDIO DE LINQ
//         //consulta completa
//         public async Task<IEnumerable<Form>> GetAllAsync()
//         {
//             return await _context.Set<Form>().ToListAsync();
//         }

//         //consulta por ID
//         public async Task<Form?> GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.Set<Form>().FindAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al obtener el formulario:{ex.Message}");
//                 throw;//Re-lanza la excepcion para que sea manejada en capas superiores
//             }
//         }

//         //crea un nuevo form
//         public async Task<Form> CreateAsync(Form form)
//         {
//             try
//             {
//                 await _context.Set<Form>().AddAsync(form);
//                 await _context.SaveChangesAsync();
//                 return form;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear el formulario: {ex.Message}");
//                 throw;
//             }
//         }

//         //actualiza un form
//         public async Task<bool> UpdateAsync(Form form)
//         {
//             try
//             {
//                 _context.Set<Form>().Update(form);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al actualizar el formulario : {ex.Message}");
//                 return false;
//             }
//         }

//         //elimina un form
//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var form = await _context.Set<Form>().FindAsync(id);
//                 if (form == null)
//                     return false;
//                 _context.Set<Form>().Remove(form);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el formulario {ex.Message}");
//                 return false;
//             }
//         }

//         ///<summary>
//         /// eliminador logico de un form
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncLINQ(int id)
//         {
//             try
//             {
//                 // 1. Buscar el registro por ID
//                 var formToDelete = await _context.Form
//                     .FirstOrDefaultAsync(f => f.Id == id);

//                 if (formToDelete == null)
//                     return false; // No existe el registro

//                 // 2. Marcar como inactivo
//                 formToDelete.Active = false;

//                 // 3. Guardar cambios
//                 int rowsAffected = await _context.SaveChangesAsync();

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error al eliminar lógicamente el formulario: {ex.Message}");
//                 return false;
//             }
//         }


//     ///<summary>
//      /// CONSULTA, CREA, ACTUALIZA Y ELIMINA POR MEDIO DE SQL
//     /// </summary>
//         //consulta completa
//         public async Task<IEnumerable<Form>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT f.Id, f.Name, f.Description
//                 FROM Form f";

//             return await _dapperConnection.QueryAsync<Form>(query);
//         }

//         //consulta por ID
//         public async Task<Form?> GetByIdAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT f.Id, f.Name, f.Description
//                     FROM Form f
//                     WHERE f.Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<Form>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el formulario con ID {FormId}", id);
//                 throw;
//             }
//         }

//         /// <summari> Se inserta un nuevo form en la base de datos </summari>
//         /// 
//         public async Task<int> CreateAsyncSql(Form form)
//         {
//             try
//             {
//                 const string sql = @"
//                     INSERT INTO Form (Name, Description)
//                     VALUES (@Name, @Description);
//                     SELECT CAST(SCOPE_IDENTITY() as int);";
                
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, form);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear Form con SQL");
//                 throw;
//             }
//         }

//         /// <summari> Se actualiza un el form en la base de datos </summari>
//         /// 
//         public async Task<bool> UpdateAsyncSql(Form form)
//         {
//             try
//             {
//                 const string sql = @"
//                     UPDATE Form
//                     SET Name = @Name,
//                         Description = @Description
//                     WHERE Id = @Id";
                
//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, form);
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar form con SQL");
//                 throw;
//             }
//         }

//         /// <summari> Se Elimina un form en la base de datos </summari>
//         /// 
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 const string sql = "DELETE FROM Form WHERE Id = @Id";
//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, new { Id = id });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar form con SQL");
//                 throw;
//             }
//         }

//         /// <summary>
//         /// Elimina un FormData de manera logica de la base  de datos SQL
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE Form 
//                     SET Active = 0
//                     WHERE Id = @Id;
//                     SELECT CAST(@@ROWCOUNT AS int);"; 

//                 int rowsAffected = await _dapperConnection.QuerySingleAsync<int>(query, new { Id = id });

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error al eliminar logicamente form: {ex.Message}");
//                 return false;
//             }
//         }
//     }
// }
