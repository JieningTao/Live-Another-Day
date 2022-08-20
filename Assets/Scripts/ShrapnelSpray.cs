using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrapnelSpray : MonoBehaviour
{
    [SerializeField]
    GameObject ProjectilePrefab;
    [SerializeField]
    ParticleSystem MuzzleFlare;
    [SerializeField]
    float AccuracyDeviation;

    [SerializeField]
    float SprayDuration = 1;
    [SerializeField]
    int SprayAmountPerTick =5;
    [SerializeField]
    float SprayTickTime = 0.2f;

    float SprayTickCD;




    private void Update()
    {

        if (SprayDuration > 0)
        {
            if (SprayTickCD > 0)
                SprayTickCD -= Time.deltaTime;
            else
            {
                for (int i = 0; i < SprayAmountPerTick; i++)
                {
                    Fire1();
                }
                if (MuzzleFlare != null)
                    MuzzleFlare.Play();

                SprayTickCD = SprayTickTime;
            }
            SprayDuration -= Time.deltaTime;
        }
        else if (SprayDuration < 0)
        {
            Destroy(gameObject);
        }

    }

    protected virtual void Fire1()
    {
        GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, transform.position, transform.rotation);
        NewBullet.SetActive(true);
        NewBullet.transform.Rotate(new Vector3(Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.World);

        
    }



}
