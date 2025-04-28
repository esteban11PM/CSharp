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
//     /// Clase de negocio encargada de la lógica relacionada con las Personas del sistema.
//     /// </summary>
//     public class PersonBusiness
//     {
//         private readonly PersonData _personData;
//         private readonly ILogger<PersonBusiness> _logger;

//         public PersonBusiness(PersonData personData, ILogger<PersonBusiness> logger)
//         {
//             _personData = personData;
//             _logger = logger;
//         }

//         /// <summary>
//         /// Obtiene todas las personas y las mapea a PersonDTO.
//         /// </summary>
//         public async Task<IEnumerable<PersonDTO>> GetAllPersonsAsync()
//         {
//             try
//             {
//                 var persons = await _personData.GetAllAsync();
//                 return MapToDTOList(persons);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener todas las personas");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de personas", ex);
//             }
//         }

//         /// <summary>
//         /// Obtiene una persona por su ID y la mapea a PersonDTO.
//         /// </summary>
//         public async Task<PersonDTO> GetPersonByIdAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó obtener una persona con ID inválido: {PersonId}", id);
//                 throw new ValidationException("id", "El ID de la persona debe ser mayor que cero");
//             }

//             try
//             {
//                 var person = await _personData.GetBydIdAsync(id);
//                 if (person == null)
//                 {
//                     _logger.LogInformation("No se encontró ninguna persona con el ID: {PersonId}", id);
//                     throw new EntityNotFoundException("Person", id);
//                 }

//                 return MapToDTO(person);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener la persona con el ID: {PersonId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar la persona con ID {id}", ex);
//             }
//         }

//         /// <summary>
//         /// Crea una nueva persona a partir de un DTO.
//         /// </summary>
//         public async Task<PersonDTO> CreatePersonAsync(PersonDTO personDTO)
//         {
//             try
//             {
//                 ValidatePerson(personDTO);

//                 // Convertir el DTO a entidad para persistirla
//                 var person = MapToEntity(personDTO);
//                 var createdPerson = await _personData.CreateAsync(person);

//                 // Mapeamos la entidad creada de vuelta a DTO
//                 return MapToDTO(createdPerson);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear una nueva persona: {PersonName}", personDTO?.Name ?? "null");
//                 throw new ExternalServiceException("Base de datos", "Error al crear la persona", ex);
//             }
//         }

//         /// <summary>
//         /// Actualiza una persona existente a partir de un DTO.
//         /// </summary>
//         public async Task<bool> UpdatePersonAsync(PersonDTO personDTO)
//         {
//             try
//             {
//                 ValidatePerson(personDTO);

//                 var existingPerson = await _personData.GetBydIdAsync(personDTO.Id);
//                 if (existingPerson == null)
//                 {
//                     throw new EntityNotFoundException("Person", personDTO.Id);
//                 }

//                 // Actualizar las propiedades de la entidad existente
//                 existingPerson.Name = personDTO.Name;
//                 existingPerson.LastName = personDTO.LastName;
//                 existingPerson.Phone = personDTO.Phone;
//                 existingPerson.Email = personDTO.Email;
//                 existingPerson.DocumentNumber = personDTO.DocumentNumber;
//                 existingPerson.Address = personDTO.Address;
//                 existingPerson.DocumentType = personDTO.DocumentType;
//                 existingPerson.BlodType = personDTO.BlodType;
//                 existingPerson.Status = personDTO.Status;

//                 return await _personData.UpdateAsync(existingPerson);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar la persona con ID: {PersonId}", personDTO.Id);
//                 throw new ExternalServiceException("Base de datos", "Error al actualizar la persona", ex);
//             }
//         }

//         /// <summary>
//         /// Elimina una persona a partir de su ID.
//         /// </summary>
//         public async Task<bool> DeletePersonAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó eliminar una persona con ID inválido: {Id}", id);
//                 throw new ValidationException("id", "El ID de la persona debe ser mayor que cero");
//             }

//             try
//             {
//                 return await _personData.DeleteAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar la persona con ID: {PersonId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al eliminar la persona con ID {id}", ex);
//             }
//         }

//         /// <summary>
//         /// Elimina un person de manera logica por ID
//         /// </summary>
//         public async Task<bool> DeletePersonLogicalAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 throw new ValidationException("ID", "El ID del person debe ser mayor que cero.");
//             }

//             var existingPerson = await _personData.GetBydIdAsync(id);
//             if (existingPerson == null)
//             {
//                 throw new EntityNotFoundException("Person", id);
//             }

//             try
//             {
//                 return await _personData.DeleteLogicAsyncSQL(id);

//             }
//             catch (ExternalServiceException ex)
//             {
//                 _logger.LogError(ex, "Error en servicio externo al eliminar el person con ID: {PersonId}", id);
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el person de manera logica con ID: {PersonId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el person de manera logica.", ex);
//             }
//         }

//         /// <summary>
//         /// Valida el DTO de la persona.
//         /// </summary>
//         private void ValidatePerson(PersonDTO personDTO)
//         {
//             if (personDTO == null)
//             {
//                 throw new ValidationException("El objeto persona no puede ser nulo");
//             }

//             if (string.IsNullOrWhiteSpace(personDTO.Name))
//             {
//                 _logger.LogWarning("Se intentó crear/actualizar una persona con Name vacío");
//                 throw new ValidationException("Name", "El nombre de la persona es obligatorio");
//             }
//         }

//         /// <summary>
//         /// Mapea una entidad Person a PersonDTO.
//         /// </summary>
//         private PersonDTO MapToDTO(Person person)
//         {
//             return new PersonDTO
//             {
//                 Id = person.Id,
//                 Name = person.Name,
//                 LastName = person.LastName,
//                 Phone = person.Phone,
//                 Email = person.Email,
//                 DocumentNumber = person.DocumentNumber,
//                 Address = person.Address,
//                 DocumentType = person.DocumentType,
//                 BlodType = person.BlodType,
//                 Status = person.Status
//             };
//         }

//         /// <summary>
//         /// Mapea un PersonDTO a una entidad Person.
//         /// </summary>
//         private Person MapToEntity(PersonDTO personDTO)
//         {
//             return new Person
//             {
//                 Id = personDTO.Id,
//                 Name = personDTO.Name,
//                 LastName = personDTO.LastName,
//                 Phone = personDTO.Phone,
//                 Email = personDTO.Email,
//                 DocumentNumber = personDTO.DocumentNumber,
//                 Address = personDTO.Address,
//                 DocumentType = personDTO.DocumentType,
//                 BlodType = personDTO.BlodType,
//                 Status = personDTO.Status
//             };
//         }

//         /// <summary>
//         /// Mapea una lista de entidades Person a una lista de PersonDTO.
//         /// </summary>
//         private IEnumerable<PersonDTO> MapToDTOList(IEnumerable<Person> persons)
//         {
//             // Se usa LINQ para simplificar el mapeo
//             return persons.Select(MapToDTO);
//         }
//     }
// }
