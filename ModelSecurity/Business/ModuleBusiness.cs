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
//     /// <summary>
//     /// Clase de negocio encargada de la lógica relacionada con los módulos del sistema.
//     /// </summary>
//     public class ModuleBusiness
//     {
//         private readonly ModuleData _moduleData;
//         private readonly ILogger<ModuleBusiness> _logger;

//         public ModuleBusiness(ModuleData moduleData, ILogger<ModuleBusiness> logger)
//         {
//             _moduleData = moduleData;
//             _logger = logger;
//         }

//         /// <summary>
//         /// Obtiene todos los módulos y los mapea a ModuleDTO.
//         /// </summary>
//         public async Task<IEnumerable<ModuleDTO>> GetAllModulesAsync()
//         {
//             try
//             {
//                 var modules = await _moduleData.GetAllAsync();
//                 return MapToDTOList(modules);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener todos los módulos");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de módulos", ex);
//             }
//         }

//         /// <summary>
//         /// Obtiene un módulo por su ID y lo mapea a ModuleDTO.
//         /// </summary>
//         public async Task<ModuleDTO> GetModuleByIdAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó obtener un módulo con ID inválido: {ModuleId}", id);
//                 throw new Utilities.Exceptions.ValidationException("id", "El ID del módulo debe ser mayor que cero");
//             }

//             try
//             {
//                 var module = await _moduleData.GetByIdAsync(id);
//                 if (module == null)
//                 {
//                     _logger.LogInformation("No se encontró ningún módulo con el ID: {ModuleId}", id);
//                     throw new EntityNotFoundException("Module", id);
//                 }

//                 return MapToDTO(module);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el módulo con ID: {ModuleId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar el módulo con ID {id}", ex);
//             }
//         }

//         /// <summary>
//         /// Crea un nuevo módulo a partir de un ModuleDTO.
//         /// </summary>
//         public async Task<ModuleDTO> CreateModuleAsync(ModuleDTO moduleDTO)
//         {
//             try
//             {
//                 ValidateModule(moduleDTO);

//                 // Mapea el DTO a entidad
//                 var module = MapToEntity(moduleDTO);
//                 var createdModule = await _moduleData.CreateAsync(module);

//                 // Mapea la entidad creada de vuelta a DTO y la retorna
//                 return MapToDTO(createdModule);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear nuevo módulo: {ModuleName}", moduleDTO?.Name ?? "null");
//                 throw new ExternalServiceException("Base de datos", "Error al crear el módulo", ex);
//             }
//         }

//         /// <summary>
//         /// Actualiza un módulo existente a partir de un ModuleDTO.
//         /// </summary>
//         public async Task<bool> UpdateModuleAsync(ModuleDTO moduleDTO)
//         {
//             try
//             {
//                 ValidateModule(moduleDTO);

//                 var existingModule = await _moduleData.GetByIdAsync(moduleDTO.Id);
//                 if (existingModule == null)
//                 {
//                     throw new EntityNotFoundException("Module", moduleDTO.Id);
//                 }

//                 // Actualiza las propiedades de la entidad existente
//                 existingModule.Name = moduleDTO.Name;
//                 existingModule.Description = moduleDTO.Description;

//                 return await _moduleData.UpdateAsync(existingModule);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar el módulo con ID: {ModuleId}", moduleDTO.Id);
//                 throw new ExternalServiceException("Base de datos", "Error al actualizar el módulo", ex);
//             }
//         }

//         /// <summary>
//         /// Elimina un módulo a partir de su ID.
//         /// </summary>
//         public async Task<bool> DeleteModuleAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó eliminar un módulo con ID inválido: {Id}", id);
//                 throw new Utilities.Exceptions.ValidationException("id", "El ID del módulo debe ser mayor que cero");
//             }

//             try
//             {
//                 return await _moduleData.DeleteAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el módulo con ID: {ModuleId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al eliminar el módulo con ID {id}", ex);
//             }
//         }


//         /// <summary>
//         /// Elimina un Module de manera logica por ID
//         /// </summary>
//         public async Task<bool> DeleteModuleLogicalAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 throw new ValidationException("ID", "El ID del module debe ser mayor que cero.");
//             }

//             var existingModule = await _moduleData.GetByIdAsync(id);
//             if (existingModule == null)
//             {
//                 throw new EntityNotFoundException("Module", id);
//             }

//             try
//             {

//                 return await _moduleData.DeleteLogicAsyncSQL(id);

//             }
//             catch (ExternalServiceException ex)
//             {
//                 _logger.LogError(ex, "Error en servicio externo al eliminar el module con ID: {FormId}", id);
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el module de manera logica con ID: {FormId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el domule de manera logica.", ex);
//             }
//         }

//         /// <summary>
//         /// Valida el DTO del módulo.
//         /// </summary>
//         private void ValidateModule(ModuleDTO moduleDTO)
//         {
//             if (moduleDTO == null)
//             {
//                 throw new Utilities.Exceptions.ValidationException("El objeto módulo no puede ser nulo");
//             }

//             if (string.IsNullOrWhiteSpace(moduleDTO.Name))
//             {
//                 _logger.LogWarning("Se intentó crear/actualizar un módulo con Name vacío");
//                 throw new Utilities.Exceptions.ValidationException("Name", "El nombre del módulo es obligatorio");
//             }
//         }

//         /// <summary>
//         /// Mapea una entidad Module a ModuleDTO.
//         /// </summary>
//         private ModuleDTO MapToDTO(Module module)
//         {
//             return new ModuleDTO
//             {
//                 Id = module.Id,
//                 Name = module.Name,
//                 Description = module.Description,
//                 Active = module.Active
//             };
//         }

//         /// <summary>
//         /// Mapea un ModuleDTO a una entidad Module.
//         /// </summary>
//         private Module MapToEntity(ModuleDTO moduleDTO)
//         {
//             return new Module
//             {
//                 Id = moduleDTO.Id,
//                 Name = moduleDTO.Name,
//                 Description = moduleDTO.Description,
//                 Active = moduleDTO.Active
                
//             };
//         }

//         /// <summary>
//         /// Mapea una lista de entidades Module a una lista de ModuleDTO.
//         /// </summary>
//         private IEnumerable<ModuleDTO> MapToDTOList(IEnumerable<Module> modules)
//         {
//             return modules.Select(MapToDTO);
//         }
//     }
// }
