using DAL.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DAL.Services
{
    public class HistoryTempService
    {
        private readonly string _connectionString;
        private static DateTime _lastExecutionTime = DateTime.MinValue;
        public SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public HistoryTempService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Domobert");
        }

        //TODO: Corriger ce workaround bancal
        #region Service

        public IEnumerable<TempHumiData> GetAll(int deviceId)
        {
            return GetAllAsync(deviceId).Result;
        }

        public int Add(TempHumiData data)
        {
            return AddAsync(data).Result;
        }

        public bool Delete(int id)
        {
            return DeleteAsync(id).Result;
        }
        #endregion


        public async Task<IEnumerable<TempHumiData>> GetAllAsync(int deviceId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    string query = "SELECT * FROM History_TempHumi WHERE DeviceID=@deviceId";
                    var car = await conn.QueryAsync<TempHumiData>(query, new { deviceId });
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

        public async Task<int> AddAsync(TempHumiData data)
        {
            if ((DateTime.Now - _lastExecutionTime).TotalMinutes < 15)
            {
                return 0; 
            }

            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    string query = "INSERT INTO History_TempHumi([Temperature], [Humidity], [Timestamp], [DeviceID])" +
                                    "VALUES(@Temperature, @Humidity, @Timestamp," +
                                    "(SELECT id FROM Devices WHERE TopicMQTT = @Topic));";

                    _lastExecutionTime = DateTime.Now;

                    return await conn.ExecuteScalarAsync<int>(query, data);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur SQL : {ex.Message}");

                throw new Exception("Une erreur est survenue lors de l'enregistrement des données de température.");
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
                    string query = "DELETE FROM History_TempHumi WHERE Id = @Id";
                    int rowsAffected = await conn.ExecuteAsync(query, new { Id = id });
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erreur SQL : {ex.Message}");

                throw new Exception("Une erreur est survenue lors de la suppression des données.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
