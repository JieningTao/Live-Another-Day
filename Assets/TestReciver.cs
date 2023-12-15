using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReciver : MonoBehaviour
{



    public void Trigger()
    {
        this.gameObject.SetActive(!gameObject.active);
    }
}
