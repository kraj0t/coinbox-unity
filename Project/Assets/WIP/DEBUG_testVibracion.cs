using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DEBUG_testVibracion : MonoBehaviour
{
    public float interval1 = 0.05f;
    public float interval2 = 0.05f;

    public Slider interval1Slider;
    public Slider interval2Slider;

    public Text interval1Text;
    public Text interval2Text;

    public RectTransform firstPresetButton;


    public void SetInterval1(float f)
    {
        interval1 = f;
        interval1Slider.value = f;
        interval1Text.text = f.ToString("F3");
    }


    public void SetInterval2(float f)
    {
        interval2 = f;
        interval2Slider.value = f;
        interval2Text.text = f.ToString("F3");
    }


    public void StartVibrating()
    {
        if (Vibration.HasVibrator())
        {
            long[] patterns = new long[] { (long)(interval1 * 1000), (long)(interval2 * 1000) };
            Vibration.Vibrate(patterns, 0);
        }
    }


    public void StopVibrating()
    {
        if (Vibration.HasVibrator())
        {
            Vibration.Cancel();
        }
    }


    public void NewPreset()
    {
        GameObject go = GameObject.Instantiate<GameObject>(firstPresetButton.gameObject);

        RectTransform r = go.GetComponent<RectTransform>();
        r.SetParent(firstPresetButton.parent, false);

        int buttonCount = r.parent.childCount;
        Vector3 pos = r.localPosition;
        pos.x += r.rect.width * ((buttonCount - 1) % 8);
        pos.y -= r.rect.height * ((buttonCount - 1) / 8);
        r.localPosition = pos;

        Text t = r.GetChild(0).GetComponent<Text>();
        t.text = "PRESET " + buttonCount.ToString() + "\n[" + interval1.ToString("F3") + ",\n" + interval2.ToString("F3") + "]";

        Button b = go.GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        float i1 = interval1;
        float i2 = interval2;
        //b.onClick.AddListener(delegate { SetPresetValues(i1, i2); });
        b.onClick.AddListener(delegate { SetInterval1(i1); SetInterval2(i2); });
    }    
}
