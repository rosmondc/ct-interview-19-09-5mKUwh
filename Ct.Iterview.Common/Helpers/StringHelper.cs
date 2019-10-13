namespace Ct.Interview.Common.Helpers
{
    public static class StringHelper
    {
        public static string RemoveEscapeCharacter(this string value)
        {
            return value.Replace("\"", "");
        }

        /// <summary>
        /// returns a 900000 milliseconds which is equivalent to 15 minutes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int TimerScheduleConvertToInt(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;
            return 900000;
        }

        /// <summary>
        /// returns a 10 seconds by default
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double CacheExpirationToDouble(this string value)
        {
            double result;
            if (double.TryParse(value, out result))
                return result;
            return 10;
        }
    }
}
