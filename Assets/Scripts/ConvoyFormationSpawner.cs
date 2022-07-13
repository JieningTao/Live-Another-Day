using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoyFormationSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject SpawnedEnemy;

    AIMNavAgent FollowerNav;

    // Start is called before the first frame update
    public void StartUp()
    {
        RaycastHit hit;
        int HitMask = LayerMask.GetMask("Terrain");

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, HitMask))
        {
            FollowerNav =  Instantiate(SpawnedEnemy, hit.point+new Vector3(0,0.4f,0), transform.rotation, null).GetComponent<AIMNavAgent>();

            FollowerNav.Spawn(transform);
            FollowerNav.enabled = true;
        }



    }

}
