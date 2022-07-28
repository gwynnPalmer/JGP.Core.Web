// ***********************************************************************
// Assembly         : JGP.Core.Web.Mvc.Attributes
// Author           : Joshua Gwynn-Palmer
// Created          : 06-17-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 06-17-2022
// ***********************************************************************
// <copyright file="RequiredIfTrueAttribute.cs" company="JGP.Mvc.Attributes">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Web.Mvc.Attributes
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Class RequiredIfTrueAttribute.
    ///     Implements the <see cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfTrueAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RequiredIfTrueAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public RequiredIfTrueAttribute(string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = "The {0} field is required."; //used if error message is not set on attribute itself
        }

        /// <summary>
        ///     Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        private string PropertyName { get; }

        /// <summary>
        ///     Returns true if ... is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="context">The context.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();

            bool.TryParse(type.GetProperty(PropertyName)?.GetValue(instance)?.ToString(), out var propertyValue);

            if (propertyValue && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}