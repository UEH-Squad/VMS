using System.Collections.Generic;

namespace VMS.Common
{
    public static class ActType
    {
        public const string upComing = "Sắp diễn ra";
        public const string happenning = "Đang diễn ra";
        public const string tookPlace = "Đã diễn ra";
        public const string closed = "Đã đóng đăng ký";


        public static List<string> GetType()
        {
           return new()
            {
                upComing,
                happenning,
                tookPlace,
                closed
            };
        }
    }
}
