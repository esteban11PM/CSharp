using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.Factories;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class PersonBusiness : GenericBusiness<Person, PersonDTO>
    {
        private readonly IDataFactoryGlobal _factory;

        public PersonBusiness(IDataFactoryGlobal factory, ILogger<Person> logger, IMapper mapper) : base(factory.CreatePersonData(), logger, mapper)
        {
            _factory = factory;

        }

        protected override void Validate(PersonDTO person)
        {
            if (person == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(person.Name))
                throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }
        public async Task<IEnumerable<PersonDTO>> GetAvailableAsync()
        {
            var allPersons = await _data.GetAllAsync(); // Todas las personas
            var allUsers = await _factory.CreateUserData().GetAllAsync(); // Todos los usuarios

            var usedPersonIds = allUsers.Select(u => u.PersonId).ToHashSet();

            var availablePersons = allPersons
                .Where(p => !usedPersonIds.Contains(p.Id)) // Personas que no estén en User.PersonId
                .ToList();

            return _mapper.Map<IEnumerable<PersonDTO>>(availablePersons);
        }

    }
}


