namespace VMS.Common
{
    public static class Routes
    {
        #region Main

        public const string HomePage = "/";
        public const string Activities = "/hoat-dong";
        public const string Organizations = "/don-vi-to-chuc";
        public const string Map = "/ban-do";

        #endregion Main

        #region Activity
        public const string CreateActivity = "/hoat-dong/them-hoat-dong";
        public const string EditActivity = "/hoat-dong/sua-hoat-dong";
        public const string DeleteActivity = "/hoat-dong/xoa-hoat-dong";
        public const string ViewActivity = "/hoat-dong";
        #endregion Activity

        #region Identity

        public const string Register = "/dang-ky";
        public const string LogIn = "/dang-nhap";
        public const string LogOut = "Identity/Account/LogOut";
        public const string Manage = "Identity/Account/Manage";

        #endregion Identity
    }
}