/// Made by Dor Ben Dor, agesonera@gmail.com

using UnityEngine;

[HelpURL("https://easings.net")]
[CreateAssetMenu(fileName = "NewCurve", menuName = "Curve")]
public class Curve : ScriptableObject
{
    public AnimationCurve curve;

    public static explicit operator Curve(AnimationCurve curve)
    {
        var c = CreateInstance(typeof(Curve)) as Curve;
        c.curve = curve;
        return c;
    }

    public static implicit operator AnimationCurve(Curve curve)
    {
        return curve.curve;
    }
}
