using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class IgnoreAlphaHit : MonoBehaviour 
{
    #region Fields & Autoprops

    private Image _image;

    #endregion

    #region Unity Locals

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = 0.95f;
    }

    #endregion
}
