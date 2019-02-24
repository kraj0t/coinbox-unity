using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DEBUG_VibrateTestButton : MonoBehaviour
{
    public long[] intervalos;

    void OnGUI()
    {
        if ( GUI.Button( new Rect( 5f, 200f, 150f, 40f ), "VIBRA" ) ) {
            Vibration.Vibrate(intervalos, -1);
        }
    }
}

