using System;
using UnityEngine;
public static class Direction
{
    // Directions as degrees
    public const int n = 90, ne = 45, e = 0, se = 315, s = 270, sw = 225, w = 180, nw = 135;
    private const int range = 45;
    private const float Deg2Rad = 0.01745329f;
    private const float Rad2Deg = 57.29578f;
    
    public static DirectionEnum FromV2(Vector2 direction)
    {
        var angle = Vector2.SignedAngle(Vector2.right, direction.normalized);
        angle *= Deg2Rad;

        return GetDirection(angle);
    }

    public static DirectionEnum GetDirection(float angleRad)
    {
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.North))
            return DirectionEnum.North;
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.NorthEast))
            return DirectionEnum.NorthEast;
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.East))
            return DirectionEnum.East;
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.SouthEast))
            return DirectionEnum.SouthEast;
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.South))
            return DirectionEnum.South;
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.SouthWest))
            return DirectionEnum.SouthWest;
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.West))
            return DirectionEnum.West;
        if (AngleWithinDirectionRange(angleRad, DirectionEnum.NorthWest))
            return DirectionEnum.NorthWest;

        throw new FormatException("There's no Direction corresponding to the angel given motherfucker. Angel was: " + angleRad);
    }

    public static bool AngleWithinDirectionRange(float angleRad, DirectionEnum dir)
    {
        var offset = range / 2f;
        var d2r = DirectionAsRadians(dir);

        var v1 = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;
        var v2 = new Vector2(Mathf.Cos(d2r), Mathf.Sin(d2r)).normalized;
  
        return Vector2.Angle(v1, v2) >= offset ? false : true;
    }

    public static Tuple<int, int> DirectionAsOffset(DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.North:
                return new Tuple<int, int>(0, 1);
            case DirectionEnum.East:
                return new Tuple<int, int>(1, 0);
            case DirectionEnum.South:
                return new Tuple<int, int>(0, -1);
            case DirectionEnum.West:
                return new Tuple<int, int>(-1, 0);
            case DirectionEnum.NorthEast:
                return new Tuple<int, int>(1, 1);
            case DirectionEnum.NorthWest:
                return new Tuple<int, int>(-1, 1);
            case DirectionEnum.SouthWest:
                return new Tuple<int, int>(-1, -1);
            case DirectionEnum.SouthEast:
                return new Tuple<int, int>(1, -1);
            default:
                return new Tuple<int, int>(0, 0);
        }
    }
    public static int DirectionToDegrees(DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.North:
                return 0;
            case DirectionEnum.East:
                return 90;
            case DirectionEnum.South:
                return 180;
            case DirectionEnum.West:
                return 270;
            case DirectionEnum.NorthEast:
                return 45;
            case DirectionEnum.NorthWest:
                return 315;
            case DirectionEnum.SouthWest:
                return 225;
            case DirectionEnum.SouthEast:
                return 135;
            default:
                return 0;
        }
    }
    private static int DirectionAsDegree(DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.North:
                return n;
            case DirectionEnum.East:
                return e;
            case DirectionEnum.South:
                return s;
            case DirectionEnum.West:
                return w;
            case DirectionEnum.NorthEast:
                return ne;
            case DirectionEnum.NorthWest:
                return nw;
            case DirectionEnum.SouthWest:
                return sw;
            case DirectionEnum.SouthEast:
                return se;
            default:
                return n;
        }
    }
    private static float DirectionAsRadians(DirectionEnum dir)
    {
        return DirectionAsDegree(dir) * Deg2Rad;
    }
}

[Flags]
public enum DirectionEnum
{
    North = 1,
    East = 2,
    South = 4,
    West = 8,
    NorthEast = North | East,
    NorthWest = North | West,
    SouthWest = South | West,
    SouthEast = South | East
}
