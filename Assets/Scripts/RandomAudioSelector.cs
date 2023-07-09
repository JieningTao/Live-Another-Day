using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSelector : MonoBehaviour
{
    [SerializeField]
    private AudioSource MyAS;
    [SerializeField]
    private float Delay;
    [SerializeField]
    private bool PlayOnAwake = true;
    [Tooltip("Set to negative to not destroy")]
    [SerializeField]
    private float DestroyTimer = 1f;
    [SerializeField]
    private Vector2 VolumeRange = new Vector2(1, 1);
    [SerializeField]
    private List<AudioClip> ClipBank;

    bool PlayInitiated;
    bool AlreadyPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayOnAwake)
            Play();

    }

    private void Update()
    {
        if (PlayInitiated)
        {
            if (Delay > 0)
                Delay -= Time.deltaTime;

            if (Delay <= 0 && !AlreadyPlayed)
                Play();
        }


    }

    public void Play()
    {
        if (Delay > 0)
        {
            PlayInitiated = true;
            Debug.Log("Start delay");
        }
        else
        {
            MyAS.clip = ClipBank[Random.Range(0, ClipBank.Count)];
            MyAS.volume = Random.Range(VolumeRange.x, VolumeRange.y);
            MyAS.Play();

            if (DestroyTimer >= 0)
                Destroy(this.gameObject, DestroyTimer);

            AlreadyPlayed = true;
        }


    }

}
