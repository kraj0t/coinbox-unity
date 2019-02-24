using UnityEngine;
using System.Collections;


public class ScaleByScreenAspect : MonoBehaviour
{

    void Start()
    {

    }

    
    void Update()
    {
        Vector3 newLocalScale = transform.localScale;
        newLocalScale.x = (float)Screen.width / Screen.height;
        newLocalScale.z = 1f;

        /*if (Screen.width > Screen.height)
        {
            newLocalScale.x = (float)Screen.width / Screen.height;
            newLocalScale.z = 1f;
        }
        else
        {
            newLocalScale.x = 1f;
            newLocalScale.z = (float)Screen.height / Screen.width;
        }*/

        Debug.Log(Screen.width.ToString() + ", " + Screen.height.ToString());

        transform.localScale = newLocalScale;
    }
}
