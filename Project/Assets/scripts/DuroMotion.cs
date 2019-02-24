using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DuroMotion : MonoBehaviour
{

    public float shakeThreshold = 0.1f;
    public float shakeImpulseFactor = 0.01f;

    public float maxAngularVelocity = 30f;
    public int solverIterationCount = 8;
    //public bool useConeFriction = true;
    public bool applyCustomInertiaTensor = false;
    public Vector3 inertiaTensor;
    public Vector3 inertiaTensorEuler;

    public float vibrateTime = 0.1f;

    public bool useGyro = true;


    private Rigidbody m_body;
    private bool m_wasCustomInertiaTensor = false;


    void Start()
    {
        m_body = GetComponent<Rigidbody>();

        Debug.Log("inertiaTensor: " + m_body.inertiaTensor.ToString()); // 5, 10, 5
        Debug.Log("inertiaTensorRotation: " + m_body.inertiaTensorRotation.ToString()); // 0, 0.2, 0, 1
    }


    void FixedUpdate()
    {
        m_body.maxAngularVelocity = maxAngularVelocity;
        if (applyCustomInertiaTensor)
        {
            m_body.inertiaTensor = inertiaTensor;
            m_body.inertiaTensorRotation = Quaternion.Euler(inertiaTensorEuler);
            // 5,5,5 and skip: pretty nice
        }
        else {
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



        if (!useGyro)
        {
            Vector3 a = Input.acceleration;
            Vector3 n = a.normalized;
            Vector3 excessImpulse = a - n;
            if (excessImpulse.magnitude > shakeThreshold)
            {
                //if (a.magnitude - 1f > shakeThreshold)
                {
                    Vector3 impulse = -excessImpulse * shakeImpulseFactor;
                    m_body.AddForce(impulse, ForceMode.VelocityChange);
                }
            }
            else {
                if (Input.gyro.userAcceleration.magnitude > shakeThreshold)
                {
                    m_body.AddForce(Input.gyro.userAcceleration * shakeImpulseFactor, ForceMode.VelocityChange);
                }
            }
        }
    }


    void OnGUI()
    {
        GUI.color = Color.blue;
        //GUI.Label(new Rect(5f, 20f, 400f, 30f), "angVel.magnitude: " + m_body.angularVelocity.magnitude.ToString());

        Vector3 a = Input.acceleration;
        Vector3 n = a.normalized;
        Vector3 excessImpulse = a - n;
        //if ( excessImpulse.magnitude > shakeThreshold ) {
        if (a.magnitude - 1f > shakeThreshold)
        {
            Vector3 impulse = excessImpulse * shakeImpulseFactor;
            m_body.AddForce(-impulse, ForceMode.VelocityChange);
        }
        //GUI.Label( new Rect( 5f, 50f, 400f, 30f ), ( Mathf.Max( 0f, a.magnitude - 1f ) ).ToString() );
        if (Time.time % 1.0 < 0.1f)
        {
            remember1 = Input.gyro.userAcceleration;
            remember2 = excessImpulse;
        }
        GUI.Label(new Rect(5f, 50f, 400f, 30f), "gyro.userAcceleration: " + remember1.ToString());
        GUI.Label(new Rect(5f, 65f, 400f, 30f), "excessImpulse: " + remember2.ToString());
    }
    Vector3 remember1;
    Vector3 remember2;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 0.1f)
        {
            long vMillis = (long)(vibrateTime * collision.relativeVelocity.magnitude * 1000f);
            Vibration.Vibrate(vMillis);
        }
    }

}
