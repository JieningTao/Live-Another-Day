using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> TravelNodes;

    [SerializeField]
    private bool DrawRail;





    private void OnDrawGizmos()
    {
        if (DrawRail)
        {
            for (int i = 0; i < TravelNodes.Count - 1; i++)
            {
                Debug.DrawLine(TravelNodes[i].position, TravelNodes[i + 1].position, Color.blue);
            }
        }
    }

    public Transform GetNextNode(Transform a)
    {
        if (a == null)
            return TravelNodes[1];

        for (int i = 0; i < TravelNodes.Count; i++)
        {
            if (TravelNodes[i] == a)
            {
                if (i == TravelNodes.Count - 1)
                    return null;
                else
                    return TravelNodes[i + 1];
            }
        }

        return null;
    }

    public Transform GetFirstNode()
    {
        return TravelNodes[0];
    }

    public Transform GetInitialNode()
    {
        return TravelNodes[1];
    }





}
