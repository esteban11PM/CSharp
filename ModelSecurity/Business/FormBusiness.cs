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
//     /// Clase de negocio encargada de la lógica relacionada con los formularios del sistema.
//     /// </summary>
//     public class FormBusiness
//     {
//         private readonly FormData _formData;
//         private readonly ILogger<FormBusiness> _logger;

//         /// <summary>
//         /// Constructor que inyecta las dependencias necesarias.
//         /// </summary>
//         /// <param name="formData">Instancia de FormData para operaciones de datos.</param>
//         /// <param name="logger">Logger tipificado para FormBusiness.</param>
//         public FormBusiness(FormData formData, ILogger<FormBusiness> logger)
//         {
//             _formData = formData;
//             _logger = logger;
//         }

//         /// <summary>
//         /// Método para obtener todos los formularios como DTOs.
//         /// </summary>
//         /// <returns>Lista de FormDTO</returns>
//         public async Task<IEnumerable<FormDTO>> GetAllFormsAsync()
//         {
//             try
//             {
//                 var forms = await _formData.GetAllAsync();
//                 return MapToDTOList(forms);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener los formularios");
//                 throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
//             }
//         }

//         /// <summary>
//         /// Método para obtener un formulario por su ID como DTO.
//         /// </summary>
//         /// <param name="id">Identificador del formulario.</param>
//         /// <returns>FormDTO correspondiente.</returns>
//         public async Task<FormDTO> GetFormByIdAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
//                 throw new ValidationException("id", "El ID del formulario debe ser mayor que cero");
//             }
//             try
//             {
//                 var form = await _formData.GetByIdAsync(id);
//                 if (form == null)
//                 {
//                     _logger.LogInformation("No se encontró ningún formulario con el ID {FormId}", id);
//                     throw new EntityNotFoundException("Form", id);
//                 }
//                 return MapToDTO(form);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al obtener el formulario con el ID: {FormId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
//             }
//         }

//         /// <summary>
//         /// Método para crear un formulario a partir de un DTO.
//         /// </summary>
//         /// <param name="formDTO">DTO que contiene los datos del formulario a crear.</param>
//         public async Task<FormDTO> CreateFormAsync(FormDTO formDTO)
//         {
//             try
//             {
//                 ValidateForm(formDTO);

//                 var form = MapToEntity(formDTO);
//                 var formCreated = await _formData.CreateAsync(form);
//                 // Mapear la entidad creada a DTO para retornarlo.
//                 return MapToDTO(formCreated);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al crear nuevo formulario: {FormName}", formDTO?.Name ?? "null");
//                 throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
//             }
//         }

//          // Actualiza un formulario existente a partir de un DTO
//         public async Task<bool> UpdateFormAsync(FormDTO formDTO)
//         {
//             try
//             {
//                 ValidateForm(formDTO);
//                 var existingForm = await _formData.GetByIdAsync(formDTO.Id);
//                 if (existingForm == null)
//                 {
//                     throw new EntityNotFoundException("Form", formDTO.Id);
//                 }
//                 existingForm.Name = formDTO.Name;
//                 existingForm.Description = formDTO.Description;
//                 return await _formData.UpdateAsync(existingForm);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al actualizar formulario con ID: {FormId}", formDTO.Id);
//                 throw new ExternalServiceException("Base de datos", "Error al actualizar el formulario", ex);
//             }
//         }

//         // Elimina un formulario por ID
//         public async Task<bool> DeleteFormAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 _logger.LogWarning("ID inválido al eliminar formulario: {FormId}", id);
//                 throw new ValidationException("id", "El ID del formulario debe ser mayor que cero");
//             }
//             try
//             {
//                 return await _formData.DeleteAsync(id);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar formulario con ID: {FormId}", id);
//                 throw new ExternalServiceException("Base de datos", $"Error al eliminar el formulario con ID {id}", ex);
//             }
//         }

//         /// <summary>
//         /// Método para validar el DTO del formulario.
//         /// </summary>
//         /// <param name="formDTO">Formulario a validar.</param>
//         private void ValidateForm(FormDTO formDTO)
//         {
//             if (formDTO == null)
//             {
//                 throw new ValidationException("El objeto formulario no puede ser nulo");
//             }

//             if (string.IsNullOrWhiteSpace(formDTO.Name))
//             {
//                 _logger.LogWarning("Se intentó crear/actualizar un formulario con Name vacío");
//                 throw new ValidationException("Name", "El nombre del formulario es obligatorio");
//             }
//         }


//         /// <summary>
//         /// Elimina un formulario de manera logica por ID
//         /// </summary>
//         public async Task<bool> DeleteFormLogicalAsync(int id)
//         {
//             if (id <= 0)
//             {
//                 throw new ValidationException("ID", "El ID del formulario debe ser mayor que cero.");
//             }

//             var existingForm = await _formData.GetByIdAsync(id);
//             if (existingForm == null)
//             {
//                 throw new EntityNotFoundException("Form", id);
//             }

//             try
//             {

//                 return await _formData.DeleteLogicAsyncLINQ(id);

//             }
//             catch (ExternalServiceException ex)
//             {
//                 _logger.LogError(ex, "Error en servicio externo al eliminar el formulario con ID: {FormId}", id);
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al eliminar el formulario de manera logica con ID: {FormId}", id);
//                 throw new ExternalServiceException("Base de datos", "Error al eliminar el formulario de manera logica.", ex);
//             }
//         }

//         /// <summary>
//         /// Método para mapear la entidad Form a FormDTO.
//         /// </summary>
//         /// <param name="form">Entidad Form</param>
//         /// <returns>FormDTO mapeado.</returns>
//         private FormDTO MapToDTO(Form form)
//         {
//             return new FormDTO
//             {
//                 Id = form.Id,
//                 Name = form.Name,
//                 Description = form.Description,
//                 Active = form.Active
                
//             };
//         }

//         /// <summary>
//         /// Método para mapear un FormDTO a la entidad Form.
//         /// </summary>
//         /// <param name="formDTO">FormDTO a mapear.</param>
//         /// <returns>Entidad Form.</returns>
//         private Form MapToEntity(FormDTO formDTO)
//         {
//             return new Form
//             {
//                 Id = formDTO.Id,
//                 Name = formDTO.Name,
//                 Description = formDTO.Description,
//                 Active = formDTO.Active
                
//             };
//         }

//         /// <summary>
//         /// Método para mapear una lista de entidades Form a una lista de FormDTO.
//         /// </summary>
//         /// <param name="forms">Lista de formularios.</param>
//         /// <returns>Lista de FormDTO.</returns>
//         private IEnumerable<FormDTO> MapToDTOList(IEnumerable<Form> forms)
//         {
//             // Uso de LINQ para mayor simplicidad
//             return forms.Select(MapToDTO);
//         }
//     }
// }
