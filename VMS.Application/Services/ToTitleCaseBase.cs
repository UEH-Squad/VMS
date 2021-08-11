using System.Globalization;

namespace ToTitleCaseBase
{
    public static class ToTitleCaseBase
    {
        public static string ToTitleCase(this string str)
        {
            TextInfo textInfo = new CultureInfo("vi-VN", false).TextInfo;
            return textInfo.ToTitleCase(str);
        }
    }
}