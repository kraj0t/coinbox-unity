using UnityEngine;
using System.Collections;


public class ScaleByScreenAspect : MonoBehaviour
{

    private Vector3 m_startLocalScale;


    void Start()
    {
        m_startLocalScale = transform.localScale;
    }

    
    void Update()
    {
        Vector3 newLocalScale = Vector3.one;
        newLocalScale.x = (float)Screen.width / Screen.height;
        newLocalScale.Scale(m_startLocalScale);

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

        //Debug.Log(Screen.width.ToString() + ", " + Screen.height.ToString());

        transform.localScale = newLocalScale;
    }
}
