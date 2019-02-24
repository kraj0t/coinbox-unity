using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DEBUG_testInputLinearAcceleration : MonoBehaviour {

    public Transform accelerometerObject;
    public Transform gyroscopeObject;

    public float moveFactor = 1f;

    public Slider accX;
    public Slider accY;
    public Slider accZ;

    public Slider gyroX;
    public Slider gyroY;
    public Slider gyroZ;

	
	void Update () {
        Vector3 a = Input.acceleration;
        Vector3 aN = a.normalized;
        Vector3 accelImpulse = a - aN;
        
        Vector3 g = Input.gyro.userAcceleration;
        Vector3 gyroImpulse = new Vector3( g.x, g.z, -g.y );

        accelerometerObject.position += accelImpulse * Time.deltaTime;
        gyroscopeObject.position += gyroImpulse * Time.deltaTime;

        accX.value = accelImpulse.x;
        accY.value = accelImpulse.y;
        accZ.value = accelImpulse.z;

        gyroX.value = gyroImpulse.x;
        gyroY.value = gyroImpulse.y;
        gyroZ.value = gyroImpulse.z;




        a = Input.acceleration;
        a = new Vector3( a.x, a.y, -a.z );
        _linearAccelAmount = Mathf.Abs( a.magnitude - 1f );
        //Vector3 accelImpulse = a * invertFactor * linearAccelAmount;
        //accX.value = Mathf.Lerp( accX.value -1f, _linearAccelAmount - 1f, 2f * Time.deltaTime);
        accX.value = _linearAccelAmount - 1f;
    }

    void OnGUI()
    {
        GUI.color = Color.yellow;
        GUI.Label(new Rect(5,90,300,30), "_linearAccelAmount: " + _linearAccelAmount.ToString() );
    }

    private float _linearAccelAmount;
}
