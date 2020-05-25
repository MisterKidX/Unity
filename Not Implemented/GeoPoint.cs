using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public struct GeoPoint : IFormattable
{
    #region Fields

    private float _lat;
    private float _lon;
    private float _alt;

    #endregion

    #region Properties

    public float Lat
    {
        get { return _lat; }
        set { _lat = value; }
    }

    public float Lon
    {
        get { return _lon; }
        set { _lon = value; }
    }

    public float Alt
    {
        get { return _alt; }
        set { _alt = value; }
    }

    #endregion

    #region Constructors

    public GeoPoint(float lat, float lon, float alt, GeoCalculator.DistanceUnits altUnit = GeoCalculator.DistanceUnits.Kilofeet)
    {
        _lat = lat;
        _lon = lon;

        switch (altUnit)
        {
            case GeoCalculator.DistanceUnits.Feet:
                this._alt = (float)GeoCalculator.ConvertDistanceUnit(alt, GeoCalculator.DistanceUnits.Feet, GeoCalculator.DistanceUnits.Degrees);
                break;
            
            case GeoCalculator.DistanceUnits.Kilofeet:
                this._alt = (float)GeoCalculator.ConvertDistanceUnit(alt, GeoCalculator.DistanceUnits.Kilofeet, GeoCalculator.DistanceUnits.Degrees);
                break;
            case GeoCalculator.DistanceUnits.NauticalMiles:
                this._alt = (float)GeoCalculator.ConvertDistanceUnit(alt, GeoCalculator.DistanceUnits.NauticalMiles, GeoCalculator.DistanceUnits.Degrees);
                break;
            case GeoCalculator.DistanceUnits.Degrees:
                this._alt = alt;
                break;
            case GeoCalculator.DistanceUnits.Meters:
                this._alt = (float)GeoCalculator.ConvertDistanceUnit(alt, GeoCalculator.DistanceUnits.Meters, GeoCalculator.DistanceUnits.Degrees);
                break;
            default:
                throw new FormatException("Geopoint could not be constructed with the given Distance Units input");
        }
    }
    public GeoPoint(GeoPoint point, GeoCalculator.DistanceUnits altUnit = GeoCalculator.DistanceUnits.Kilofeet) : this(point.Lat, point.Lon, point.Alt, altUnit)
    {
    }

    #endregion

    #region Methods

    #endregion

    #region Static Methods

    public static GeoPoint Add(GeoPoint left, GeoPoint right)
    {
        return new GeoPoint(left.Lat + right.Lat, left.Lon + right.Lon, left.Alt + right.Alt);
    }

    public static GeoPoint Subtract(GeoPoint left, GeoPoint right)
    {
        return new GeoPoint(left.Lat - right.Lat, left.Lon - right.Lon, left.Alt - right.Alt);
    }

    public static double Distance(GeoPoint p1, GeoPoint p2)
    {
        return GeoCalculator.GetGeoDistance(p1, p2);
    }

    #endregion

    #region Methods Overrides

    public override string ToString()
    {
        return String.Format("Degree: {0}, Degree: {1}, KFT: {2}",Lat, Lon,
                            GeoCalculator.ConvertDistanceUnit(Alt, GeoCalculator.DistanceUnits.Degrees, GeoCalculator.DistanceUnits.Kilofeet));
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        switch (format)
        {
            case "0.0":
                return String.Format("({0}, {1}, {2})", Lat, Lon,
                            GeoCalculator.ConvertDistanceUnit(Alt, GeoCalculator.DistanceUnits.Degrees, GeoCalculator.DistanceUnits.Kilofeet).ToString("0.0"));
            default:
                return this.ToString();
                break;
        }
    }

    public override bool Equals(object obj)
    {
        var point = (GeoPoint)obj;
        return point != null &&
               Lat == point.Lat &&
               Lon == point.Lon &&
               Alt == point.Alt;
    }

    public override int GetHashCode()
    {
        var hashCode = 1248711632;
        hashCode = hashCode * -1521134295 + Lat.GetHashCode();
        hashCode = hashCode * -1521134295 + Lon.GetHashCode();
        hashCode = hashCode * -1521134295 + Alt.GetHashCode();
        return hashCode;
    }



    #endregion

    #region Operator Overrides

    public static GeoPoint operator +(GeoPoint left, GeoPoint right)
    {
        return Add(left, right);
    }

    public static GeoPoint operator -(GeoPoint left, GeoPoint right)
    {
        return Subtract(left, right);
    }

    public static bool operator ==(GeoPoint left, GeoPoint right)
    {
        return left.Lat == right.Lat && left.Lon == right.Lon && left.Alt == right.Alt;
    }

    public static bool operator !=(GeoPoint left, GeoPoint right)
    {
        return left.Lat != right.Lat && left.Lon != right.Lon && left.Alt != right.Alt;
    }

    #endregion
}
