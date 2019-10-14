namespace Ct.Interview.Common.Helpers
{
    public static class StringHelper
    {
        public static string RemoveEscapeCharacter(this string value)
        {
            return value.Replace("\"", "");
        }

        /// <summary>
        /// returns a 86400  which is equivalent to 24 hours
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DefaultCacheExpirationToInt(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;
            return 86400;            
        }

        /// <summary>
        /// returns a 2 seconds
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int MinimumCacheExpirationToInt(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;
            return 2;
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

        /// <summary>
        /// returns a 300 seconds/5 minutes by default
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double RetryDownloadCsvToDouble(this string value)
        {
            double result;
            if (double.TryParse(value, out result))
                return result;
            return 300;
        }
    }
}
