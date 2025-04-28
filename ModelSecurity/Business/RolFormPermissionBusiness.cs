// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Data;
// using Entity.DTOs;
// using Entity.Model;
// using Microsoft.Extensions.Logging;
// using Utilities.Exceptions;

// namespace Business
// {
//     public class RolFormPermissionBusiness
//     {
//         private readonly RolFormPermissionData _rolFormPermissionData;
//         private readonly ILogger<RolFormPermissionBusiness> _logger;

//         public RolFormPermissionBusiness(RolFormPermissionData rolFormPermissionData, ILogger<RolFormPermissionBusiness> logger)
//         {
//             _rolFormPermissionData = rolFormPermissionData;
//             _logger = logger;
//         }
        
//         /// <summary>
//         /// Obtiene todos los campos de RolFormPermission como DTOs.
//         /// </summary>
//         public async Task<IEnumerable<RolFormPermissionDTO>> GetAllRolFormPermissionsAsync()
//         {
//             try
//             {
//                 var entities = await _rolFormPermissionData.GetAllAsync();
//                 return MapToDTOList(entities);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener todos los permisos de formulario de rol.");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar los permisos", ex);
//             }
//         }
        
//         /// <summary>
//         /// Obtiene un RolFormPermission por ID como DTO.
//         /// </summary>
//         public async Task<RolFormPermissionDTO> GetRolFormPermissionByIdAsync(int id)
//         {
//             if (id <= 0) throw new ValidationException("id", "El ID debe ser mayor que cero");

//             try
//             {
//                 var entity = await _rolFormPermissionData.GetByIdAsync(id);
//                 if (entity == null) throw new EntityNotFoundException("RolFormPermission", id);

//                 return MapToDTO(entity);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el permiso con ID: {Id}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
//             }
//         }
        
//         /// <summary>
//         /// Crea un nuevo registro de RolFormPermission.
//         /// </summary>
//         public async Task<RolFormPermissionCreateDTO> CreateRolFormPermissionAsync(RolFormPermissionCreateDTO createDTO)
//         {
//             //ValidateRolFormPermission(dto);

//             try
//             {
//                 // ValidateRolUser(rolUserDTO);

//                 var rolUserPermission = MapCreateToEntity(createDTO);
//                 var rolUserPermissionCreado = await _rolFormPermissionData.CreateAsync(rolUserPermission);

//                 //Se reobtiene el usuario con persona
//                 var rolWithUser = (await _rolFormPermissionData.GetAllAsync()).FirstOrDefault(ru => ru.Id == rolUserPermissionCreado.Id);

//                 return MapToCreateDTO(rolWithUser);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear nuevo RolUser: {RolUserId}", createDTO?.Id ?? 0);
//                 throw new ExternalServiceException("Base de datos", "Error al crear el RolUser", ex);
//             }
//         }
        
//         /// <summary>
//         /// Actualiza un registro existente de RolFormPermission.
//         /// </summary>
//         public async Task<RolFormPermissionDTO> UpdateRolFormPermissionAsync(RolFormPermissionCreateDTO createDTO)
//         {
//             // ValidateRolFormPermission(dto);
//             var existingRolFormPermission = await _rolFormPermissionData.GetByIdAsync(createDTO.Id);
//             if (existingRolFormPermission == null)
//             {
//                 throw new EntityNotFoundException("RolPermission", createDTO.Id);
//             }
//             try
//             {
//                 //Actualizar propiedades
//                 existingRolFormPermission.Active = createDTO.Active;
//                 existingRolFormPermission.RolId = createDTO.RolId;
//                 existingRolFormPermission.PermissionId = createDTO.PermissionId;
//                 existingRolFormPermission.FormId = createDTO.FormId;

//                 //llama el data layer para actualizarlo
//                 var updated = await _rolFormPermissionData.UpdateAsync(existingRolFormPermission);
//                 if (!updated)
//                 {
//                     throw new ExternalServiceException("Base de datos", "No se pudo actualizar el RolFormPermission", null);
//                 }
//                 // Vuelve a obtener el RolFormPermission actualizado incluyendo la información de Person
//                 var rolFormPermission = await _rolFormPermissionData.GetByIdAsync(createDTO.Id);
//                 return MapToDTO(rolFormPermission);

//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar el permiso con ID: {Id}", createDTO.Id);
//                 throw new ExternalServiceException("Base de datos", $"Error al actualizar el permiso con ID {createDTO.Id}", ex);
//             }
//         }

//         /// <summary>
//         /// Elimina un RolFormPermission por ID.
//         /// </summary>
//         public async Task DeleteRolFormPermissionAsync(int id)
//         {
//             try
//             {
//                 var rolFormPermission = await _rolFormPermissionData.GetByIdAsync(id);
//                 if (rolFormPermission == null)
//                 {
//                     _logger.LogInformation("No se encontró ningún RolUser con ID: {RolUserId}", id);
//                     throw new EntityNotFoundException("RolUser", id);
//                 }

//                 await _rolFormPermissionData.DeleteAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar RolUser con ID: {RolUserId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el RolUser", ex);
//             }
//         }


//         /// <summary>
//         /// Elimina un FormModule de manera logica por ID
//         /// </summary>
//         public async Task<bool> DeleteRolFormPermissionLogicalAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 throw new ValidationException("ID", "El ID del rolFormPermission debe ser mayor que cero.");
//             }

//             var existingUser = await _rolFormPermissionData.GetByIdAsync(id);
//             if (existingUser == null)
//             {
//                 throw new EntityNotFoundException("RolFormPermission", id);
//             }
//             try
//             {

//                 return await _rolFormPermissionData.DeleteLogicAsyncSQL(id);

//             }
//             catch (ExternalServiceException ex)
//             {
//                 _logger.LogError(ex, "Error en servicio externo al eliminar el rolFormPermission con ID: {RolFormPermissionId}", id);
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el rolFormPermission de manera logica con ID: {rolFormPermissionId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el rolFormPermission de manera logica.", ex);
//             }
//         }

//         // /// <summary>
//         // /// Valida los datos de RolFormPermission.
//         // /// </summary>
//         // private void ValidateRolFormPermission(RolFormPermissionDTO dto)
//         // {
//         //     if (dto == null) throw new ValidationException("El objeto no puede ser nulo");
//         //     if (dto.RoleId <= 0) throw new ValidationException("RoleId", "El RoleId debe ser mayor que cero");
//         //     if (dto.PermissionId <= 0) throw new ValidationException("PermissionId", "El PermissionId debe ser mayor que cero");
//         //     if (dto.FormId <= 0) throw new ValidationException("FormId", "El FormId debe ser mayor que cero");
//         // }

//         /// <summary>
//         /// Mapea un objeto RolFormPermission a RolFormPermissionDTO.
//         /// </summary>
//         private RolFormPermissionDTO MapToDTO(RolFormPermission entity)
//         {
//             return new RolFormPermissionDTO
//             {
//                 Id = entity.Id,
//                 Active = entity.Active,
//                 RolId = entity.RolId,
//                 RoleName = entity.Rol?.Name,
//                 PermissionId = entity.PermissionId,
//                 PermissionName = entity.Permission?.Name,
//                 FormId = entity.FormId,
//                 FormName = entity.Form?.Name
//             };
//         }

//         /// <summary>
//         /// Mapea un objeto RolFormPermissionDTO a RolFormPermission.
//         /// </summary>
//         private RolFormPermission MapToEntity(RolFormPermissionDTO dto)
//         {
//             return new RolFormPermission
//             {
//                 Id = dto.Id,
//                 RolId = dto.RolId,
//                 PermissionId = dto.PermissionId,
//                 FormId = dto.FormId
//             };
//         }

//         /// <summary>
//         /// Mapea un objeto RolFormPermission a RolFormPermissionCreateDTO.
//         /// </summary>
//         /// 
//         public RolFormPermissionCreateDTO MapToCreateDTO(RolFormPermission rolFormPermission)
//         {
//             return new RolFormPermissionCreateDTO
//             {
//                 Id = rolFormPermission.Id,
//                 Active = rolFormPermission.Active,
//                 RolId = rolFormPermission.RolId,
//                 PermissionId = rolFormPermission.PermissionId,
//                 FormId = rolFormPermission.FormId
//             };
//         }

//         /// <summary>
//         /// Mapea un objeto RolFormPermissionCreateDTO a RolFormPermission.
//         /// </summary>
//         /// 
//         public RolFormPermission MapCreateToEntity(RolFormPermissionCreateDTO createDTO)
//         {
//             return new RolFormPermission
//             {
//                 Id = createDTO.Id,
//                 Active = createDTO.Active,
//                 RolId = createDTO.RolId,
//                 PermissionId = createDTO.PermissionId,
//                 FormId = createDTO.FormId
//             };
//         }
//         /// <summary>
//         /// Metodo para mapear una lista de RolFormPermission a una lista de RolFormPermissionDTO 
//         /// </summary>
//         /// <param name="rolFormPermissions"></param>
//         /// <returns></returns>
//         private IEnumerable<RolFormPermissionDTO> MapToDTOList(IEnumerable<RolFormPermission> rolesUsersPermissions)
//         {
//             return rolesUsersPermissions.Select(MapToDTO);
//         }
//     }
// }
