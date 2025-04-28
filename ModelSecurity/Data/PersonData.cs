// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Entity.Context;
// using Entity.DTOs;
// using Dapper;
// using Entity.Model;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using System.Data;

// namespace Data
// {
//     public class PersonData
//     {
//         protected readonly ApplicationDbContext _context;
//         private readonly IDbConnection _dapperConnection;
//         protected readonly ILogger<PersonData> _looger;

//         public PersonData(ApplicationDbContext context, IDbConnection dapperConnection, ILogger<PersonData> logger)
//         {
//             _context = context;
//             _dapperConnection = dapperConnection;
//             _looger = logger;
//         }
        
//     //CONSULTAS POR MEDIO DE LINQ
//         //consulta completa
//         public async Task<IEnumerable<Person>> GetAllAsync()
//         {
//             return await _context.Set<Person>().ToListAsync();
//         }

//         //consulta por ID
//         public async Task<Person?> GetBydIdAsync(int id)

//         {
//             try
//             {
//                 return await _context.Set<Person>().FindAsync(id);
//             }
//             catch(Exception ex) {
//                 _looger.LogError(ex, $"Error al obtener la persona con id {id}");
//                 throw;
//             }
//         }

//         public async Task<Person> CreateAsync(Person person)
//         {
//             try
//             {
//                 await _context.Set<Person>().AddAsync(person);
//                 await _context.SaveChangesAsync();
//                 return person;
//             }
//             catch (Exception ex) 
//             {
//                 _looger.LogError($"Error al crear la persona{ex.Message}");
//                 throw;
//             }

//         }

//         public async Task<bool> UpdateAsync(Person person)
//         {
//             try
//             {
//                 _context.Set<Person>().Update(person);
//                 await _context.SaveChangesAsync();
//                 return  true;
//             }
//             catch (Exception ex) {
//                 _looger.LogError($"Error al actualizar la persona {ex.Message}");
//                 throw;
//             }
            
//         }

//         public async Task<bool> DeleteAsync(int id)
//         {
//             try
//             {
//                 var person = await _context.Set<Person>().FindAsync(id);
//                 if (person == null)
//                     return false;
//                 _context.Set<Person>().Remove(person);
//                 await _context.SaveChangesAsync();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _looger.LogError($"Error al eliminar la persona {ex.Message}");
//                 return false;
//             }
//         }

//     //CONSULTAS POR MEDIO DE SQL
//         //consulta completa
//         public async Task<IEnumerable<Person>> GetAllAsyncSQL()
//         {
//             string query = @"
//                 SELECT Id, Name, LastName, PhoneNumber, Email, Address
//                 FROM Person";

//             return await _dapperConnection.QueryAsync<Person>(query);
//         }

//         //consulta por ID
//         public async Task<Person?> GetByIdAsynSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     SELECT Id, Name, LastName, PhoneNumber, Email, Address
//                     FROM Person
//                     WHERE Id = @Id";

//                 return await _dapperConnection.QueryFirstOrDefaultAsync<Person>(query, new { Id = id });
//             }
//             catch (Exception ex)
//             {
//                 _looger.LogError(ex, $"Error al obtener la persona con ID {id}");
//                 throw;
//             }
//         }

//         // agraga un nuevo campo de la entidad en la base de datos en SQL
//         public async Task<int> CreateAsyncSql(Person person)
//         {
//             try
//             {
//                 string sql = @"INSERT INTO Person (Name, LastName, PhoneNumber, Email, Address)
//                                 VALUES(@Name, @LastName, @PhoneNumber, @Email, @Address);
//                                 SELECT CAST(SCOPE_IDENTITY() as int)";
//                 return await _dapperConnection.ExecuteScalarAsync<int>(sql, new{
//                     person.Name,
//                     person.LastName,
//                     person.Phone,
//                     person.Email,
//                     person.DocumentNumber,
//                     person.Address,
//                     person.DocumentType,
//                     person.BlodType,
//                     person.Status,
//                 });
//             }
//             catch (Exception ex)
//             {
//                 _looger.LogError(ex,"Error al crear una nueva Person con SQL");
//                 throw;
//             }
//         }

//         //actualiza un campo de la entidad existente en la base de datos con SQL
//         public async Task<bool> UpdateAsyncSql(Person person)
//         {
//             try
//             {
//                 string sql = @"
//                             UPDATE Person 
//                             SET 
//                                 Name = @Name,
//                                 LastName = @LastName
//                                 PhoneNumber = @PhoneNumber
//                                 Email = @Email
//                                 Address = @Address
//                             WHERE Id = @Id";
//                 var affectedRows = await  _dapperConnection.ExecuteAsync(sql, new{
//                     person.Name,
//                     person.LastName,
//                     person.Phone,
//                     person.Email,
//                     person.DocumentNumber,
//                     person.Address,
//                     person.DocumentType,
//                     person.BlodType,
//                     person.Status,
//                 });
//                 return affectedRows > 0;
//             }
//             catch (Exception ex)
//             {
//                 _looger.LogError(ex, "Error al actualizar person con SQL");
//                 throw;
//             }
//         }

//         //Elimina un campo de la entidad en la base de datos con SQL
//         public async Task<bool> DeleteAsyncSql(int id)
//         {
//             try
//             {
//                 string sql = "DELETE FROM Person WHERE Id = @Id";

//                 var affectedRows = await _dapperConnection.ExecuteAsync(sql, new {Id = id});
//                 return affectedRows >0;
//             }
//             catch(Exception ex)
//             {
//                 _looger.LogError(ex, "Error al eliminar un campo con SQL");
//                 throw;
//             }
//         }

//         /// <summary>
//         /// Elimina un Person de manera logica de la base  de datos SQL
//         /// </summary>
//         public async Task<bool> DeleteLogicAsyncSQL(int id)
//         {
//             try
//             {
//                 string query = @"
//                     UPDATE Person 
//                     SET Status = 0
//                     WHERE Id = @Id;
//                     SELECT CAST(@@ROWCOUNT AS int);";

//                 int rowsAffected = await _dapperConnection.QuerySingleAsync<int>(query, new { Id = id });

//                 return rowsAffected > 0;
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error al eliminar logicamente form: {ex.Message}");
//                 return false;
//             }
//         }
//     }
// }
