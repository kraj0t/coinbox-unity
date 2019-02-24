using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DEBUG_SceneChangeButton : MonoBehaviour
{
    void OnGUI()
    {
        if ( GUI.Button( new Rect( 5f, 100f, 150f, 40f ), "CHANGE SCENE" ) ) {
            SceneManager.LoadScene( ( SceneManager.GetActiveScene().buildIndex + 1 ) % SceneManager.sceneCountInBuildSettings );
        }
    }
}
