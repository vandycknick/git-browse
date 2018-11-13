namespace GitBrowse
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string str, string value)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(value))
                return str;

            while (str.EndsWith(value))
                str = str.Remove(str.LastIndexOf(value));

            return str;
        }
    }
}
