using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 

namespace Data.Repositories.Global
{
    public class ModuleFormData : GenericData<FormModule>
    {
        private ApplicationDbContext context;
        private ILogger<ModuleFormData> _logger;
        public ModuleFormData(ApplicationDbContext context, ILogger<FormModule> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            try
            {
                return await context.FormModule
                    .Include(mf => mf.Module)
                    .Include(mf => mf.Form)
                    .Where(mf => mf.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ModuleForms");
                throw;
            }
        }

        public override async Task<FormModule> GetByIdAsync(int id)
        {
            try
            {
                return await context.FormModule
                    .Include(mf => mf.Module)
                    .Include(mf => mf.Form)
                    .Where(mf => mf.Active)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ModuleForms");
                throw;
            }
        }
    }
}


