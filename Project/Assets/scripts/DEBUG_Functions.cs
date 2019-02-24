using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class DEBUG_Functions : MonoBehaviour
{
    public Transform floorObject;
    public float floorMoveSpeed = 2f;

    public float coinScalingSpeed = 2f;

    public GameObject coinPrefab;
    public Transform coinSpawnDummy;
    public float coinFadeTime = 1f;

    public float shakeVelocity = 0.1f;

    public Renderer[] boxObjects;
    public LayerMask coinboxLayer;
    public Material[] renderMaterials;
    public PhysicMaterial[] physMaterials;


    private float m_desiredLocalY;
    private float m_desiredCoinScale;
    private float m_currentCoinScale;
    private float m_coinScalingSpeed;
    private int m_currentMaterialIndex;

    private HashSet<GameObject> m_objectsBeingDestroyed = new HashSet<GameObject>();


    void Start()
    {
        m_desiredLocalY = floorObject.localPosition.y;
        m_desiredCoinScale = coinPrefab.transform.localScale.x;
        m_currentCoinScale = m_desiredCoinScale;
    }


    void FixedUpdate()
    {
        _UpdateWallDistance();
        _UpdateCoinsScale();
    }


    public void SetVibrationThresholdAllCoins( float f )
    {
        CoinImpactDeviceVibrator[] coins = Object.FindObjectsOfType<CoinImpactDeviceVibrator>();
        foreach ( CoinImpactDeviceVibrator c in coins ) {
            c.relativeVelocityImpactThreshold = f;
        }
    }


    public void SetVibrationAmountAllCoins( float f )
    {
        CoinImpactDeviceVibrator[] coins = Object.FindObjectsOfType<CoinImpactDeviceVibrator>();
        foreach ( CoinImpactDeviceVibrator c in coins ) {
            c.vibrateDurationFactor = f;
        }
    }


    public void SetVibrationMaxDurationAllCoins( float f )
    {
        CoinImpactDeviceVibrator[] coins = Object.FindObjectsOfType<CoinImpactDeviceVibrator>();
        foreach ( CoinImpactDeviceVibrator c in coins ) {
            c.maxVibrateTime = f;
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
        for ( int i = 0; i < coins.Length && !destroyedOne; i++ ) {
            GameObject go = coins[ i ].gameObject;
            if ( !m_objectsBeingDestroyed.Contains( go ) ) {
                StartCoroutine( DestroyGameObjectWithFadeOut( go ) );
                destroyedOne = true;
            }
            //GameObject.Destroy( go );
            //destroyedOne = true;
        }
    }


    public void SetWallDistance( float d )
    {
        m_desiredLocalY = 10f - d;
    }


    public void SetCoinScale( float s )
    {
        m_desiredCoinScale = s;
    }


    public void ChangeScene()
    {
        SceneManager.LoadScene( ( SceneManager.GetActiveScene().buildIndex + 1 ) % SceneManager.sceneCountInBuildSettings );
    }


    public void ShakeCoins()
    {
        //Vector3 randomVel = ( Random.insideUnitSphere * 0.1f - Physics.gravity.normalized ) * shakeVelocity;
        Vector3 randomVel = -Physics.gravity.normalized * shakeVelocity;
        Vector3 randomAngVel = Random.insideUnitSphere * shakeVelocity * 10f;
        CoinImpactDeviceVibrator[] coins = Object.FindObjectsOfType<CoinImpactDeviceVibrator>();
        foreach ( CoinImpactDeviceVibrator c in coins ) {
            c.GetComponent<Rigidbody>().velocity = randomVel;
            c.GetComponent<Rigidbody>().angularVelocity = randomAngVel;
        }
    }


    public void ChangeMaterial()
    {
        m_currentMaterialIndex = ( m_currentMaterialIndex + 1 ) % renderMaterials.Length;
        Material rm = renderMaterials[ m_currentMaterialIndex ];
        PhysicMaterial pm = physMaterials[ m_currentMaterialIndex ];

        var allRenderers = FindObjectsOfType<Collider>();
        var rList = new System.Collections.Generic.List<Collider>();
        for ( var i = 0; i < allRenderers.Length; i++ ) {
            if ( 1 << allRenderers[ i ].gameObject.layer == coinboxLayer.value && !allRenderers[ i ].gameObject.name.Contains( "glass" ) ) {
                rList.Add( allRenderers[ i ] );
            }
        }
        Collider[] objArr = rList.ToArray();

        foreach ( Collider r in objArr ) {
            r.GetComponent<Renderer>().sharedMaterial = rm;
            r.GetComponent<Collider>().sharedMaterial = pm;
        }
    }


    private void _UpdateWallDistance()
    {
        if ( floorObject.localPosition.y != m_desiredLocalY ) {
            Vector3 pos = floorObject.localPosition;
            pos.y = Mathf.Lerp( pos.y, m_desiredLocalY, Time.deltaTime / floorMoveSpeed );
            if ( Mathf.Abs( pos.y - m_desiredLocalY ) < 0.01f ) {
                pos.y = m_desiredLocalY;
            }

            CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
            foreach ( CoinMotion c in coins ) {
                c.GetComponent<Rigidbody>().WakeUp();
            }

            floorObject.localPosition = pos;
        }
    }


    private void _UpdateCoinsScale()
    {
        if ( m_desiredCoinScale != m_currentCoinScale ) {
            m_currentCoinScale = Mathf.SmoothDamp( m_currentCoinScale, m_desiredCoinScale, ref m_coinScalingSpeed, 1f / coinScalingSpeed );

            CoinMotion[] coins = Object.FindObjectsOfType<CoinMotion>();
            foreach ( CoinMotion c in coins ) {
                c.transform.localScale = new Vector3( m_currentCoinScale, m_currentCoinScale, m_currentCoinScale );
                //c.GetComponent<MeshCollider>().contactOffset = 0.001f * (m_currentCoinScale / 0.275f);
                c.GetComponent<Rigidbody>().ResetCenterOfMass();
                c.GetComponent<Rigidbody>().ResetInertiaTensor();
                c.GetComponent<Rigidbody>().WakeUp();
            }

        }
    }


    private IEnumerator DestroyGameObjectWithFadeOut( GameObject go )
    {
        m_objectsBeingDestroyed.Add( go );

        float startTime = Time.time;
        Renderer r = go.GetComponent<Renderer>();
        Color origColor = r.material.color;
        r.material.shader = Shader.Find( "Standard (Specular setup)" );
        Color origSpecColor = r.material.GetColor( "_SpecColor" );
        r.material.SetOverrideTag( "RenderType", "Transparent" );
        r.material.SetInt( "_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One );
        r.material.SetInt( "_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha );
        r.material.SetInt( "_ZWrite", 0 );
        r.material.DisableKeyword( "_ALPHATEST_ON" );
        r.material.DisableKeyword( "_ALPHABLEND_ON" );
        r.material.EnableKeyword( "_ALPHAPREMULTIPLY_ON" );
        r.material.renderQueue = 3000;

        while ( Time.time < startTime + coinFadeTime ) {
            float t01 = ( Time.time - startTime ) / coinFadeTime;
            r.material.color = origColor * ( 1f - t01 );
            Color spec = origSpecColor;
            spec.a *= ( 1f - t01 );
            r.material.SetColor( "_SpecColor", spec );
            r.material.SetFloat( "_Smoothness", spec.a );
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        GameObject.Destroy( go );
        m_objectsBeingDestroyed.Remove( go );
    }
}

