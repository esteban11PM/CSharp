using Entity.Model;
using Entity;
using Microsoft.Extensions.Logging;
using Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Global
{
    public class PersonData : GenericData<Person>
    {
        private readonly ApplicationDbContext _context;
        public PersonData(ApplicationDbContext context, ILogger<Person> logger) : base(context, logger)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                return await _context.Person
                    .Where(p => p.Status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Registra el error según tu manejo de excepciones/logging
                throw;
            }
        }
    }
}


