using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpenseTrackerCrudWebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class BindAndRequiredAttribute : ValidationAttribute, IBindingSourceMetadata, IPropertyValidationFilter
    {
        private readonly RequiredAttribute _required = new RequiredAttribute();

        // Use a known binding source (e.g., Body, Query, etc.). You can adjust this based on your need.
        public BindingSource BindingSource => BindingSource.Body;

        public override bool IsValid(object? value)
        {
            return _required.IsValid(value); // Checks null/empty
        }

        public override string FormatErrorMessage(string name)
        {
            return _required.FormatErrorMessage(name);
        }

        // Always validate this property even if parent is null
        public bool ShouldValidateEntry(ValidationEntry entry, ValidationEntry parentEntry)
        {
            return true;
        }
    }
}
