using UnityEngine;
using System.Collections;


public class test_density : MonoBehaviour {

    public float density = 1f;
    public float mass;

    Rigidbody m_body;

    void Start()
    {
        m_body = GetComponent<Rigidbody>();
        mass = m_body.mass;
    }
	
	void Update () {
        m_body.SetDensity( density );
        mass = m_body.mass;
        m_body.ResetCenterOfMass();
        m_body.ResetInertiaTensor();
        m_body.WakeUp();
    }
}
