using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSelector : MonoBehaviour
{
    [SerializeField]
    private AudioSource MyAS;
    [SerializeField]
    private bool PlayOnAwake = true;
    [SerializeField]
    private float DestroyTimer = 1f;
    [SerializeField]
    private Vector2 VolumeRange = new Vector2(0.4f, 0.6f);
    [SerializeField]
    private List<AudioClip> ClipBank;



    // Start is called before the first frame update
    void Start()
    {
        if (PlayOnAwake)
            Play();
    }

    public void Play()
    {
        MyAS.clip = ClipBank[Random.Range(0, ClipBank.Count)];
        MyAS.volume = Random.Range(VolumeRange.x, VolumeRange.y);
        MyAS.Play();
        if(DestroyTimer>=0)
        Destroy(this.gameObject, DestroyTimer);
    }

}
