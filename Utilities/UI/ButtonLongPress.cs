using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Fields & Autoprops

    public UnityEvent OnHoldClick;
    public float holdTime;

    private float counter = 0;
    private bool toCalc = false;
    private Button btn;
    #endregion

    #region Methods

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void OnHoldEventHandler()
    {
        if (OnHoldClick != null)
            OnHoldClick.Invoke();
    }

    private IEnumerator Measure()
    {
        while (true)
        {
            counter += Time.deltaTime;
            btn.image.fillAmount = 1 - (counter / holdTime);

            if (counter < holdTime && toCalc == true)
                yield return null;
            else if (toCalc == true && counter > holdTime)
            {
                OnHoldEventHandler();
                break;
            }
            else
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toCalc = true;
        StartCoroutine(Measure());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        toCalc = false;
        btn.image.fillAmount = 1;

        if (counter >= holdTime)
            OnHoldEventHandler();

        counter = 0;
    }

    #endregion
}
