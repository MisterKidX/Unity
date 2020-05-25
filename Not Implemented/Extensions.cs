using System;

namespace Utility
{
    static class Extensions
    {
        // Int, double Extensions
        /// <summary>
        /// returns a boolean whether a nuber is between two other numbers
        /// </summary>
        /// <param name="num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsBetweenNumbers(this int num, double min, double max)
        {
            if (num >= min && num <= max) { return true; }
            return false;
        }
        public static bool IsBetweenNumbers(this double num, double min, double max)
        {
            if (num >= min && num <= max) { return true; }
            return false;
        }
        public static double Power(this double num, int power)
        {
            return Math.Pow(num, power);
        }
        public static double Power(this float num, int power)
        {
            return Math.Pow(num, power);
        }

        /// <summary>
        /// Does a cosine calculation with degrees.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Degrees</returns>
        public static double Cos(this float num)
        {
            var a = new UnitOf.Angle();

            return Math.Cos(a.FromDegrees(num).ToRadians());
        }
    }
}