using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace VMS.Common.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static bool TryGetQueryString<T>(this NavigationManager navigationManager, string key, out T value)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T)(object)valueAsInt;
                    return true;
                }

                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)valueFromQueryString.ToString();
                    return true;
                }

                if (typeof(T) == typeof(bool) && bool.TryParse(valueFromQueryString, out var valueAsBool))
                {
                    value = (T)(object)valueAsBool;
                    return true;
                }

                if (typeof(T) == typeof(List<int>))
                {
                    List<int> valuesAsInt = new();
                    foreach (var item in valueFromQueryString)
                    {
                        if (int.TryParse(item, out var number))
                        {
                            valuesAsInt.Add(number);
                        }
                    }
                    value = (T)(object)valuesAsInt;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
