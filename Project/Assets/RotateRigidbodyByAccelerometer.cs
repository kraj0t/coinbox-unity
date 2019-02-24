using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RotateRigidbodyByAccelerometer : MonoBehaviour
{
    public float smoothSpeed = 4f;

    public Vector3 tiltFixEuler;


    private Rigidbody m_body;


    void Start()
    {
        m_body = GetComponent<Rigidbody>();

        Quaternion tiltFix = Quaternion.Euler(tiltFixEuler);
        Vector3 rawDeviceEuler = Input.acceleration.normalized * 180f;
        //Vector3 rawDeviceEuler = Input.gyro.attitude.eulerAngles;
        Vector3 fixedEuler = new Vector3(rawDeviceEuler.x, -rawDeviceEuler.z, rawDeviceEuler.y);
        //transform.rotation = gyroFix * Quaternion.Euler(fixedEuler);
        Quaternion fixedRot = tiltFix * Quaternion.Euler(fixedEuler);
        m_body.MoveRotation(fixedRot);
    }


    void Update()
    {
        Input.gyro.enabled = true;

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Input.acceleration.normalized * 180f), 8f * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Input.gyro.attitude, 8f * Time.deltaTime);
        Quaternion tiltFix = Quaternion.Euler(tiltFixEuler);
        Vector3 rawDeviceEuler = Input.acceleration.normalized * 180f;
        //Vector3 rawDeviceEuler = Input.gyro.attitude.eulerAngles;
        Vector3 fixedEuler = new Vector3(rawDeviceEuler.x, -rawDeviceEuler.z, rawDeviceEuler.y);
        //transform.rotation = gyroFix * Quaternion.Euler(fixedEuler);
        Quaternion fixedRot = tiltFix * Quaternion.Euler(fixedEuler);
        Quaternion smoothedRot = Quaternion.Lerp(m_body.rotation, fixedRot, smoothSpeed * Time.deltaTime);
        m_body.MoveRotation(smoothedRot);
    }
}
