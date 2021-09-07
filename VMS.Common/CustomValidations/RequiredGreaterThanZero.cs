using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Common.CustomValidations
{
    public class RequiredGreaterThanZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int addressPathId = Convert.ToInt32(value);

            if (addressPathId > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Đơn vị hành chính không hợp lệ!");
        }
    }
}