using VMS.Application.ViewModels;
using VMS.Common.Enums;

namespace VMS.Common.Extensions
{
    public static class AccountExtensions
    {
        public static bool IsValidAccount(this CreateAccountViewModel account, Role role)
        {
            return role switch
            {
                Role.Admin => !string.IsNullOrEmpty(account.UserName)
                           && !string.IsNullOrEmpty(account.Password),

                Role.Organization => !string.IsNullOrEmpty(account.Course)
                                  && !string.IsNullOrEmpty(account.FullName)
                                  && !string.IsNullOrEmpty(account.Email),

                Role.User => !string.IsNullOrEmpty(account.StudentId)
                          && !string.IsNullOrEmpty(account.Course)
                          && !string.IsNullOrEmpty(account.FullName)
                          && !string.IsNullOrEmpty(account.Email),

                _ => false,
            };
        }

        public static bool IsValidAccount(this AccountViewModel account, Role role)
        {
            return role switch
            {
                Role.Admin => !string.IsNullOrEmpty(account.UserName)
                           && !string.IsNullOrEmpty(account.Password),

                Role.Organization => !string.IsNullOrEmpty(account.Course)
                                  && !string.IsNullOrEmpty(account.FullName)
                                  && !string.IsNullOrEmpty(account.Email),

                Role.User => !string.IsNullOrEmpty(account.StudentId)
                          && !string.IsNullOrEmpty(account.Course)
                          && !string.IsNullOrEmpty(account.FullName)
                          && !string.IsNullOrEmpty(account.Email),

                _ => false,
            };
        }
    }
}
