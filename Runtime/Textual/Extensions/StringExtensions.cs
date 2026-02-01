using System;

namespace EasyToolkit.Core.Textual
{


    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string val)
        {
            return string.IsNullOrEmpty(val);
        }

        public static bool IsNullOrWhiteSpace(this string val)
        {
            return string.IsNullOrWhiteSpace(val);
        }

        public static bool IsNotNullOrEmpty(this string val)
        {
            return !string.IsNullOrEmpty(val);
        }

        public static bool IsNotNullOrWhiteSpace(this string val)
        {
            return !string.IsNullOrWhiteSpace(val);
        }

        public static string ToUpperFirst(this string val)
        {
            if (val.IsNullOrEmpty())
                return val;
            return char.ToUpper(val[0]) + val[1..];
        }

        public static string ToLowerFirst(this string val)
        {
            if (val.IsNullOrEmpty())
                return val;
            return char.ToLower(val[0]) + val[1..];
        }

        public static string DefaultIfNullOrEmpty(this string val, string defaultValue)
        {
            return IsNullOrEmpty(val) ? defaultValue : val;
        }

        public static string DefaultIfNullOrWhiteSpace(this string val, string defaultValue)
        {
            return IsNullOrWhiteSpace(val) ? defaultValue : val;
        }

        public static bool Contains(this string str, string toCheck, StringComparison comparisonType)
        {
            return str.IndexOf(toCheck, comparisonType) >= 0;
        }

        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        public static float ToFloat(this string str)
        {
            return float.Parse(str);
        }

        public static string SafeToString<T>(this T obj)
        {
            return obj == null ? "null" : obj.ToString();
        }


    }
}
