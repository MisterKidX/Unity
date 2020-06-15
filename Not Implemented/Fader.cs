using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    Image _image;

    #region Interface

    [SerializeField] float _fadeTime;
    [SerializeField] UnityEvent FadeComplete = new UnityEvent();

    public void FadeOut()
    {
        _image.enabled = true;
        _image.color = Color.clear;
        _image.CrossFadeColor(Color.black, _fadeTime, true, true);
        StartCoroutine(OnFadeComplete(_fadeTime, false));
    }

    public void FadeIn()
    {
        _image.enabled = true;
        _image.color = Color.black;
        _image.CrossFadeColor(Color.clear, _fadeTime, true, true);
        StartCoroutine(OnFadeComplete(_fadeTime, true));
    }

    #endregion

    #region Implementation

    void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
    }

    IEnumerator OnFadeComplete(float waitTime, bool deactivate = true)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        if (FadeComplete != null)
        {
            FadeComplete.Invoke();
        }

        _image.enabled = deactivate ? false : true;
    }

    #endregion

}
