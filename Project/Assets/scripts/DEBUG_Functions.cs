using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class DEBUG_Functions : MonoBehaviour
{
    public Transform floorObject;
    public float floorMoveSpeed = 2f;

    public GameObject coinPrefab;
    public Transform coinSpawnDummy;
    public float coinFadeTime = 1f;


    private float m_desiredLocalY;

    private HashSet<GameObject> m_objectsBeingDestroyed = new HashSet<GameObject>();


    void Start()
    {
        m_desiredLocalY = floorObject.localPosition.y;
    }


    void FixedUpdate()
    {
        _UpdateWallDistance();
    }


    public void SetShakeThresholdAllCoins( float f )
    {
        CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
        foreach ( CoinMotion c in coins ) {
            c.shakeThreshold = f;
        }
    }


    public void SetShakeAmountAllCoins( float f )
    {
        CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
        foreach ( CoinMotion c in coins ) {
            c.shakeImpulseFactor = f;
        }
    }


    public void InvertShakeAmountAllCoins()
    {
        CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
        foreach ( CoinMotion c in coins ) {
            c.invertedShakeDirection = !c.invertedShakeDirection;
        }
    }


    public void InvertUseGyroAllCoins()
    {
        CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
        foreach ( CoinMotion c in coins ) {
            c.useGyro = !c.useGyro;
        }
    }


    public void AddCoin()
    {
        GameObject c = GameObject.Instantiate<GameObject>( coinPrefab );
        c.transform.position = coinSpawnDummy.position;
    }


    public void RemoveCoin()
    {        
        CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
        bool destroyedOne = false;
        for (int i = 0; i < coins.Length && !destroyedOne; i++ ) {
            GameObject go = coins[i].gameObject;
            //if ( !m_objectsBeingDestroyed.Contains( go ) ) {
            //    StartCoroutine( DestroyGameObjectWithFadeOut( go ) );
            //    destroyedOne = true;
            //}
            GameObject.Destroy( go );
            destroyedOne = true;
        }
    }
    
    
    public void SetWallDistance( float d )
    {
        m_desiredLocalY = 10f - d;
    }


    private void _UpdateWallDistance()
    {
        Vector3 pos = floorObject.localPosition;
        pos.y = Mathf.Lerp( pos.y, m_desiredLocalY, floorMoveSpeed * Time.deltaTime );
        if ( Mathf.Abs( pos.y - m_desiredLocalY ) < 0.01f ) {
            pos.y = m_desiredLocalY;
        }

        CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
        foreach ( CoinMotion c in coins ) {
            c.GetComponent<Rigidbody>().WakeUp();
        }

        floorObject.localPosition = pos;
    }


    private IEnumerator DestroyGameObjectWithFadeOut( GameObject go )
    {
        m_objectsBeingDestroyed.Add( go );

        float startTime = Time.time;
        Renderer r = go.GetComponent<Renderer>();
        Color c = r.material.color;
        float originalAlpha = c.a;
        r.material.SetOverrideTag( "RenderType", "Transparent" );

        while ( Time.time < startTime + coinFadeTime ) {
            float t01 = ( Time.time - startTime ) / coinFadeTime;
            c.a = originalAlpha * ( 1f - t01 );
            r.material.color = c;
            yield return null;
        }

        yield return new WaitForEndOfFrame();        
        GameObject.Destroy( go );
        m_objectsBeingDestroyed.Remove( go );
    }
}

