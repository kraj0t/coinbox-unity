using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class UITextSetter : MonoBehaviour {
    public void SetText(float f)
    {
        GetComponent<Text>().text = f.ToString( "F3" );
    }
}
