using System;
using System.Collections;
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

    public class RequiredHasItems : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IList list = value as IList;

            if (list is not null && list.Count > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.MemberName} không được rỗng!");
        }
    }
}