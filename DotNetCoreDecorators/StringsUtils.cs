namespace DotNetCoreDecorators
{
    public static class StringsUtils
    {

        public static string AddLastSymbolIfNotExists(this string src, char last)
        {
            if (string.IsNullOrEmpty(src))
                return string.Empty + last;

            if (src[^1] == last)
                return src;

            return src + last;
        }

        public static string AddFirstSymbolIfNotExists(this string src, char first)
        {
            if (string.IsNullOrEmpty(src))
                return first + string.Empty;

            if (src[0] == first)
                return src;

            return first + src;
        }

    }
}