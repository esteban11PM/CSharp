// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection.Metadata;
// using System.Threading.Tasks;
// using Data;
// using Entity.DTOs;
// using Entity.Model;
// using Microsoft.Extensions.Logging;
// using Utilities.Exceptions;

// namespace Business
// {
//     ///<summary>
//     ///Clase de negocio encargada de la logica relacionada con los Usuarios del sistema;
//     ///</summary>
//     public class UserBusiness
//     {
//         private readonly UserData _userData;
//         private readonly ILogger<UserBusiness> _logger;

//         public UserBusiness(UserData userData, ILogger<UserBusiness> logger)
//         {
//             _userData = userData;
//             _logger = logger;
//         }

//         /// <summary>
//         /// Obtiene todos los usuarios como DTOs.
//         /// </summary>
//         public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
//         {
//             try
//             {
//                 var users = await _userData.GetAllUsersAsync();
//                 return MapToDTOList(users);
                
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener todos los usuarios");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
//             }
//         }

//         /// <summary>
//         /// Obtiene un usuario por ID como DTO.
//         /// </summary>
//         public async Task<UserDTO> GetUserByIdAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó obtener un usuario con ID inválido: {UserId}", id);
//                 throw new ValidationException("id", "El ID del usuario debe ser mayor que cero");
//             }

//             try
//             {
//                 var user = await _userData.GetByIdAsync(id);
//                 return user == null
//                     ? throw new EntityNotFoundException("Usuario", id)
//                     : MapToDTO(user);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
//             }
//         }

//         /// <summary>
//         /// Crea un nuevo usuario en la entidad.
//         /// </summary>
//         public async Task<UserCreateDTO> CreateUserAsync(UserCreateDTO userDTO)
//         {
//             try
//             {
//                 // Validar el PersonId
//                 var user = MapCreateToEntity(userDTO);
//                 var userCreated = await _userData.CreateAsync(user);

//                 // Se reobtine el usuario con Person
//                 var userWithPerson = (await _userData.GetAllUsersAsync())
//                     .FirstOrDefault(u => u.Id == userCreated.Id);

//                 return MapToCreateDTO(userWithPerson);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear nuevo usuario: {Username}", userDTO?.Username ?? "null");
//                 throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
//             }
//         }

//         /// <summary>
//         /// Actualiza un usuario existente.
//         /// </summary>
//         public async Task<UserDTO> UpdateUserAsync(UserCreateDTO userCreateDTO)
//         {

//             //ValidateUser(userDTO);

//             var existingUser = await _userData.GetByIdAsync(userCreateDTO.Id);
//             if (existingUser == null)
//             {
//                 throw new EntityNotFoundException("User", userCreateDTO.Id);
//             }
//             try
//             {
//                 // Actualizar propiedades
//                 existingUser.Username = userCreateDTO.Username;
//                 existingUser.Password = userCreateDTO.Password;
//                 existingUser.State = userCreateDTO.State;
//                 existingUser.PersonId = userCreateDTO.PersonId;

//                 // Llamar al Data layer para actualizar
//                 var updated = await _userData.UpdateAsync(existingUser);
//                 if (!updated)
//                 {
//                     throw new ExternalServiceException("Base de datos", "No se pudo actualizar el usuario", null);
//                 }

//                 // Vuelve a obtener el usuario actualizado incluyendo la información de Person
//                     var userWithPerson = await _userData.GetByIdAsync(userCreateDTO.Id);
//                     return MapToDTO(userWithPerson);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar el usuario con ID: {UserId}", userCreateDTO.Id);
//                 throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
//             }
//         }

//         /// <summary>
//         /// Elimina un usuario por ID.
//         /// </summary>
//         public async Task<bool> DeleteUserAsync(int id)
//         {
//             try
//             {
//                 var existingUser = await _userData.GetByIdAsync(id);
//                 if (existingUser == null)
//                 {
//                     throw new EntityNotFoundException("User", id);
//                 }

//                 return await _userData.DeleteAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el usuario con ID: {UserId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el usuario.", ex);
//             }
//         }


//         /// <summary>
//         /// Valida los datos del usuario.
//         /// </summary>
//         private void ValidateUser(UserDTO userDTO)
//         {
//             if (userDTO == null)
//             {
//                 throw new ValidationException("El objeto usuario no puede ser nulo");
//             }

//             if (string.IsNullOrWhiteSpace(userDTO.Username))
//             {
//                 _logger.LogWarning("Se intentó crear/actualizar un usuario con nombre vacío");
//                 throw new ValidationException("Username", "El nombre de usuario es obligatorio");
//             }
//         }

//         /// <summary>
//         /// Elimina un user de manera logica por ID
//         /// </summary>
//         public async Task<bool> DeleteUserLogicalAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 throw new ValidationException("ID", "El ID del user debe ser mayor que cero.");
//             }

//             var existingUser = await _userData.GetByIdAsync(id);
//             if (existingUser == null)
//             {
//                 throw new EntityNotFoundException("Form", id);
//             }

//             try
//             {

//                 return await _userData.DeleteLogicAsyncSQL(id);

//             }
//             catch (ExternalServiceException ex)
//             {
//                 _logger.LogError(ex, "Error en servicio externo al eliminar el user con ID: {UserId}", id);
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el user de manera logica con ID: {UserId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el user de manera logica.", ex);
//             }
//         }

//         /// <summary>
//         /// Mapea un objeto User a UserDTO.
//         /// </summary>
//         private UserDTO MapToDTO(User user)
//         {
//             return new UserDTO
//             {
//                 Id = user.Id,
//                 Username = user.Username,
//                 State = user.State,
//                 PersonId = user.PersonId,
//                 PersonName = user.Person?.Name
//             };
//         }

//         /// <summary>
//         /// Mapea un objeto UserDTO a User.
//         /// </summary>
//         private User MapToEntity(UserDTO userDTO)
//         {
//             return new User
//             {
//                 Id = userDTO.Id,
//                 Username = userDTO.Username,
//                 State = userDTO.State,
//                 PersonId = userDTO.PersonId,
//             };
//         }

//         /// <summary>
//         /// Mapea un objeto User a UserCreateDTO.
//         /// </summary>
//         private UserCreateDTO MapToCreateDTO(User user)
//         {
//             return new UserCreateDTO
//             {
//                 Id = user.Id,
//                 Username = user.Username,
//                 Password = user.Password,
//                 State= user.State,
//                 PersonId = user.PersonId
//             };

//         }

//         private User MapCreateToEntity(UserCreateDTO userCreateDTO)
//         {
//             return new User
//             {
//                 Id = userCreateDTO.Id,
//                 Username = userCreateDTO.Username,
//                 Password = userCreateDTO.Password,
//                 State = userCreateDTO.State,
//                 PersonId = userCreateDTO.PersonId
//             };
//         }

//         /// <summary>
//         /// Metodo para mapear una lista de UserRol a una lista de RolUserDTO 
//         /// </summary>
//         /// <param name="users"></param>
//         /// <returns></returns>
//         private IEnumerable<UserDTO> MapToDTOList(IEnumerable<User> users)
//         {
//             return users.Select(MapToDTO);
//         }
//     }
// }
