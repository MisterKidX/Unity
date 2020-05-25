using System;
using System.Collections;
using System.Collections.Generic;
using UnitOf;
using System.Linq;
using UnityEngine;
using Utility.Astronomy;

namespace Utility.Geometry
{
    public static class Calculator
    {
        public static float Distance (float x, float y, float z, float x2, float y2, float z2)
        {
            return (float)Math.Sqrt((x-x2).Power(2) + (y-y2).Power(2) + (z-z2).Power(2));
        }

        public static float Distance(Point p1, Point p2)
        {
            return Distance(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z);
        }
    }

    /// <summary>
    /// An abstraction to any 3d and 2d shape.
    /// </summary>
    public abstract class Shape
    {
        #region Properties

        public abstract float Area { get;}
        public abstract float Volume { get;}
        public int precisionPoint = 5;

        #endregion
    }

    /// <summary>
    /// A point
    /// </summary>
    public struct Point : IFormattable
    {
        #region Properties

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        /// <summary>
        /// The polar angle to (0,0,0) in radians as in cartesian coordinate system
        /// </summary>
        public float AzimuthAngle
        {
            get
            {
                Angle a = new Angle();

                if (X == 0 && Z == 0)
                    return (float)a.FromDegrees(0).ToRadians();
                else if (Z == 0 && X < 0)
                    return (float)a.FromDegrees(180).ToRadians();
                else if (Z == 0 && X > 0)
                    return (float)a.FromDegrees(0).ToRadians();
                else if (X == 0 && Z > 0)
                    return (float)a.FromDegrees(90).ToRadians();
                else if (X == 0 && Z < 0)
                    return (float)a.FromDegrees(-90).ToRadians();

                return (float)Math.Atan(Z / X);
            }
        }

        /// <summary>
        /// The Azimuth angle to (0,0,0) in radians as in cartesian coordinate system
        /// </summary>
        public float PolarAngle
        {
            get
            {
                Angle a = new Angle();

                if (Z == 0 && X == 0 && Y > 0)
                    return (float)a.FromDegrees(90).ToRadians();
                else if (Z == 0 && X == 0 && Y < 0)
                    return (float)a.FromDegrees(-90).ToRadians();
                else if (Z == 0 && X == 0 && Y == 0)
                    return 0;

                return (float)Math.Atan(Y * Math.Sqrt(X.Power(2) + Z.Power(2)) / (X.Power(2) + Z.Power(2)));
            }
        }

        #endregion

        #region ctors

        public Point (float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
        }

        #endregion

        #region Methods

        public static Point Add(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static Point Subtract(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static double Distance(Point p1, Point p2)
        {
            var p3 = p1 - p2;
            return Math.Sqrt(p3.X.Power(2) + p3.Y.Power(2) + p3.Z.Power(2));
        }

        public static Point Midpoint(Point p1, Point p2)
        {
            var p3 = p1 + p2;
            return new Point(p3.X/2, p3.Y/2, p3.Z/2);
        }

        public static Point Zero()
        {
            return new Point(0, 0, 0);
        }

        #endregion

        #region Overrides

        public static Point operator +(Point p1, Point p2)
        {
            return Add(p1, p2);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return Subtract(p1, p2);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return p1.X != p2.X && p1.Y != p2.Y && p1.Z != p2.Z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            var point = (Point)obj;
            return X == point.X &&
                   Y == point.Y &&
                   Z == point.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "G":
                    return ToString();
                case "C":
                    return string.Format($"({Distance(this, Zero())}, {this.PolarAngle}, {this.AzimuthAngle}");
                default:
                    throw new FormatException("Could not parse the required format.");
            }
        }
        #endregion
    }

    /// <summary>
    /// A 3D plane
    /// Equaton: ax +by +cz + d = 0
    /// https://brilliant.org/wiki/3d-coordinate-geometry-equation-of-a-plane/
    /// </summary>
    public struct Plane { }
    /// <summary>
    /// See for reference https://en.wikipedia.org/wiki/Ellipsoid
    /// Uses left coordinate system and spherical coordinate system. See https://en.wikipedia.org/wiki/Spherical_coordinate_system
    /// Equation: X^2/a^2 + Y^2/b^2 + Z^2/c^2 = 1
    /// </summary>
    public class Ellipsoid : Shape
    {
        #region Properties

        /// Principal Axes
        public float X { get; set; } // width
        public float Y { get; set; } // height
        public float Z { get; set; } // depth
        
        public Point Center { get; set; }

        public override float Area
        {
            get
            {
                throw new NotImplementedException("Not yet implemented the Area of a sphere.");
                var order = new List<float>() { X, Y, Z };
                order.Sort();

                var c = order[2];
                var b = order[1];
                var a = order[0];

                var cosTheta = c / a;
                var sinTheta = b / a;
                var K = (a.Power(2) * (b.Power(2) - c.Power(2))) / (b.Power(2) * (a.Power(2) * -c.Power(2)));

                var returnV = (float) (2 * Math.PI * a.Power(2) + ((2 * Math.PI * a * b) / sinTheta));

                return returnV;
            }
        }

        public override float Volume
        {
            get
            {
                return (float)((4 / 3) * Math.PI * X * Y * Z);
            }
        }

        #endregion

        #region ctors

        public Ellipsoid (float x, float y, float z, Point midPoint)
        {
            X = x;
            Y = y;
            Z = z;
            Center = midPoint;
        }

        #endregion

        #region Methods

        /// <summary>
        /// A sphereoid is Obtuse if its width is is larger than its height.
        /// </summary>
        /// <returns></returns>
        public bool IsOblateSphereoid()
        {
            if (!IsSphereoid()) return false;
            return X > Z;
        }

        /// <summary>
        /// A sphereoid is Prolate if its height is is larger than its width.
        /// </summary>
        /// <returns></returns>
        public bool IsProlateSphereoid()
        {
            if (!IsSphereoid()) return false;
            return Z > X;
        }

        /// <summary>
        /// an Ellipsoid is a sphereoid 
        /// </summary>
        /// <returns></returns>
        public bool IsSphereoid()
        {
            return Z == X || Z == Y || X == Y;
        }

        /// <summary>
        /// An Ellipsoid is a perfect sphere if its height is equal to its width.
        /// </summary>
        /// <returns></returns>
        public bool IsSpherical()
        {
            return Z == X && Z == Y;
        }

        /// <summary>
        /// A scalene ellipsoid is an eelipsoid where all principal axes are distinct
        /// </summary>
        /// <returns></returns>
        public bool IsScaleneEllipsoid()
        {
            return Z != X && Z != Y && X != Y;
        }

        /// <summary>
        /// Is the point within the sphere?
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsPointWithin(Point p)
        {
            return (Math.Round(p.X.Power(2), precisionPoint) / Math.Round(X.Power(2), precisionPoint)
                + Math.Round(p.Y.Power(2), precisionPoint) / Math.Round(Y.Power(2), precisionPoint)
                + Math.Round(p.Z.Power(2), precisionPoint) / Math.Round(Z.Power(2), precisionPoint)) < 1;
        }

        /// <summary>
        /// Is the point on the sphere?
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsPointOn(Point p)
        {
            return (Math.Round(p.X.Power(2), precisionPoint) / Math.Round(X.Power(2), precisionPoint)
                + Math.Round(p.Y.Power(2), precisionPoint) / Math.Round(Y.Power(2), precisionPoint)
                + Math.Round(p.Z.Power(2), precisionPoint) / Math.Round(Z.Power(2), precisionPoint)) >= 0.99 &&
                (Math.Round(p.X.Power(2), precisionPoint) / Math.Round(X.Power(2), precisionPoint)
                + Math.Round(p.Y.Power(2), precisionPoint) / Math.Round(Y.Power(2), precisionPoint)
                + Math.Round(p.Z.Power(2), precisionPoint) / Math.Round(Z.Power(2), precisionPoint)) <= 1.01;
        }

        /// <summary>
        /// Is the point outside the sphere?
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsPointOutside(Point p)
        {
            return (Math.Round(p.X.Power(2), precisionPoint) / Math.Round(X.Power(2), precisionPoint)
                + Math.Round(p.Y.Power(2), precisionPoint) / Math.Round(Y.Power(2), precisionPoint)
                + Math.Round(p.Z.Power(2), precisionPoint) / Math.Round(Z.Power(2), precisionPoint)) > 1;
        }

        /// <summary>
        /// Returns a point that sits on the ellipsoid
        /// </summary>
        /// <param name="polarAngle">"Up and down" angle</param>
        /// <param name="azimuthAngle">"Left to right" angle</param>
        /// <returns>Returns a new point on the ellipsoid</returns>
        public Point GetPoint(float polarAngle, float azimuthAngle)
        {
            Angle a = new Angle();

            var polar = a.FromDegrees(polarAngle).ToRadians();
            var azimut = a.FromDegrees(azimuthAngle).ToRadians();

            float x = (float)Math.Round(X * Math.Cos(polar) * Math.Cos(azimut), precisionPoint);
            float z = (float)Math.Round(Z * Math.Cos(polar) * Math.Sin(azimut), precisionPoint);
            float y = (float)Math.Round(Y * Math.Sin(polar), precisionPoint);

            return new Point(x, y, z);
        }


        #endregion
    }

    public class Sphereoid : Ellipsoid
    {
        #region Field & Autoprops

        public new float X { get; set; }
        public new float Y { get; set; }
        public new float Z { get { return X; } }


        #endregion

        #region ctors

        public Sphereoid(float xz, float y, Point midPoint) : base(xz, y, xz, midPoint) { }

        #endregion

    }

    public class Sphere : Sphereoid
    {
        #region Fields $ Autoprops

        public new float X { get; set; }
        public new float Y { get { return X; } }
        public new float Z { get { return X; } }
        public float Radius { get { return X; } set { X = value; } }

        #endregion

        #region ctors

        public Sphere(float radius, Point midPoint) : base(radius, radius, midPoint) { }

        #endregion


        #region Methods

        /// http://www.movable-type.co.uk/scripts/gis-faq-5.1.html
        /// https://en.wikipedia.org/wiki/Great-circle_distance
        /// <summary>
        /// Gets the distance on given arc between two points on the Ellipsoid.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public float GetLengthArc(Point p1, Point p2)
        {
            var ang = new Angle();

            if (!IsPointOn(p1) || !IsPointOn(p2))
                return -1;

            //var dlon = ang.FromDegrees(p1.AzimuthAngle - p2.AzimuthAngle).ToRadians();
            //var dlat = ang.FromDegrees(p1.PolarAngle - p2.PolarAngle).ToRadians();
            //var a = Math.Sin(dlat / 2).Power(2) + Math.Cos(p1.PolarAngle) * Math.Cos(p2.PolarAngle) * Math.Sin(dlon / 2).Power(2);
            //var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            //var d = X * c;

            var dPolar = Math.Abs(p1.PolarAngle - p2.PolarAngle);
            var dAzimut = Math.Abs(p1.AzimuthAngle - p2.AzimuthAngle);

            var delta = Math.Acos(Math.Sin(p1.PolarAngle) * Math.Sin(p2.PolarAngle) + Math.Cos(p1.PolarAngle) * Math.Cos(p2.PolarAngle) * Math.Cos(dAzimut));
            var distance = X * delta;

            return (float)distance;
        }

        #endregion
    }
}

namespace Utility.Geography
{
    using Geometry;

    public struct GeoPoint
    {
        #region Fields & Autoprops

        public float Lat { get; set; }
        public float Lon { get; set; }
        public float Alt { get; set; }

        #endregion

        #region Properties

        private Point _point
        {
            get
            {
                return Earth.GetPlanetAsSphere().GetPoint(Lat, Lon);
            }
        }

        #endregion

        #region ctors

        public GeoPoint(float lat, float lon, float alt)
        {
            Lat = lat;
            Lon = lon;
            Alt = alt;
            
        }
        public GeoPoint(GeoPoint point) : this(point.Lat, point.Lon, point.Alt)
        {

        }

        #endregion
    }
}