using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class SetText : MonoBehaviour
    {
        public Slider slider;
        private Text txt;

        private void Awake()
        {
            txt = GetComponent<Text>();
            SetTextSlider();
        }

        public void SetTextSlider()
        {
            txt.text = slider.value.ToString("0.0");
        }
    }
}

