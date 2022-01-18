namespace VMS.Common.Enums
{
    public enum ReportState
    {
        Default = 0,
        Pinned = 1,
        Processing = 2,
        Done = 3,
        Closed = 4,
        Deleted = 5
    }

    public static class ReportStateExtension
    {
        public static string GetName(this ReportState state)
        {
            return state switch
            {
                ReportState.Pinned => "Đã ghim",
                ReportState.Processing => "Đang xử lý",
                ReportState.Done => "Hoàn tất xử lý",
                ReportState.Closed => "Đã đóng",
                _ => string.Empty,
            };
        }
    }
}
