using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectController : MonoBehaviour
{
    [SerializeField]
    List<ParticleSystem> ParticleEffects;


    public void PlayeEffect(int EffectNum)
    {
        try
        {
            ParticleEffects[EffectNum].Play();
        }
        catch
        {
            Debug.Log("EffectError", this);
        }
    }
}
