namespace VMS.Common
{
    public static class Routes
    {
        #region Main

        public const string HomePage = "/";
        public const string Activities = "/hoat-dong";
        public const string Organizations = "/don-vi-to-chuc";
        public const string Map = "/ban-do";

        public const string User = "/trang-ca-nhan";

        #endregion Main

        #region Identity

        public const string Register = "/dang-ky";
        public const string LogIn = "/dang-nhap";
        public const string LogOut = "Identity/Account/LogOut";
        public const string Manage = "Identity/Account/Manage";

        #endregion Identity
    }
}