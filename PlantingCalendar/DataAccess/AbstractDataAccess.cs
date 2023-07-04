using Microsoft.Extensions.Options;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Dynamic;

namespace PlantingCalendar.DataAccess
{
    public abstract class AbstractDataAccess
    {
        private readonly DataAccessSettings _dataAccessSettings;

        public AbstractDataAccess(IOptions<DataAccessSettings> dataAccessSettings)
        {
            _dataAccessSettings = dataAccessSettings.Value;
        }

        protected async Task<List<T>> ExecuteSql<T>(string sql)
        {
            var sqlConnection = new SqlConnection(_dataAccessSettings.Plantbase);

            var command = new SqlCommand(sql, sqlConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, sqlConnection);

            DataTable dataTable = new DataTable();

            try
            {
                await sqlConnection.OpenAsync();
                dataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            if (dataTable != null) {

                var itemList = ConvertDataTable<T>(dataTable);

                return itemList;
            }

            return null;
        }

        protected async Task ExecuteSql(string sql)
        {
            var sqlConnection = new SqlConnection(_dataAccessSettings.Plantbase);

            var command = new SqlCommand(sql, sqlConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, sqlConnection);

            DataTable dataTable = new DataTable();

            try
            {
                await sqlConnection.OpenAsync();
                dataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }

        private List<T> ConvertDataTable<T>(DataTable dataTable)
        {
            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();

            return dataTable.AsEnumerable().Select(row =>
            {
                var newObject = Activator.CreateInstance<T>();

                foreach (var property in properties)
                {
                    if (columnNames.Contains(property.Name.ToLower())
                        && property.CanWrite)
                    {
                        try
                        {
                            var item = row[property.Name];

                            property.SetValue(newObject, (item == DBNull.Value) ? null : item);
                        }
                        catch (Exception ex) 
                        {
                            throw;
                        }
                    }
                }

                return newObject;

            }).ToList();
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
    }
}