namespace VMS.Common
{
    public static class Routes
    {
        #region Main

        public const string HomePage = "/";
        public const string ActivitySearch = "/hoat-dong";
        public const string ActivityInfo = "/chi-tiet-hoat-dong";
        public const string Organizations = "/don-vi-to-chuc";
        public const string Map = "/ban-do";

        #region Org + User profile

        public const string User = "/trang-ca-nhan";
        public const string EditProfile = "/chinh-sua-thong-tin";
        public const string VolunteersListPage = "/danh-sach-tinh-nguyen-vien";

        #endregion Org + User profile

        #endregion Main

        #region Activity

        public const string CreateActivity = "/tao-hoat-dong";
        public const string EditActivity = "/sua-hoat-dong";

        #endregion Activity

        #region Topic

        public const string Topic = "/chu-de";
        public const string CreateTopic = "/chu-de/them";
        public const string EditTopic = "/chu-de/sua";
        public const string DeleteTopic = "/chu-de/xoa";

        #endregion Topic

        #region Identity

        public const string Register = "/dang-ky";
        public const string LogIn = "/dang-nhap";
        public const string LogOut = "Identity/Account/LogOut";
        public const string Manage = "Identity/Account/Manage";

        #endregion Identity
    }
}