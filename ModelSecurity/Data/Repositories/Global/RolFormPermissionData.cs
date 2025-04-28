using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Global
{
    public class RolFormPermissionData : GenericData<RolFormPermission>
    {
        private ApplicationDbContext context;
        private ILogger<RolFormPermissionData> _logger;

        public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermission> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<RolFormPermission>> GetAllAsync()
        {
            try
            {
                return await context.RolFormPermission
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .Where(rfp => rfp.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar RolFormPermissions");
                throw;
            }
        }

        public override async Task<RolFormPermission> GetByIdAsync(int id)
        {
            try
            {
                return await context.RolFormPermission
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .Where(rfp => rfp.Active)
                    .FirstOrDefaultAsync(rfp => rfp.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recuperar RolFormPermissions");
                throw;
            }
        }
    }
}


