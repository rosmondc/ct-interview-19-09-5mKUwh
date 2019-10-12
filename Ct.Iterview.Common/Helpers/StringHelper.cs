namespace Ct.Interview.Common.Helpers
{
    public static class StringHelper
    {
        public static string RemoveEscapeCharacter(this string s)
        {
            return s.Replace("\"", "");
        }
    }
}
