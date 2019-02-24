using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
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
    public struct ImpactAudioDefinition
    {
        public LayerMask layers;
        public ClipRange[] clipRanges;
    }


    public ImpactAudioDefinition[] impactAudioDefinitions;

    public float globalImpactVelocityMultiplier = 1f;
    public float globalMinImpactVelocity = 0.1f;

    public float minRandomPitchDistortion = 0.95f;
    public float maxRandomPitchDistortion = 1.05f;

    public float delayAfterImpact = 0.05f;


    void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = globalImpactVelocityMultiplier * collision.relativeVelocity.magnitude;
        if (impactVelocity >= globalMinImpactVelocity)
        //if (impactVelocity < globalMinImpactVelocity)
        //    Debug.Log("impactVelocity: " + impactVelocity.ToString("F2"));
        //else
        {
            bool foundLayer = false;
            for (int impactDefIter = 0; impactDefIter < impactAudioDefinitions.Length && !foundLayer; impactDefIter++)
            {
                ImpactAudioDefinition iad = impactAudioDefinitions[impactDefIter];
                if ((iad.layers & 1 << collision.gameObject.layer) != 0)
                {
                    foundLayer = true;

                    // Loop clip ranges and find the right one to play.
                    //ClipRange crWithSmallerThreshold = iad.clipRanges[0];
                    int crIndexWithSmallerThreshold = -1;
                    for (int clipsIter = 0; clipsIter < iad.clipRanges.Length; clipsIter++)
                    {
                        ClipRange c = iad.clipRanges[clipsIter];
                        if (c.maxImpactVelocity >= impactVelocity)
                        {
                            if (crIndexWithSmallerThreshold == -1)
                            {
                                crIndexWithSmallerThreshold = clipsIter;
                            }
                            else
                            {
                                if (c.maxImpactVelocity < iad.clipRanges[crIndexWithSmallerThreshold].maxImpactVelocity)
                                {
                                    crIndexWithSmallerThreshold = clipsIter;
                                }
                            }
                        }
                    }

                    if (crIndexWithSmallerThreshold == -1 )
                    {
                        crIndexWithSmallerThreshold = 0;
                    }

                    ClipRange cr = iad.clipRanges[crIndexWithSmallerThreshold];
                    AudioClip randomClip = cr.clips[Random.Range(0, cr.clips.Length)];
                    float VOLUMEFACTOR = Mathf.Min(1f, impactVelocity) * 1.5f + 0.25f;
                    
                    AudioSource.PlayClipAtPoint(randomClip, collision.contacts[0].point, VOLUMEFACTOR);
                    //Debug.Log("VOLUMEFACTOR: " + VOLUMEFACTOR.ToString("F2"));
                    //Debug.Log("impactVelocity: " + impactVelocity.ToString("F2") + ", clipRange.minImpactVelocity: " + cr.maxImpactVelocity.ToString("F2"));
                }
            }
        }
    }
}
