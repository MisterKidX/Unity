/// Made by Dor Ben Dor, agesonera@gmail.com

using System.Collections;
using UnityEngine;

public static class Terp
{
    /// <summary>
    /// Easily get a value from a range based on curve.
    /// </summary>
    /// <param name="a">start value</param>
    /// <param name="b">end value</param>
    /// <param name="curve">The given curve</param>
    /// <param name="curveSample">the interpolation value normalized</param>
    /// <returns>the interpolated value according to the curve</returns>
    public static float Interpolate(float a, float b, Curve curve, float curveSample)
    {
        curveSample = Mathf.Clamp01(curveSample);

        return Mathf.LerpUnclamped(a, b, curve.curve.Evaluate(curveSample));
    }

    /// <summary>
    /// Easily interpolate using time
    /// </summary>
    /// <param name="a">start value</param>
    /// <param name="b">end value</param>
    /// <param name="curve">The given curve</param>
    /// <param name="smoothTime">how much time until the interpolation is completed?</param>
    /// <param name="counter">reference counter. start from zero for default.</param>
    /// <param name="deltaTime">the delta of the interpolation. default is Time.deltatime</param>
    /// <returns></returns>
    public static float Interpolate(float a, float b, Curve curve, float smoothTime, ref float counter, float deltaTime = 0)
    {
        deltaTime = deltaTime <= 0 ? Time.deltaTime : deltaTime;
        smoothTime = Mathf.Clamp(smoothTime, 0.001f, int.MaxValue);

        counter += deltaTime;

        return Mathf.LerpUnclamped(a, b, curve.curve.Evaluate(counter / smoothTime));
    }
}
