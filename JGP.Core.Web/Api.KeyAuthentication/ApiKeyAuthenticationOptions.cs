// ***********************************************************************
// Assembly         : JGP.Core.Web
// Author           : Joshua Gwynn-Palmer
// Created          : 07-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-29-2022
// ***********************************************************************
// <copyright file="ApiKeyAuthenticationOptions.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Authentication;

namespace JGP.Core.Web.Api.KeyAuthentication;

/// <summary>
///     Class ApiKeyAuthenticationOptions.
///     Implements the <see cref="AuthenticationSchemeOptions" />
/// </summary>
/// <seealso cref="AuthenticationSchemeOptions" />
public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    ///     The default scheme
    /// </summary>
    public const string DefaultScheme = "ApiKey";
}