﻿using System.Collections.Generic;

namespace VMS.Common
{
    public static class ActType
    {
        public const string Upcoming = "Sắp diễn ra";
        public const string Happenning = "Đang diễn ra";
        public const string TookPlace = "Đã diễn ra";
        public const string Closed = "Đã đóng đăng ký";
        public const string Approved = "Đã được duyệt";
        public const string NotApproved = "Chưa được duyệt";


        public static List<string> GetList()
        {
            return new()
            {
                Upcoming,
                Happenning,
                TookPlace,
                Closed,
                Approved,
                NotApproved
            };
        }
    }
}
