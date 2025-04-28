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
//     ///Clase de negocio encargada de la lógica relacionada con los permisos del sistema
//     ///</summary>
//     public class PermissionBusiness
//     {
//         private readonly PermissionData _permissionData;
//         private readonly ILogger<PermissionBusiness> _logger;

//         public PermissionBusiness(PermissionData permissionData, ILogger<PermissionBusiness> logger)
//         {
//             _permissionData = permissionData;
//             _logger = logger;
//         }

//         // Método para obtener todos los permisos como DTOs
//         public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
//         {
//             try
//             {
//                 var permissions = await _permissionData.GetAllAsync();
//                 return MapToDTOList(permissions);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener todos los permisos");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
//             }
//         }

//         // Método para obtener un permiso por Id
//         public async Task<PermissionDTO> GetPermissionByIdAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó obtener un permiso con ID inválido: {PermissionId}", id);
//                 throw new ValidationException("id", "El ID del permiso debe ser mayor que cero");
//             }
//             try
//             {
//                 var permission = await _permissionData.GetByIdAsync(id);
//                 return permission == null
//                     ? throw new EntityNotFoundException("Permission", id)
//                     : MapToDTO(permission);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el permiso con ID: {PermissionId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
//             }
//         }

//         // Método para crear un permiso desde un DTO
//         public async Task<PermissionDTO> CreatePermissionAsync(PermissionDTO permissionDTO)
//         {
//             try
//             {
//                 ValidatePermission(permissionDTO);

//                 var permission = MapToEntity(permissionDTO);
//                 var createdPermission = await _permissionData.CreateAsync(permission);

//                 return MapToDTO(createdPermission);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear nuevo permiso: {PermissionNombre}", permissionDTO?.Name ?? "null");
//                 throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
//             }
//         }

//         // Método para actualizar un permiso
//         public async Task<bool> UpdatePermissionAsync(PermissionDTO permissionDTO)
//         {
//             try
//             {
//                 ValidatePermission(permissionDTO);

//                 var existingPermission = await _permissionData.GetByIdAsync(permissionDTO.Id);
//                 if (existingPermission == null)
//                 {
//                     throw new EntityNotFoundException("Permission", permissionDTO.Id);
//                 }

//                 existingPermission.Name = permissionDTO.Name;

//                 return await _permissionData.UpdateAsync(existingPermission);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar el permiso con ID: {PermissionId}", permissionDTO.Id);
//                 throw new ExternalServiceException("Base de datos", "Error al actualizar el permiso", ex);
//             }
//         }

//         // Método para eliminar un permiso
//         public async Task<bool> DeletePermissionAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó eliminar un permiso con ID inválido: {Id}", id);
//                 throw new ValidationException("id", "El ID del permiso debe ser mayor que cero");
//             }
//             return await _permissionData.DeleteAsync(id);
//         }

//         /// <summary>
//         /// Elimina un formulario de manera logica por ID
//         /// </summary>
//         public async Task<bool> DeletePermissionLogicalAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 throw new ValidationException("ID", "El ID del Permission debe ser mayor que cero.");
//             }

//             var existingPermission = await _permissionData.GetByIdAsync(id);
//             if (existingPermission == null)
//             {
//                 throw new EntityNotFoundException("Form", id);
//             }

//             try
//             {

//                 return await _permissionData.DeleteLogicAsyncSQL(id);

//             }
//             catch (ExternalServiceException ex)
//             {
//                 _logger.LogError(ex, "Error en servicio externo al eliminar el Permission con ID: {FormId}", id);
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el Permission de manera logica con ID: {FormId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el Permission de manera logica.", ex);
//             }
//         }

//         // Método para validar el DTO
//         private void ValidatePermission(PermissionDTO permissionDTO)
//         {
//             if (permissionDTO == null)
//             {
//                 throw new ValidationException("El objeto permiso no puede ser nulo");
//             }
//             if (string.IsNullOrWhiteSpace(permissionDTO.Name))
//             {
//                 _logger.LogWarning("Se intentó crear/actualizar un permiso con Name vacío");
//                 throw new ValidationException("Name", "El nombre del permiso es obligatorio");
//             }
//         }

//         // Método para mapear un Permission a PermissionDTO
//         private PermissionDTO MapToDTO(Permission permission)
//         {
//             return new PermissionDTO
//             {
//                 Id = permission.Id,
//                 Name = permission.Name,
//                 Description = permission.Description,
//                 Active = permission.Active
//             };
//         }

//         // Método para mapear de PermissionDTO a Permission
//         private Permission MapToEntity(PermissionDTO permissionDTO)
//         {
//             return new Permission
//             {
//                 Id = permissionDTO.Id,
//                 Name = permissionDTO.Name,
//                 Description = permissionDTO.Description,
//                 Active = permissionDTO.Active
//             };
//         }

//         // Método para mapear una lista de Permission a una lista de PermissionDTO
//         private IEnumerable<PermissionDTO> MapToDTOList(IEnumerable<Permission> permissions)
//         {
//             return permissions.Select(MapToDTO);
//         }
//     }
// }