using System.Globalization;

namespace VMS.Common.Extensions
{
    public static class StringExtentions
    {
        public static string ToTitleCase(this string str)
        {
            TextInfo textInfo = new CultureInfo("vi-VN", false).TextInfo;
            return textInfo.ToTitleCase(str);
        }
    }
}