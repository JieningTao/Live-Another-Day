using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnTable : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5;

    private PartSwitchManager PSM;

    private void Start()
    {
        PSM = FindObjectOfType<PartSwitchManager>();
    }

    void Update()
    {
        transform.Rotate(0, Speed, 0);


        if (Input.GetKeyDown(KeyCode.Space))
            PSM.SlotRandomPart();
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");

    }
}
