using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Global
{
    public class UserData : GenericData<User>
    {
        private readonly ApplicationDbContext _context;
        public UserData(ApplicationDbContext context, ILogger<User> logger)
            : base(context, logger)
        {
            _context = context;
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.User
                    .Include(u => u.Person)
                    .Where(u => u.State)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Registra el error según tu manejo de excepciones/logging
                throw;
            }
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            try
            {
                return await _context.User
                    .Include(u => u.Person)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}


