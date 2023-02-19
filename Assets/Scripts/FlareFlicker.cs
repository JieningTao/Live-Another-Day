using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareFlicker : MonoBehaviour
{
    [SerializeField]
    float FlareIntensity = 0.8f;

    LensFlare FlareComponent;

    // Start is called before the first frame update
    void Start()
    {
        FlareComponent = GetComponent<LensFlare>();
    }

    // Update is called once per frame
    void Update()
    {
        Flicker();
    }

    private void Flicker()
    {
        FlareComponent.brightness = FlareIntensity + (Random.Range(-0.2f, 0.2f) * FlareIntensity);
    }
}
