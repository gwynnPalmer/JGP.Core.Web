// ***********************************************************************
// Assembly         : JGP.Core.Web
// Author           : Joshua Gwynn-Palmer
// Created          : 07-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-29-2022
// ***********************************************************************
// <copyright file="ServiceInfo.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Web.Api.KeyStorage
{
    using Finbuckle.MultiTenant;

    /// <summary>
    ///     Class Service.
    /// </summary>
    public class Service
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Service" /> class.
        /// </summary>
        public Service()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Service" /> class.
        /// </summary>
        /// <param name="tenantInfo">The tenant information.</param>
        public Service(ITenantInfo tenantInfo)
        {
            ApiKey = tenantInfo.ConnectionString;
            ServiceId = Guid.Parse(tenantInfo.Id);
            ServiceName = tenantInfo.Name;
        }

        /// <summary>
        ///     Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        public string ApiKey { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid ServiceId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string ServiceName { get; set; }

        /// <summary>
        ///     Converts to tenantinfo.
        /// </summary>
        /// <returns>TenantInfo.</returns>
        public TenantInfo ToTenantInfo()
        {
            return new TenantInfo
            {
                Id = ServiceId.ToString(),
                Identifier = $"Api-Service-{ServiceName}",
                Name = ServiceName,
                ConnectionString = ApiKey
            };
        }

        /// <summary>
        ///     Updates the specified tenant information.
        /// </summary>
        /// <param name="tenantInfo">The tenant information.</param>
        public void Update(ITenantInfo tenantInfo)
        {
            ApiKey = tenantInfo.ConnectionString;
            ServiceId = Guid.Parse(tenantInfo.Id);
            ServiceName = tenantInfo.Name;
        }
    }
}