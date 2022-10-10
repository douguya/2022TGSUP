using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("SEValue",1.0f);
    }

    public void SEOnValueChange(float newSliderValue)
    {
        SEManager sEManager = SEManager.Instance;
        sEManager.VolumeControl(Pa2Db(newSliderValue));
    }

    //��������f�V�x���֕ϊ�
    private float Pa2Db(float pa)
    {
        pa = Mathf.Clamp(pa, 0.0001f, 10f);
        return 20f * Mathf.Log10(pa);
    }
}
