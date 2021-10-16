using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VMS.Common.CustomValidations
{
    public class IsValidPhoneNumber : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string phoneNumber = value as string;
            return Regex.Match(phoneNumber, @"^([\+]?84[-]?|[0])?[1-9][0-9]{8}$").Success;
        }
    }
}
