using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Global
{
    public class UserRolData : GenericData<RolUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolUser> _logger;

        // Aquí se recibe ILogger<RolUser> y se lo asigna tanto a la clase base como al campo local
        public UserRolData(ApplicationDbContext context, ILogger<RolUser> logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger; // Asigna el logger recibido al campo privado
        }

        public override async Task<IEnumerable<RolUser>> GetAllAsync()
        {
            try
            {
                return await _context.RolUser
                    .Include(ru => ru.User)
                    .Include(ru => ru.Role)
                    .Where(ur => ur.Active)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar RolUser");
                throw;
            }
        }

        public override async Task<RolUser> GetByIdAsync(int id)
        {
            try
            {
                return await _context.RolUser
                    .Include(ur => ur.User)
                    .Include(ur => ur.Role)
                    .Where(ur => ur.Active)
                    .FirstOrDefaultAsync(ur => ur.Id == id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving UserRols");
                throw;
            }
        }
    }
}
