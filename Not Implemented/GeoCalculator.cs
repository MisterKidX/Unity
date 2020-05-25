using System;
using Geography;
using UnityEngine;

namespace Utility
{
    public static class GeoCalculator
    {
        #region Enums

        public enum DistanceUnits { NauticalMiles, Kilofeet, Feet, Degrees, Meters }
        public enum GeometricShape { Circle }

        #endregion

        #region Fields

        public const double FeetInNM = 6076.12;
        public const double NMInDegree = 60;
        public const double FeetInMeters = 3.28084;
        public const double MetersInDegree = 111139f;
        // In meters
        public const int earthRadius = 6371000;
        // in meters
        public const int earthCircumference = 40075000;

        #endregion

        #region Geo Methods

        /// <summary>
        /// Get distance between two given decimal Latitudes, Longitudes and Altitudes
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lat2"></param>  
        /// <param name="lon1"></param>
        /// <param name="lon2"></param>
        /// <param name="alt1"></param>
        /// <param name="alt2"></param>
        /// <returns>Returns distance in NM</returns>
        public static float GetGeoDistance(double lat1, double lon1, double alt1, double lat2, double lon2, double alt2)
        {
            var distanceLat = (lat1 - lat2).Power(2);
            var distanceLon = (lon1 - lon2).Power(2);
            var distanceAlt = (alt1 - alt2).Power(2);

            //var newR = earthRadius * Math.Cos(lat1);
            //var newC = 2 * Math.PI * newR;
            //var normalizedDegree = ((Math.Abs(lon1 - lon2) / 360f));
            //var arcMeasure = newC * normalizedDegree;
            //var degDistanceLon = ConvertDistanceUnit(arcMeasure, DistanceUnits.Meters, DistanceUnits.Degrees);
            //var distanceLon = degDistanceLon.Power(2);

            return (float)(NMInDegree * (Math.Sqrt(distanceLat + distanceLon + distanceAlt)));
        }
        /// <summary>
        /// Get distance between two GeoPoints
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>Returns distance in NM</returns>
        public static float GetGeoDistance(GeoPoint p1, GeoPoint p2)
        {
            return GetGeoDistance(p1.Lat, p1.Lon, p1.Alt, p2.Lat, p2.Lon, p2.Alt);
        }
        /// <summary>
        /// Performs Lerping while keeping the same position
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="percent"></param>
        /// <returns>returns the new postion relative to the old one</returns>
        public static double StepLerp(double value1, double value2, double percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new FormatException("Please give a percentage for LERP that is higher tahn 0 and lower than 100");
            }

            return value1 > value2 ? (value2 + (value1 - value2) * percent) : (value1 + (value2 - value1) * percent);
        }
        /// <summary>
        /// Converts a number from one unit of distance to another
        /// </summary>
        /// <param name="num"></param>
        /// <param name="fromUnit"></param>
        /// <param name="toUnit"></param>
        /// <returns>return the number in his other distance unit shape. </returns>
        public static double ConvertDistanceUnit(double num, DistanceUnits fromUnit, DistanceUnits toUnit)
        {
            if (fromUnit == toUnit)
                return num;

            switch (fromUnit)
            {
                case (DistanceUnits.Degrees):
                    switch (toUnit)
                    {
                        case (DistanceUnits.Feet):
                            return num * NMInDegree * FeetInNM;
                        case (DistanceUnits.Kilofeet):
                            return ((num * NMInDegree * FeetInNM) / 1000d);
                        case (DistanceUnits.NauticalMiles):
                            return num * NMInDegree;
                        default:
                            throw new FormatException("Can't convert.");
                    }
                case (DistanceUnits.Feet):
                    switch (toUnit)
                    {
                        case (DistanceUnits.Degrees):
                            return ((num / FeetInNM) / NMInDegree);
                        case (DistanceUnits.Kilofeet):
                            return (num / 1000d);
                        case (DistanceUnits.NauticalMiles):
                            return (num / FeetInNM);
                        default:
                            throw new FormatException("Can't convert.");
                    }
                case (DistanceUnits.Kilofeet):
                    switch (toUnit)
                    {
                        case (DistanceUnits.Degrees):
                            return ((num * 1000d) / FeetInNM) / NMInDegree;
                        case (DistanceUnits.Feet):
                            return num * 1000d;
                        case (DistanceUnits.NauticalMiles):
                            return (num * 1000d) / FeetInNM;
                        default:
                            throw new FormatException("Can't convert.");
                    }
                case (DistanceUnits.NauticalMiles):
                    switch (toUnit)
                    {
                        case (DistanceUnits.Degrees):
                            return num / NMInDegree;
                        case (DistanceUnits.Feet):
                            return num * FeetInNM;
                        case (DistanceUnits.Kilofeet):
                            return (num * FeetInNM) / 1000;
                        default:
                            throw new FormatException("Can't convert.");
                    }
                case (DistanceUnits.Meters):
                    switch (toUnit)
                    {
                        case (DistanceUnits.Degrees):
                            return num / MetersInDegree;
                        case (DistanceUnits.Feet):
                            return num * FeetInMeters;
                        case (DistanceUnits.Kilofeet):
                            return (num * FeetInMeters) / 1000;
                        default:
                            throw new FormatException("Can't convert.");
                    }
                default:
                    throw new FormatException("Can't convert.");
            }

            throw new NotImplementedException("Distance unit not implemented mathematically into code");
        }

        #endregion

        #region Math Methods

        /// <summary>
        /// A method to determine wether two circles intersect
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>returb true if intersection occurs</returns>
        public static bool AreCirclesIntersecting(GeoCircle c1, GeoCircle c2)
        {
            double distance = GetGeoDistance(c1.Midpoint, c2.Midpoint);

            if (distance < Math.Abs(c1.R - c2.R)) // circle contains another circle. This will be dealt in phase 2.
                return false;
            else if (distance > c1.R + c2.R || distance == 0 && c1.R == c2.R) // circles are too far || circles are on top each other and have same radius
                return false;
            else
                return true;
        }
        /// <summary>
        /// A method for computing the two points created by circle intersection. returns null if no intersection occurs and returns the same Point if circles are tangent.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>If the list has only one element it means the circles are tangent. if the list is null, it means no points were found, therefore no intersection. returns the points in degrees.</returns>
        public static GeoPoint[] CirclesIntersection(GeoCircle c1, GeoCircle c2)
        {
            // See reference image: https://i.stack.imgur.com/aUXMY.gif
            // See reference URL: https://stackoverflow.com/questions/3349125/circle-circle-intersection-points
            http://paulbourke.net/geometry/circlesphere/

            // First we check to see if Circles really intersect
            if (!AreCirclesIntersecting(c1, c2))
                return null;
            // We rearrange, to make c1 the circle with the bigger radius
            if (c2.R > c1.R)
            {
                GeoCircle c3 = new GeoCircle(c2);
                c2 = c1;
                c1 = c3;
            }

            GeoPoint[] points = new GeoPoint[2]; // symbolize the points of intersection

            float d = GetGeoDistance(c1.Midpoint, c2.Midpoint); // Distance from centers
            float a = (float)((c1.R.Power(2) - (c2.R.Power(2)) + d.Power(2)) / (2 * d)); // distance from center of circle1 to intersection mid-point
            float b; // distance from center of circle2 to intersection mid-point
            float h = (float)Math.Sqrt(c1.R.Power(2) - (a.Power(2))); // (half) height of the lens created from intersection

            d = (float)ConvertDistanceUnit(d, DistanceUnits.NauticalMiles, DistanceUnits.Degrees);
            a = (float)ConvertDistanceUnit(a, DistanceUnits.NauticalMiles, DistanceUnits.Degrees);
            h = (float)ConvertDistanceUnit(h, DistanceUnits.NauticalMiles, DistanceUnits.Degrees);

            GeoPoint p = new GeoPoint() // Midpoint of lens (created by intersection)
            {
                Lat = c1.Midpoint.Lat + a * (c2.Midpoint.Lat - c1.Midpoint.Lat) / d,
                Lon = c1.Midpoint.Lon + a * (c2.Midpoint.Lon - c1.Midpoint.Lon) / d,
                Alt = c1.Midpoint.Alt,
            };

            //DrawUI.instance.DrawIcon(DrawUI.instance.defaultIcon, p, 32);

            // Since Altitude is given with feet, we must first convert it to NM.
            points[0] = new GeoPoint()
            {
                Lat = p.Lat + h * (c2.Midpoint.Lon - c1.Midpoint.Lon) / d,
                Lon = p.Lon - h * (c2.Midpoint.Lat - c1.Midpoint.Lat) / d,
                Alt = p.Alt
            };
            points[1] = new GeoPoint()
            {
                Lat = p.Lat - h * (c2.Midpoint.Lon - c1.Midpoint.Lon) / d,
                Lon = p.Lon + h * (c2.Midpoint.Lat - c1.Midpoint.Lat) / d,
                Alt = p.Alt
            };

            return points;
        }
        public static double Lerp(double min, double max, double pointBetween)
        {
            var newMax = max - min;
            var newPoint = pointBetween - min;

            if (pointBetween < 0)
            {
                throw new NotImplementedException("Lerping wasn't possible because of bad input");
            }

            return newPoint / newMax;
        }
        public static double AngleBetweenLines(GeoLine g1, GeoLine g2)
        {
            return Math.Abs(Math.Atan2(g2.M - g1.M, 1 + g1.M * g2.M) * 180 / Math.PI);
        }

        public static float CalculatePixelPerNMX()
        {
            //calculate
            float screenWidthInNM = GetGeoDistance(StaticMap.Instance.BLCornerCoord, StaticMap.Instance.BRCornerCoord);
            float screenRelativeResulotionWidth = DrawUI.instance.relatedCanvas.referenceResolution.x;
            var pixelsPerNMX = screenRelativeResulotionWidth / screenWidthInNM;
            return pixelsPerNMX;
        }

        public static float CalculatePixelPerNMY()
        {
            //calculate
            float screenHeightInNM = GetGeoDistance(StaticMap.Instance.BLCornerCoord, StaticMap.Instance.TLCornerCoord);
            float screenRelativeResulotionHeight = DrawUI.instance.relatedCanvas.referenceResolution.y;
            var pixelsPerNMY = screenRelativeResulotionHeight / screenHeightInNM;
            return pixelsPerNMY;
        }

        #endregion

    }


}
