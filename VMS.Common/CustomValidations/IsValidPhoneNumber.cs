using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VMS.Common.CustomValidations
{
    public class IsValidPhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string phoneNumber = Convert.ToString(value);

            bool isValid = Regex.Match(phoneNumber, @"^([\+]?84[-]?|[0])?[1-9][0-9]{8}$").Success;

            if (isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Số điện thoại không hợp lệ!");
        }
    }
}
