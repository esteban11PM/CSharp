// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Entity.Contexts;
// using Entity.DTOs;
// using Entity.Model;
// using Dapper;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using System.Data;

// namespace Data
// {
//     public class AuditLogData
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         private readonly ILogger<AuditLogData> _logger;

//         public AuditLogData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<AuditLogData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _logger = logger;
//         }
//     //CONSULTAS POR MEDIO DE LINQ
//         //consulta completa
//         public async Task<IEnumerable<AuditLog>> GetAllAsync()
//         {
//             return await _context.Set<AuditLog>().ToListAsync();
//         }

//         //consulta por ID
//         public async Task<AuditLog?> GetByIdAsync(int id)
//         {
//             try
//             {
//                 return await _context.Set<AuditLog>().FindAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener la auditoria con ID {AuditLogId}", id);
//                 throw;
//             }

//         }
//     //CONSULTAS POR MEDIO DE SQL
//         //consulta completa
//         public async Task<IEnumerable<AuditLogDTO>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT al.Id, al.TableName, al.AffectedId, al.PropertyName, 
//                     al.OldValue, al.NewValue, al.Action, al.Timestamp, 
//                     al.UserId, u.Username as UserName
//                 FROM AuditLog al
//                 INNER JOIN User u ON al.UserId = u.Id";

//             return await _dapperConnection.QueryAsync<AuditLogDTO>(query);
//         }

//         //consulta por ID
//         public async Task<AuditLogDTO?> GetByIdAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT al.Id, al.TableName, al.AffectedId, al.PropertyName, 
//                         al.OldValue, al.NewValue, al.Action, al.Timestamp, 
//                         al.UserId, u.Username as UserName
//                     FROM AuditLog al
//                     INNER JOIN User u ON al.UserId = u.Id
//                     WHERE al.Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<AuditLogDTO>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener la auditoría con ID {AuditLogId}", id);
//                 throw;
//             }
//         }


//         public async Task<AuditLog> CreateAsync(AuditLog auditLog)
//         {
//             try
//             {
//                 await _context.Set<AuditLog>().AddAsync(auditLog);
//                 await _context.SaveChangesAsync();
//                 return auditLog;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear la auditoria: {ex.Message}");
//                 throw;
//             }
//         }

//         public async Task<bool> UpdateAsync(AuditLog auditLog)
//         {
//             try
//             {
//                 _context.Set<AuditLog>().Update(auditLog);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al actualizar la auditoria : {ex.Message}");
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var auditLog = await _context.Set<AuditLog>().FindAsync(id);
//                 if (auditLog == null)
//                     return false;
//                 _context.Set<AuditLog>().Remove(auditLog);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar la auditoria {ex.Message}");
//                 return false;
//             }
//         }
//     }
// }
