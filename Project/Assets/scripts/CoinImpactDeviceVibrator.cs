using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CoinImpactDeviceVibrator : MonoBehaviour
{
    public float vibrateDurationFactor = 0.01f;
    public float relativeVelocityImpactThreshold = 0.1f;
    public float maxVibrateTime = 0.01f;

    void OnCollisionEnter(Collision collision)
    {
        float vibrationTime = vibrateDurationFactor * collision.relativeVelocity.magnitude;
        Debug.Log("vibrationTime: " + vibrationTime.ToString("F2"));
        if (vibrationTime > relativeVelocityImpactThreshold)
        {
            float vibrationSeconds = Mathf.Min(maxVibrateTime, vibrationTime);
            long vMillis = (long)(vibrationSeconds * 1000f);
            Vibration.Vibrate(vMillis);
        }
    }
}
