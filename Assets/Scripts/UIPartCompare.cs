using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPartCompare : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text CurrentName;
    [SerializeField]
    private GameObject CurrentStatParent;
    private List<GameObject> CurrentStats = new List<GameObject>();
    [SerializeField]
    private UnityEngine.UI.Text CurrentDescription;

    [Space(20)]

    [SerializeField]
    private UnityEngine.UI.Text SelectedName;
    [SerializeField]
    private GameObject SelectedStatParent;
    private List<GameObject> SelectedStats = new List<GameObject>();
    [SerializeField]
    private UnityEngine.UI.Text SelectedDescription;

    [Space(20)]

    [SerializeField]
    private GameObject StatPrefab;
    [SerializeField]
    private UnityEngine.UI.Text LeftText;
    [SerializeField]
    private UnityEngine.UI.Text RightText;





    private GameObject CreateStat(GameObject Parent, string Left, string Right)
    {
        LeftText.text = Left;
        RightText.text = Right;

        GameObject a = Instantiate(StatPrefab, Parent.transform);
        a.SetActive(true);
        return a;
    }

    private void ClearSelectedStats()
    {
        foreach (GameObject a in SelectedStats)
        {
            Destroy(a);
        }
        SelectedStats.Clear();
    }

    public void LoadSelectedPart(LoadOutPart a)
    {
        ClearSelectedStats();
        if (a)
        {
            SelectedName.text = a.Name;

            List<string> Temp = a.GetStats();

            for (int i = 0; i < Temp.Count; i += 2)
            {
                SelectedStats.Add(CreateStat(SelectedStatParent, Temp[i], Temp[i + 1]));
            }
            SelectedDescription.text = a.Description;
        }
        else
        {
            SelectedName.text = "";
            SelectedDescription.text = "";
        }

    }


    private void ClearCurrentStats()
    {
        foreach (GameObject a in CurrentStats)
        {
            Destroy(a);
        }
        CurrentStats.Clear();
        CurrentName.text = "";
    }

    public void LoadCurrentPart(LoadOutPart a)
    {
        ClearCurrentStats();
        if (a)
        {
            CurrentName.text = a.Name;

            List<string> Temp = a.GetStats();

            for (int i = 0; i < Temp.Count; i += 2)
            {
                CurrentStats.Add(CreateStat(CurrentStatParent, Temp[i], Temp[i + 1]));
            }
            CurrentDescription.text = a.Description;
        }
        else
        {
            SelectedName.text = "";
            CurrentDescription.text = "";
        }

    }
}
