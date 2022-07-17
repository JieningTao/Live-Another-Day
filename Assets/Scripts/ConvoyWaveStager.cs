using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoyWaveStager : MonoBehaviour
{
    [SerializeField]
    float Delay = 0;






    List<FormationTravelMount> WaveTravelMounts;




    private void Start()
    {
        WaveTravelMounts = new List<FormationTravelMount>();

        WaveTravelMounts.AddRange(GetComponentsInChildren<FormationTravelMount>());

        foreach (FormationTravelMount a in WaveTravelMounts)
        {
            a.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Delay > 0)
            Delay -= Time.deltaTime;
        else if (!WaveTravelMounts[0].gameObject.active)
        {
            foreach (FormationTravelMount a in WaveTravelMounts)
            {
                a.gameObject.SetActive(true);
            }
        }

    }




}
