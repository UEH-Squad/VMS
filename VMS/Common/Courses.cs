using System;
using System.Collections.Generic;

namespace VMS.Common
{
    public static class Courses
    {
        public static List<string> GetCourses()
        {
            /* Tính khóa hiện tại bằng cách: [Năm hiện tại] - [Năm thành lập] + [Tháng hiện tại >= 9 ? 2 : 1] */
            int presentCourse = DateTime.Now.Year - 1976 + (DateTime.Now.Month >= 9 ? 2 : 1);

            List<string> courses = new();

            for (int i = 0; i < 8; i++)
            {
                courses.Add($"K{presentCourse - i}");
            }

            return courses;
        }

        public static List<string> GetLevels()
        {
            return new()
            {
                "Ban Chuyên môn",
                "Khoa/Viện/KTX",
                "CLB/Đội/Nhóm"
            };
        }
    }
}
