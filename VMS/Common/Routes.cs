namespace VMS.Common
{
    public static class Routes
    {
        #region Main

        public const string HomePage = "/";
        public const string ActivitySearch = "/hoat-dong";
        public const string ActivityInfo = "/chi-tiet-hoat-dong";
        public const string Map = "/ban-do";
        public const string OrganizerSearch = "/don-vi-to-chuc";

        #endregion Main

        #region Org + User profile

        public const string UserProfile = "/ho-so-ca-nhan";
        public const string OrgProfile = "/ho-so-to-chuc";
        public const string EditOrgProfile = "/chinh-sua-ho-so-to-chuc";
        public const string EditUserProfile = "/chinh-sua-ho-so-ca-nhan";
        public const string VolunteersList = "/quan-ly-tinh-nguyen-vien";
        public const string ActivityManagement = "/quan-ly-hoat-dong";
        public const string Rating = "/quan-ly-tinh-nguyen-vien/danh-gia";
        public const string ActivityLogPage = "/lich-su-hoat-dong";
        public const string EditRequestPage = "/quan-ly-hoat-dong/yeu-cau-chinh-sua";

        #endregion Org + User profile
        public const string ResetPassword = "/reset-password";
        #region Activity

        public const string CreateActivity = "/tao-hoat-dong";
        public const string EditActivity = "/sua-hoat-dong";

        #endregion Activity

        #region Identity

        public const string Register = "/dang-ky";
        public const string LogIn = "/dang-nhap";
        public const string LogOut = "Identity/Account/LogOut";
        public const string Manage = "Identity/Account/Manage";

        #endregion Identity

        #region Admin

        public const string AdminActivityManagement = "/admin/quan-ly-hoat-dong";
        public const string AdminWatchRating = "/admin/quan-ly-hoat-dong/xem-danh-gia";
        public const string AdminWatchlistVolunteer = "/admin/quan-ly-hoat-dong/xem-danh-sach";

        public const string AdminActivityInfo = "/admin/chi-tiet-hoat-dong";

        public const string AdminOrganizationManagement = "/admin/quan-ly-to-chuc";
        public const string AdminEditOrganizationProfile = "/admin/chinh-sua-ho-so-to-chuc";
        public const string AdminVolunteerManagement = "/admin/quan-ly-ca-nhan";
        public const string AdminEditVolunteerProfile = "/admin/chinh-sua-ho-so-ca-nhan";

        public const string AdminResolveReport = "/admin/quan-ly-bao-cao";
        public const string AdminDetailReport = "/admin/quan-ly-bao-cao/chi-tiet";

        public const string AdminOrgAccountManagement = "/admin/quan-ly-tai-khoan-to-chuc";
        public const string AdminVolunteerAccountManagement = "/admin/quan-ly-tai-khoan-ca-nhan";
        public const string AdminAdminAccountManagement = "/admin/quan-ly-tai-khoan-quan-tri-vien";
        public const string AdminFeatureSuggestionManagement = "/admin/bo-sung-tinh-nang";
        public const string AdminDashboardManagement = "/admin/bao-cao-thong-ke";

        #endregion Admin
    }
}