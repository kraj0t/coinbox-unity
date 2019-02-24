using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DEBUG_Functions : MonoBehaviour
{
    public void SetShakeThresholdAllCoins(float f)
    {
        DuroMotion[] coins = Object.FindObjectsOfType<DuroMotion>();
        foreach (DuroMotion c in coins)
        {
            c.shakeThreshold = f;
        }
    }


    public void SetShakeAmountAllCoins(float f)
    {
        DuroMotion[] coins = Object.FindObjectsOfType<DuroMotion>();
        foreach (DuroMotion c in coins)
        {
            c.shakeImpulseFactor = f;
        }
    }


    public void InvertShakeAmountAllCoins()
    {
        DuroMotion[] coins = Object.FindObjectsOfType<DuroMotion>();
        foreach (DuroMotion c in coins)
        {
            c.shakeImpulseFactor = -c.shakeImpulseFactor;
        }
    }


    public void InvertUseGyroAllCoins()
    {
        DuroMotion[] coins = Object.FindObjectsOfType<DuroMotion>();
        foreach (DuroMotion c in coins)
        {
            c.useGyro = !c.useGyro;
        }
    }
}

