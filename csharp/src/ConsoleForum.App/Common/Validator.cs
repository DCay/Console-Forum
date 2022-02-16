using System.Text.RegularExpressions;

namespace ConsoleForum.App.Common
{
    public static class Validator
    {
        public static bool IsNull(object obj)
        {
            return obj == null;
        }

        public static bool IsNullOrEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsAlphanumeric(string text)
        {
            return new Regex("[a-zA-Z0-9]*").IsMatch(text);
        }
    }
}
