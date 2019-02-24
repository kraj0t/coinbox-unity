using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( Rigidbody ) )]
public class CoinImpactAudio : MonoBehaviour
{
    [System.Serializable]
    public struct ClipRange
    {
        // You can provide more than one clip. The final clip will be chosen randomly.
        public AudioClip[] clips;
        public float maxImpactVelocity;
    }

    [System.Serializable]
    public struct CoinAudioDefinition
    {
        public PhysicMaterial physicMaterial;
        public ClipRange[] impactClipRanges;
        public ClipRange[] slideClipRanges;
        public ClipRange[] rollClipRanges;
    }


    public CoinAudioDefinition[] impactAudioDefinitions;
    public CoinAudioDefinition[] slideAudioDefinitions;
    public CoinAudioDefinition[] rollAudioDefinitions;

    public float globalImpactVelocityMultiplier = 0.5f;
    public float globalMinImpactVelocity = 0.05f;

    public float globalSlideVelocityMultiplier = 0.5f;
    public float globalMinSlideVelocity = 0.05f;

    public float globalRollVelocityMultiplier = 0.5f;
    public float globalMinRollVelocity = 0.05f;

    public float minRandomPitchDistortion = 0.95f;
    public float maxRandomPitchDistortion = 1.05f;

    public float delayAfterImpact = 0.05f;


    private Dictionary<PhysicMaterial, CoinAudioDefinition> m_physMatDictionary;
    private int m_startAudioSourceCount = 16;
    private List<AudioSource> m_idleAudioSources;
    private Dictionary<Collider, AudioSource> m_currentSlideAudioSources;
    private Dictionary<Collider, AudioSource> m_currentRollAudioSources;


    void Start()
    {
        m_physMatDictionary = new Dictionary<PhysicMaterial, CoinAudioDefinition>( impactAudioDefinitions.Length );
        foreach ( CoinAudioDefinition iad in impactAudioDefinitions ) {
            m_physMatDictionary.Add( iad.physicMaterial, iad );
        }

        m_idleAudioSources = new List<AudioSource>();
        for ( int i = 0; i < m_startAudioSourceCount; i++ ) {
            m_idleAudioSources.Add( _GetIdleAudioSource() );
        }
        m_currentSlideAudioSources = new Dictionary<Collider, AudioSource>();
        m_currentRollAudioSources = new Dictionary<Collider, AudioSource>();
    }


    void OnCollisionEnter( Collision collision )
    {
        CoinAudioDefinition iad;
        if ( m_physMatDictionary.TryGetValue( collision.collider.sharedMaterial, out iad ) ) {
            // Loop clip ranges and find the right one to play, if any.
            float impactVelocity = globalImpactVelocityMultiplier * collision.relativeVelocity.magnitude;
            if ( impactVelocity >= globalMinImpactVelocity ) {

                ClipRange cr = _FindClipRangeByThreshold( iad.impactClipRanges, impactVelocity );
                AudioClip randomClip = cr.clips[ Random.Range( 0, cr.clips.Length ) ];

                float VOLUMEFACTOR = Mathf.Min( 1f, impactVelocity ) * 1.5f + 0.25f;
                AudioSource.PlayClipAtPoint( randomClip, collision.contacts[ 0 ].point, VOLUMEFACTOR );
                //Debug.Log("VOLUMEFACTOR: " + VOLUMEFACTOR.ToString("F2"));
                //Debug.Log("impactVelocity: " + impactVelocity.ToString("F2") + ", clipRange.minImpactVelocity: " + cr.maxImpactVelocity.ToString("F2"));
            }

            // TODO: clean this up a bit.
            if ( iad.slideClipRanges.Length != 0 ) {
                AudioSource slideAS = _GetIdleAudioSource();
                slideAS.clip = iad.slideClipRanges[ 0 ].clips[ 0 ];
                slideAS.loop = true;
                slideAS.time = Random.Range( 0f, slideAS.clip.length );
                slideAS.Play();
                m_currentSlideAudioSources.Add( collision.collider, slideAS );
            }
            if ( iad.rollClipRanges.Length != 0 ) {
                AudioSource rollAS = _GetIdleAudioSource();
                rollAS.clip = iad.rollClipRanges[ 0 ].clips[ 0 ];
                rollAS.loop = true;
                rollAS.time = Random.Range( 0f, rollAS.clip.length );
                rollAS.Play();
                m_currentRollAudioSources.Add( collision.collider, rollAS );
            }
        }
    }


    void OnCollisionStay( Collision collision )
    {
        // Retrieve the ongoing audioSources that were assigned to this collision at OnCollisionEnter.
        AudioSource slideAS;
        AudioSource rollAS;
        if ( m_currentSlideAudioSources.TryGetValue( collision.collider, out slideAS ) && m_currentRollAudioSources.TryGetValue( collision.collider, out rollAS ) ) {
            // The audioSources have been playing since OnCollisionEnter detected the collision.
            // So, in this block, we are going to determine the volume of both audios.
            float slideVolume = 1f;
            float rollVolume = 1f;

            // Find the average world normal from all contacts.
            Vector3 avgNormal = collision.contacts[ 0 ].normal;
            foreach ( ContactPoint cp in collision.contacts ) {
                avgNormal = avgNormal + cp.normal;
            }
            avgNormal /= (float)collision.contacts.Length;

            // Determine how tilted the coin is relative to the contact surface.
            // We first transport the contact normal from world space to the coin's local space.
            // The Z of the coin is the flat face. So, the more that the 
            // normal points to the coin's Z, the flatter the coin is contacting with the surface.
            //Vector3 normalCoinSpace = transform.rotation * avgNormal;
            //Mathf.Abs(normalCoinSpace.z)
            float slideRatio = Mathf.Abs( Vector3.Dot( avgNormal, transform.forward ) );

            // Distribute the volume of both audios depending on the slideRatio.
            slideVolume *= slideRatio;
            rollVolume *= 1f - slideRatio;

            // Also, depending on how fast the collision is being, we adjust the volume of the audios.
            float slideVelocity = collision.relativeVelocity.magnitude * globalSlideVelocityMultiplier;
            float rollVelocity = collision.relativeVelocity.magnitude * globalRollVelocityMultiplier;
            if ( slideVelocity < globalMinSlideVelocity ) {
                slideVelocity = 0f;
            }
            if ( rollVelocity < globalMinRollVelocity ) {
                rollVelocity = 0f;
            }
            slideVolume *= slideVelocity;
            rollVolume *= rollVelocity;

            // TODO: ajustar pitch segun la velocidad

            // TODO: ajustar el pitch de roll segun la angVel

            // QUIZAS NO HAGA FALTA hacer fadeIn (lerp) de ambos audios, teniendo en cuenta que el volumen esta a cero cuando empieza el contacto

            slideAS.volume = slideVolume;
            rollAS.volume = rollVolume;
        }
    }


    void OnCollisionExit( Collision collision )
    {
        // Retrieve the audioSources that were assigned to this collision at OnCollisionEnter.
        AudioSource slideAS;
        AudioSource rollAS;
        if ( m_currentSlideAudioSources.TryGetValue( collision.collider, out slideAS ) && m_currentRollAudioSources.TryGetValue( collision.collider, out rollAS ) ) {
            // Stop the audios, set their volume to zero, and set them back as idle.
            _SetIdleAudioSource( slideAS );
            _SetIdleAudioSource( rollAS );

            m_currentSlideAudioSources.Remove( collision.collider );
            m_currentRollAudioSources.Remove( collision.collider );
        }
    }


    private ClipRange _FindClipRangeByThreshold( ClipRange[] crArr, float impactVelocity )
    {
        int crIndexWithSmallerThreshold = -1;
        for ( int clipsIter = 0; clipsIter < crArr.Length; clipsIter++ ) {
            ClipRange c = crArr[ clipsIter ];
            if ( c.maxImpactVelocity >= impactVelocity ) {
                if ( crIndexWithSmallerThreshold == -1 ) {
                    crIndexWithSmallerThreshold = clipsIter;
                } else {
                    if ( c.maxImpactVelocity < crArr[ crIndexWithSmallerThreshold ].maxImpactVelocity ) {
                        crIndexWithSmallerThreshold = clipsIter;
                    }
                }
            }
        }

        if ( crIndexWithSmallerThreshold == -1 ) {
            crIndexWithSmallerThreshold = 0;
        }

        return crArr[ crIndexWithSmallerThreshold ];
    }


    private AudioSource _GetIdleAudioSource()
    {
        /*if ( m_idleAudioSources.Count == 0 ) {
            AudioSource au = gameObject.AddComponent<AudioSource>();
            au.volume = 0f;
            m_idleAudioSources.Add( au );
        }
        return m_idleAudioSources[ 0 ];
        */

        if ( m_idleAudioSources.Count == 0 ) {
            AudioSource au = gameObject.AddComponent<AudioSource>();
            au.volume = 0f;
            //au.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            au.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
            return au;
        } else {
            AudioSource au = m_idleAudioSources[ 0 ];
            m_idleAudioSources.RemoveAt( 0 );
            return au;
        }
    }


    private void _SetIdleAudioSource( AudioSource au )
    {
        au.Stop();
        au.volume = 0f;
        m_idleAudioSources.Add( au );
    }
}
