// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Data;
// using Entity.DTOs;
// using Entity.Model;
// using Microsoft.Extensions.Logging;
// using Utilities.Exceptions;

// namespace Business
// {
//     ///<summary>
//     ///Clase de negocio encargada de la logica relacionada con la auditoria del sistema;
//     ///</summary>
//     public class AuditLogBusiness
//     {
//         private readonly AuditLogData _auditLogData;
//         private readonly ILogger<AuditLogBusiness> _logger;

//         public AuditLogBusiness(AuditLogData auditLogData, ILogger<AuditLogBusiness> logger)
//         {
//             _auditLogData = auditLogData;
//             _logger = logger;
//         }

//         // Método para obtener todos los auditLog como DTOs
//         public async Task<IEnumerable<AuditLogDTO>> GetAllAuditLogAsync()
//         {
//             try
//             {
//                 var auditLogs = await _auditLogData.GetAllAsync();
//                 var auditLogDTOs = new List<AuditLogDTO>();

//                 foreach (var auditLog in auditLogs)
//                 {
//                     auditLogDTOs.Add(new AuditLogDTO
//                     {
                        
//                         Id = auditLog.Id,
//                         TableName = auditLog.TableName,
//                         AffectedId = auditLog.AffectedId,
//                         PropertyName = auditLog.PropertyName,
//                         OldValue = auditLog.OldValue,
//                         NewValue = auditLog.NewValue,
//                         Action = auditLog.NewValue,
//                         Timestamp = auditLog.Timestamp,
//                     });
//                 }

//                 return auditLogDTOs;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener todos los auditLog");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de auditLog", ex);
//             }
//         }

//         // Método para obtener un auditLog por ID como DTO
//         public async Task<AuditLogDTO> GetRolByIdAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó obtener un auditLog con ID inválido: {AuditLogId}", id);
//                 throw new Utilities.Exceptions.ValidationException("id", "El ID del auditLog debe ser mayor que cero");
//             }

//             try
//             {
//                 var auditLog = await _auditLogData.GetByIdAsync(id);
//                 if (auditLog == null)
//                 {
//                     _logger.LogInformation("No se encontró ningún auditLog con ID: {AuditLogId}", id);
//                     throw new EntityNotFoundException("AuditLog", id);
//                 }

//                 return new AuditLogDTO
//                 {
//                     Id = auditLog.Id,
//                     TableName = auditLog.TableName,
//                     AffectedId = auditLog.AffectedId,
//                     PropertyName = auditLog.PropertyName,
//                     OldValue = auditLog.OldValue,
//                     NewValue = auditLog.NewValue,
//                     Action = auditLog.NewValue,
//                     Timestamp = auditLog.Timestamp,
//                 };
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el auditLog con ID: {AuditLogId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar el AuditLog con ID {id}", ex);
//             }
//         }

//         // Método para crear un auditLog desde un DTO
//         public async Task<AuditLogDTO> CreateAuditLogAsync(AuditLogDTO auditLogDto)
//         {
//             try
//             {
//                 ValidateAuditLog(auditLogDto);

//                 var auditLog = new AuditLog
//                 {
//                     TableName = auditLogDto.TableName,
//                     AffectedId = auditLogDto.AffectedId,
//                     PropertyName = auditLogDto.PropertyName,
//                     OldValue = auditLogDto.OldValue,
//                     NewValue = auditLogDto.NewValue,
//                     Action = auditLogDto.NewValue,
//                     Timestamp = auditLogDto.Timestamp,
//                 };

//                 var auditLogCreado = await _auditLogData.CreateAsync(auditLog);

//                 return new AuditLogDTO
//                 {
//                     Id = auditLogCreado.Id,
//                     TableName = auditLogCreado.TableName,
//                     AffectedId = auditLogCreado.AffectedId,
//                     PropertyName = auditLogCreado.PropertyName,
//                     OldValue = auditLogCreado.OldValue,
//                     NewValue = auditLogCreado.NewValue,
//                     Action = auditLogCreado.NewValue,
//                     Timestamp = auditLogCreado.Timestamp,
//                 };
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear nuevo auditLog: {AuditLogNombre}", auditLogDto?.TableName ?? "null");
//                 throw new ExternalServiceException("Base de datos", "Error al crear el auditLog", ex);
//             }
//         }

//         // Método para validar el DTO
//         private void ValidateAuditLog(AuditLogDTO auditLogDTO)
//         {
//             if (auditLogDTO == null)
//             {
//                 throw new Utilities.Exceptions.ValidationException("El objeto auditLog no puede ser nulo");
//             }

//             if (string.IsNullOrWhiteSpace(auditLogDTO.TableName))
//             {
//                 _logger.LogWarning("Se intentó crear/actualizar un auditLog con TableName vacío");
//                 throw new Utilities.Exceptions.ValidationException("TableName", "El TableName del AuditLog es obligatorio");
//             }
//         }

//         // Método para mapear el AuditLog a AuditLogDTO
//         private AuditLogDTO MapToDTO(AuditLog auditLog)
//         {
//             return new AuditLogDTO
//             {
//                 Id = auditLog.Id,
//                 TableName = auditLog.TableName,
//                 AffectedId = auditLog.AffectedId,
//                 PropertyName = auditLog.PropertyName,
//                 OldValue = auditLog.OldValue,
//                 NewValue = auditLog.NewValue,
//                 Action = auditLog.NewValue,
//                 Timestamp = auditLog.Timestamp,

//             };
//         }

//         // Método para Mapear de AuditLogDTO a auditLog
//         private AuditLog MapToEntity(AuditLogDTO auditLogDTO)
//         {
//             return new AuditLog
//             {
//                 Id = auditLogDTO.Id,
//                 TableName = auditLogDTO.TableName,
//                 AffectedId = auditLogDTO.AffectedId,
//                 PropertyName = auditLogDTO.PropertyName,
//                 OldValue = auditLogDTO.OldValue,
//                 NewValue = auditLogDTO.NewValue,
//                 Action = auditLogDTO.NewValue,
//                 Timestamp = auditLogDTO.Timestamp,
                
//             };
//         }

//         // Método para mapear una lista de auditLog a una lista de AuditLogDTO
//         private IEnumerable<AuditLogDTO> MapToDTOList(IEnumerable<AuditLog> auditLogs)
//         {
//             var auditLodDTOs = new List<AuditLogDTO>();
//             foreach(var audit in auditLogs)
//             {
//                 auditLodDTOs.Add(MapToDTO(audit));
//             }
//             return auditLodDTOs;
//         }
//     }
// }
