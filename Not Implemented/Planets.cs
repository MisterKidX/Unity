using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Utility.Geometry;

namespace Utility.Astronomy
{
    public abstract class Planet
    {
        public abstract int meanRadius { get; set; }
        public int meanDiameter;
        public int equatorialRadius;
        public int polarRadius;
        public int equatorialCircumference;
        public int meridionalCircumference;
    }

    public static class Earth
    {
        // In Meters
        public const int meanDiameter = 12742000;
        public const int meanRadius = 6371000; // meanDiameter / 2
        public const int equatorialRadius = 6378100; // Radius at the equator
        public const int polarRadius = 6378100; // radius at the polar
        public const int equatorialCircumference = 40075017; // 2π * equatorial radius
        public const int meridionalCircumference = 40007860; // 2π * meridional radius

        public const float LengthPerDegree = 111120f; // (Meridonial Circumfrence / 2) / 360 latitude lines
        public const float kmLat = LengthPerDegree; // It is true!

        private static readonly Geometry.Sphere earth = new Geometry.Sphere(meanRadius, Point.Zero()); // used for mathematical calculations

        /// <summary>
        /// Gets the appropriate distance between two longitudes given a certain latitude.
        /// </summary>
        /// <param name="lat">in Degrees</param>
        /// <returns></returns>
        public static float GetLengthLon(float lat)
        {
            Debug.Print("Have not yet implemented oblate sphereoid earth");
            return LengthPerDegree * (float)Math.Cos(lat);
        }

        public static float GetLengthBetween(float lat1, float lon1, float lat2, float lon2)
        {
            return earth.GetLengthArc(earth.GetPoint(lat1, lon1), earth.GetPoint(lat2, lon2));
        }

        public static Geometry.Sphere GetPlanetAsSphere()
        {
            return earth;
        }
    }
}

