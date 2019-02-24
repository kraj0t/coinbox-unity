using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DEBUG_ShakeCoinsButton : MonoBehaviour
{
    public float shakeVelocity = 0.1f;


    void OnGUI()
    {
        if ( GUI.Button( new Rect( 5f, 150f, 150f, 40f ), "SHAKE COINS" ) ) {
            GameObject coinsParent = GameObject.Find("COINS");
            for (int i = 0; i < coinsParent.transform.childCount; i++ )
            {
                Vector3 randomVel = Random.insideUnitSphere * shakeVelocity;
                coinsParent.transform.GetChild(i).GetComponent<Rigidbody>().velocity = randomVel;
            }
        }
    }
}

