using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using DAL.Models;
using Dapper;
using Common;

namespace DAL.Services
{
    public class DeviceService : IDeviceRepository<Device>
    {
        private readonly string _connectionString;
        public SqlConnection Connection {
            get{return new SqlConnection(_connectionString);}
        }
        public DeviceService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Domobert");
        }

        //TODO: Corriger ce workaround bancal
        #region IDeviceRepository


        public IEnumerable<Device> GetAll()
        {
            return GetAllAsync().Result;
        }

        public Device GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }

        public int Add(Device device)
        {
            return AddAsync(device).Result;
        }

        public bool Update(int id, Device device)
        {
            return UpdateAsync(id, device).Result;
        }

        public bool Delete(int id)
        {
            return DeleteAsync(id).Result;
        }
        #endregion

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    string query = "SELECT * FROM Devices";
                    var car = await conn.QueryAsync<Device>(query);
                    return car;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur SQL : {ex.Message}");

                throw new Exception("Une erreur est survenue lors de la récupération des données.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<Device> GetByIdAsync(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    string query = "SELECT * FROM Devices WHERE Id = @Id";
                    return await conn.QuerySingleOrDefaultAsync<Device>(query, new { Id = id });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur SQL : {ex.Message}");

                throw new Exception("Une erreur est survenue lors de la récupération des données.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddAsync(Device device)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    string query = "INSERT INTO Devices ([name], [type], [location], topicMQTT) " +
                                    "VALUES (@Name, @Type, @Location, @TopicMQTT); ";
                    return await conn.ExecuteScalarAsync<int>(query, device);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur SQL : {ex.Message}");

                throw new Exception("Une erreur est survenue lors de l'enregistrement des données.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(int id, Device device)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    string query = "UPDATE Devices " +
                                    "SET [Name] = @Name, [type] = @Type, [location] = @Location, topicMQTT = @TopicMQTT " +
                                    "WHERE Id = @id";
                    int rowsAffected = await conn.ExecuteAsync(query, device);
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur SQL : {ex.Message}");

                throw new Exception("Une erreur est survenue lors de la modification des données.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    string query = "DELETE FROM Devices WHERE Id = @Id";
                    int rowsAffected = await conn.ExecuteAsync(query, new { Id = id });
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur SQL : {ex.Message}");

                throw new Exception("Une erreur est survenue lors de la supression des données.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
