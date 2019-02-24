using UnityEngine;
using System.Collections;

public class DEBUG_duroSpawner : MonoBehaviour {

    public Rigidbody loQueVieneSiendoElDuro;

    public Vector3 randomThrowAngles = new Vector3(10f, 10f, 10f);

    public float minThrowForce = 0.1f;
    public float maxThrowForce = 1f;

    public Vector3 minTorqueForce = Vector3.zero;
    public Vector3 maxTorqueForce = Vector3.zero;

    public float coinLifetime = 3f;

    public int mouseButton = 0;


    void Update () {
	    if (Input.GetMouseButtonDown(mouseButton) ) {
            GameObject go = GameObject.Instantiate<GameObject>(loQueVieneSiendoElDuro.gameObject);
            go.transform.position = transform.position;
            go.transform.rotation = _GetQuatRandomEuler(randomThrowAngles) * transform.rotation;
            //go.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-randomTorqueForce.x, randomTorqueForce.x), Random.Range(-randomTorqueForce.y, randomTorqueForce.y), Random.Range(-randomAngularVelocity.z, randomAngularVelocity.z) );
            go.GetComponent<Rigidbody>().AddRelativeTorque( Random.Range(minTorqueForce.x, maxTorqueForce.x), Random.Range(minTorqueForce.y, maxTorqueForce.y), Random.Range(minTorqueForce.z, maxTorqueForce.z), ForceMode.Impulse );
            go.GetComponent<Rigidbody>().AddForce(go.transform.forward * Random.Range(minThrowForce, maxThrowForce), ForceMode.Impulse );
            
            GameObject.Destroy(go, coinLifetime);
        }
	}


    void OnGUI()
    {
        GUI.color = Color.blue;
        GUI.Label(new Rect(5f, 5f, 400f, 30f), "Botón izq: random, botón der: acierto");
    }


    private Quaternion _GetQuatRandomEuler( Vector3 v )
    {
        return Quaternion.Euler(
            Random.Range(-v.x, v.x),
            Random.Range(-v.y, v.y),
            Random.Range(-v.y, v.z)
            );
    }
}

