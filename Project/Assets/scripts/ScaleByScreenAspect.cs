using UnityEngine;
using System.Collections;


public class ScaleByScreenAspect : MonoBehaviour
{

    private Vector3 m_startLocalScale;

    private float m_lastRatio;


    void Start()
    {
        m_startLocalScale = transform.localScale;

        m_lastRatio = (float)Screen.width / Screen.height;
        Vector3 newLocalScale = Vector3.one;
        newLocalScale.x = m_lastRatio;
        newLocalScale.Scale( m_startLocalScale );
        transform.localScale = newLocalScale;
    }

    
    void Update()
    {
        float newRatio = (float)Screen.width / Screen.height;
        if (newRatio != m_lastRatio) {
            m_lastRatio = newRatio;
            Vector3 newLocalScale = Vector3.one;
            newLocalScale.x = m_lastRatio;
            newLocalScale.Scale( m_startLocalScale );
            transform.localScale = newLocalScale;
        }
        

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

        
    }
}
