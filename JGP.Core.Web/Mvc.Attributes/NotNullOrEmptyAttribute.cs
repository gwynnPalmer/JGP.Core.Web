// ***********************************************************************
// Assembly         : JGP.Core.Web.Mvc.Attributes
// Author           : Joshua Gwynn-Palmer
// Created          : 06-17-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 06-17-2022
// ***********************************************************************
// <copyright file="NotNullOrEmptyAttribute.cs" company="JGP.Mvc.Attributes">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Web.Mvc.Attributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Class NotNullOrEmptyAttribute.
    ///     Implements the <see cref="System.ComponentModel.DataAnnotations.RequiredAttribute" />
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.RequiredAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullOrEmptyAttribute : RequiredAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NotNullOrEmptyAttribute" /> class.
        /// </summary>
        public NotNullOrEmptyAttribute()
        {
        }

        /// <summary>
        ///     Checks that the value of the required data field is not empty.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns><see langword="true" /> if validation is successful; otherwise, <see langword="false" />.</returns>
        public override bool IsValid(object? value)
        {
            var list = value as IEnumerable;
            return base.IsValid(value)
                   && list != null
                   && list.GetEnumerator().MoveNext()
                   && list.Cast<object>().All(item => item != null);
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return IsValid(value)
                ? ValidationResult.Success
                : new ValidationResult($"{validationContext.DisplayName} must contain at least one element.");
        }
    }
}