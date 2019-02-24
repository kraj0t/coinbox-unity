using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DuroMotion : MonoBehaviour {

    public float maxAngularVelocity = 30f;    
    public int solverIterationCount = 8;
    //public bool useConeFriction = true;
    public bool applyCustomInertiaTensor = false;    
    public Vector3 inertiaTensor;
    public Vector3 inertiaTensorEuler;


    private Rigidbody m_body;
    private bool m_wasCustomInertiaTensor = false;


	void Start () {
        m_body = GetComponent<Rigidbody>();

        Debug.Log("inertiaTensor: " + m_body.inertiaTensor.ToString()); // 5, 10, 5
        Debug.Log("inertiaTensorRotation: " + m_body.inertiaTensorRotation.ToString()); // 0, 0.2, 0, 1
    }
	

	void FixedUpdate () {
        m_body.maxAngularVelocity = maxAngularVelocity;        
        if (applyCustomInertiaTensor)
        {
            m_body.inertiaTensor = inertiaTensor;
            m_body.inertiaTensorRotation = Quaternion.Euler(inertiaTensorEuler);
            // 5,5,5 and skip: pretty nice
        }
        else
        {
            if (m_wasCustomInertiaTensor)
            {
                m_body.ResetInertiaTensor();
                Debug.Log("inertiaTensor: " + m_body.inertiaTensor.ToString()); // 5, 10, 5
                Debug.Log("inertiaTensorRotation: " + m_body.inertiaTensorRotation.ToString()); // 0, 0.2, 0, 1
                //Debug.Log(new Quaternion(0f, 0.2f, 0f, 1f).eulerAngles.ToString()); <- 0, 22.6, 0
            }
        }
        m_wasCustomInertiaTensor = applyCustomInertiaTensor;

        m_body.solverIterationCount = solverIterationCount;
        //m_body.useConeFriction = useConeFriction; // <-- Unity broke the support for cone friction.
    }


    void OnGUI()
    {
        GUI.color = Color.blue;
        //GUI.Label(new Rect(5f, 20f, 400f, 30f), "angVel.magnitude: " + m_body.angularVelocity.magnitude.ToString());
    }
}
