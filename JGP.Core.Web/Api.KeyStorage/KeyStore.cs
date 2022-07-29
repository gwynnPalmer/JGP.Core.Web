// ***********************************************************************
// Assembly         : JGP.Core.Web
// Author           : Joshua Gwynn-Palmer
// Created          : 07-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-29-2022
// ***********************************************************************
// <copyright file="KeyStore.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Web.Api.KeyStorage
{
    using System.Data;
    using Finbuckle.MultiTenant;
    using Microsoft.Data.SqlClient;

    /// <summary>
    ///     Class DbKeyStore.
    ///     Implements the <see cref="Finbuckle.MultiTenant.IMultiTenantStore{Finbuckle.MultiTenant.TenantInfo}" />
    /// </summary>
    /// <seealso cref="Finbuckle.MultiTenant.IMultiTenantStore{Finbuckle.MultiTenant.TenantInfo}" />
    public class DbKeyStore : IMultiTenantStore<TenantInfo>
    {
        /// <summary>
        ///     The services
        /// </summary>
        private static List<TenantInfo> _services;

        /// <summary>
        ///     The connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbKeyStore" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DbKeyStore(string connectionString)
        {
            _connectionString = connectionString;
            _services = Populate();
        }

        /// <summary>
        ///     Get all as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
        public async Task<IEnumerable<TenantInfo>> GetAllAsync()
        {
            return _services;
        }

        /// <summary>
        ///     Try add as an asynchronous operation.
        /// </summary>
        /// <param name="tenantInfo">The tenant information.</param>
        /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
        public async Task<bool> TryAddAsync(TenantInfo tenantInfo)
        {
            const string query = "INSERT INTO dbo.KeyStore(ServiceId, ServiceName, ApiKey) VALUES(@id, @name, @apiKey)";
            await using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", tenantInfo.Id);
            command.Parameters.AddWithValue("@name", tenantInfo.Name);
            command.Parameters.AddWithValue("@apiKey", tenantInfo.ConnectionString);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteScalarAsync();
                lock (_services)
                {
                    _services.Add(tenantInfo);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        ///     Try get as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A Task&lt;TenantInfo&gt; representing the asynchronous operation.</returns>
        public async Task<TenantInfo?> TryGetAsync(string id)
        {
            return _services.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        ///     Try get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A Task&lt;TenantInfo&gt; representing the asynchronous operation.</returns>
        public async Task<TenantInfo?> TryGetByIdentifierAsync(string identifier)
        {
            return _services.FirstOrDefault(x => x.Identifier == identifier);
        }

        /// <summary>
        ///     Try remove as an asynchronous operation.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> TryRemoveAsync(string identifier)
        {
            if (_services.All(x => x.Identifier != identifier))return false;

            var service = _services.First(x => x.Identifier == identifier);
            const string query = "DELETE FROM dbo.KeyStore WHERE ServiceId = @id";
            await using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", service.Id);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteScalarAsync();
                lock (_services)
                {
                    _services.Remove(service);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        ///     Try update as an asynchronous operation.
        /// </summary>
        /// <param name="tenantInfo">The tenant information.</param>
        /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
        public async Task<bool> TryUpdateAsync(TenantInfo tenantInfo)
        {
            if (_services.All(x => x.Id != tenantInfo.Id)) return false;

            var service = _services.FirstOrDefault(x => x.Id == tenantInfo.Id);
            if (service == null) return false;

            const string query = "UPDATE dbo.KeyStore SET ServiceName = @name, ApiKey = @apiKey WHERE ServiceId = @id";
            await using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", tenantInfo.Id);
            command.Parameters.AddWithValue("@name", tenantInfo.Name);
            command.Parameters.AddWithValue("@apiKey", tenantInfo.ConnectionString);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteScalarAsync();
                lock (_services)
                {
                    _services.Remove(service);
                    _services.Add(tenantInfo);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        ///     Populates this instance.
        /// </summary>
        /// <returns>List&lt;TenantInfo&gt;.</returns>
        private List<TenantInfo> Populate()
        {
            var services = new List<TenantInfo>();
            const string query = "SELECT ServiceId, ServiceName, ApiKey FROM dbo.KeyStore";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var service = new Service
                    {
                        ApiKey = reader.GetString(2),
                        ServiceId = reader.GetGuid(0),
                        ServiceName = reader.GetString(1)
                    };
                    services.Add(service.ToTenantInfo());
                }

                reader.Close();
                return services;
            }
            catch (Exception)
            {
                return services;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}