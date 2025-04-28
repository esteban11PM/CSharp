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
//     ///Clase de negocio encargada de la logica relacionada con los roles del sistema;
//     ///</summary>
//     public class RolBusiness
//     {
//         private readonly RolData _rolData;
//         private readonly ILogger<RolBusiness> _logger;

//         public RolBusiness(RolData rolData, ILogger<RolBusiness> logger)
//         {
//             _rolData = rolData;
//             _logger = logger;
//         }

//         // Método para obtener todos los roles como DTOs
//         public async Task<IEnumerable<RolDTO>> GetAllRolesAsync()
//         {
//             try
//             {
//                 var roles = await _rolData.GetAllAsync();
//                 return MapToDTOList(roles);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener todos los roles");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
//             }
//         }

//         // Método para obtener un rol por ID como DTO
//         public async Task<RolDTO> GetRolByIdAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
//                 throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
//             }

//             try
//             {
//                 var rol = await _rolData.GetByIdAsync(id);
//                 return rol == null
//                     ? throw new EntityNotFoundException("Role", id)
//                     : MapToDTO(rol);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
//             }
//         }

//         // Método para crear un rol desde un DTO
//         public async Task<RolDTO> CreateRolAsync(RolDTO rolDto)
//         {
//             try
//             {
//                 ValidateRol(rolDto);

//                 var rol = MapToEntity(rolDto);
//                 var rolCreado = await _rolData.CreateAsync(rol);

//                 return MapToDTO(rolCreado);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", rolDto?.Name ?? "null");
//                 throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
//             }
//         }

//         private Rol MapToDTO(RolDTO rolDto)
//         {
//             throw new NotImplementedException();
//         }

//         //Metodo para Actualizar un rol desde un DTO
//         public async Task<bool> UpdateRolAsync(RolDTO rolDTO)
//         {
//             try
//             {
//                 ValidateRol(rolDTO);

//                 var existingRol = await _rolData.GetByIdAsync(rolDTO.Id);
//                 if (existingRol == null)
//                 {
//                     throw new EntityNotFoundException("Rol", rolDTO.Id);
//                 }

//                 // Actualizar propiedades
//                 existingRol.Id = rolDTO.Id;
//                 existingRol.Name = rolDTO.Name;
//                 existingRol.Description = rolDTO.Description;
//                 existingRol.Active = rolDTO.Active;

//                 return await _rolData.UpdateAsync(existingRol);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar el rol con ID: {RolId}", rolDTO.Id);
//                 throw new ExternalServiceException("Base de datos", "Error al actualizar el rol.", ex);
//             }
//         }

//         //Método para eliminar un rol desde un DTO
//         public async Task<bool> DeleteRolAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó eliminar un rol con ID inválido: {Id}", id);
//                 throw new ValidationException("id", "El ID del rol debe ser mayor que cero");
//             }

//             return await _rolData.DeleteAsync(id);
//         }

//         /// <summary>
//         /// Elimina un rol de manera logica por ID.
//         /// </summary>
//         public async Task<bool> SoftDeleteRolAsync(int id)
//         {
//             var rol = await _rolData.GetByIdAsyncSql(id);
//             if (rol == null)
//             {
//                 throw new EntityNotFoundException("Rol", id);
//             }

//             return await _rolData.SoftDeleteAsyncSQL(id);
//         }



//         // Método para validar el DTO
//         private void ValidateRol(RolDTO RolDto)
//         {
//             if (RolDto == null)
//             {
//                 throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
//             }

//             if (string.IsNullOrWhiteSpace(RolDto.Name))
//             {
//                 _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
//                 throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
//             }
//         }

//         // Método para mapear el Rol a RolDTO
//         private RolDTO MapToDTO(Rol rol)
//         {
//             return new RolDTO
//             {
//                 Id = rol.Id,
//                 Name = rol.Name,
//                 Active = rol.Active,
//                 Description = rol.Description

//             };
//         }

//         // Método para Mapear de RolDTO a rol
//         private Rol MapToEntity(RolDTO rolDTO)
//         {
//             return new Rol
//             {
//                 Id = rolDTO.Id,
//                 Name = rolDTO.Name,
//                 Active = rolDTO.Active,
//                 Description = rolDTO.Description
//             };
//         }

//         // Método para mapear una lista de Rol a una lista de RolDTO
//         private IEnumerable<RolDTO> MapToDTOList(IEnumerable<Rol> rols)
//         {
//             return rols.Select(MapToDTO);
//         }
//     }
// }
