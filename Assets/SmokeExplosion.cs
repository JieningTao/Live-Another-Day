using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeExplosion : BaseExplosion
{
    [SerializeField]
    GameObject Collider;
    [SerializeField]
    ParticleSystem Effect;
    [SerializeField]
    float Duration;

    private void Start()
    {
        Effect.Play();
        Collider.SetActive(true);
    }

    private void Update()
    {
        Duration -= Time.deltaTime;

        if (Duration <= 0)
        {
            Effect.Stop();
            Destroy(gameObject, DestroyTimer);
        }


    }
}
