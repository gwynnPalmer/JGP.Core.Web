// ***********************************************************************
// Assembly         : JGP.Core.Web
// Author           : Joshua Gwynn-Palmer
// Created          : 07-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-29-2022
// ***********************************************************************
// <copyright file="KeyStoreContext.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Web.Api.KeyStorage
{
    using Finbuckle.MultiTenant;
    using Finbuckle.MultiTenant.Stores;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     Class KeyStoreContext.
    ///     Implements the <see cref="Finbuckle.MultiTenant.Stores.EFCoreStoreDbContext{Finbuckle.MultiTenant.TenantInfo}" />
    /// </summary>
    /// <seealso cref="Finbuckle.MultiTenant.Stores.EFCoreStoreDbContext{Finbuckle.MultiTenant.TenantInfo}" />
    public class KeyStoreContext : EFCoreStoreDbContext<TenantInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyStoreContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public KeyStoreContext(DbContextOptions<KeyStoreContext> options) : base(options)
        {
        }
    }
}