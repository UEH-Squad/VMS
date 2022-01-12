namespace VMS.Common.Enums
{
    public enum StatusAct{
        Current, 
        Favor,
        Ended,
        Calendar,

        Upcoming,
        Happenning,
        TookPlace,
        Closed,
        All,
        Approved,
        NotApproved,
    }

    public static class StatusActExtension
    {
        public static string GetName(this StatusAct status)
        {
            return status switch
            {
                StatusAct.Happenning => "Đang diễn ra",
                StatusAct.TookPlace => "Đã diễn ra",
                StatusAct.Closed => "Đã đóng đăng ký",
                _ => string.Empty,
            };
        }
    }
}