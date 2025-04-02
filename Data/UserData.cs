using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Contexts;
using Dapper;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Data
{
    public class UserData
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dapperConnection;
        private readonly ILogger<UserData> _logger;

        public UserData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<UserData> logger)
        {
            _context = context;
            _dapperConnection = dapperConnection;
            _logger = logger;
        }

    // CONSULTA POR MEDIO DE LINQ
        // consulta general
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>().ToListAsync();
        }
        // consulta por ID
        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<User>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {UserId}", id);
                throw; // Relanza la excepcion a las capas superiores
            }
        }

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                _context.Set<User>().Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario : {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var user = await _context.Set<User>().FindAsync(id);
                if (user == null)
                    return false;
                _context.Set<User>().Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el usuario {ex.Message}");
                return false;
            }
        }

    //CONSULTA POR MEDIO DE SQL
        // consulta general
        public async Task<IEnumerable<User>> GetAllAsyncSQL()
        {
            string query = @"
                SELECT 
                    u.Id, 
                    u.Username, 
                    u.State, 
                    p.Id AS PersonId, p.Name AS PersonName
                FROM User u
                INNER JOIN Person p ON u.PersonId = p.Id";

            return await _dapperConnection.QueryAsync<User>(query);
        }

        // consulta por ID
        public async Task<User?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"
                    SELECT 
                        u.Id, 
                        u.Username, 
                        u.State, 
                        p.Id AS PersonId, p.Name AS PersonName
                    FROM User u
                    INNER JOIN Person p ON u.PersonId = p.Id
                    WHERE u.Id = @Id";

                return await _dapperConnection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {UserId}", id);
                throw;
            }
        }

        // inserta un nuevo campo de la entidad en la base de datos con SQL
        public async Task<int> CreateAsyncSql(User user)
        {
            try
            {
                string sql = @"INSERT INTO User (Username, Password, State)
                                VALUES(@UserName, @Password, @State);
                                SELECT CAST(SCOPE_IDENTITY() as int)";
                return await _dapperConnection.ExecuteScalarAsync<int>(sql, new{
                    user.Username,
                    user.Password,
                    user.State
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al crear un nuevo User con SQL");
                throw;
            }
        }

        // actualiza un campo existente de la entiad en la base de datos con SQL
        public async Task<bool> UpdateAsyncSql(User user)
        {
            try
            {
                string sql = @"
                            UPDATE User 
                            SET 
                                Username = @Username,
                                Password = @Password,
                                State = @State
                            WHERE Id = @Id";
                var affectedRows = await  _dapperConnection.ExecuteAsync(sql, new{
                    user.Username,
                    user.Password,
                    user.State
                });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar User con SQL");
                throw;
            }
        }

        //Elimina un campo de la entidad en la base de datos con SQL
        public async Task<bool> DeleteAsyncSql(int id)
        {
            try
            {
                string sql = "DELETE FROM User WHERE Id = @Id";

                var affectedRows = await _dapperConnection.ExecuteAsync(sql, new {Id = id});
                return affectedRows >0;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar un campo con SQL");
                throw;
            }
            
        }


    }
}
