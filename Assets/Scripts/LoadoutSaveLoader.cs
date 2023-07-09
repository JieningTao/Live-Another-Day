using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutSaveLoader : MonoBehaviour
{
    [SerializeField]
    private MechAssemblyRack MAR;
    [SerializeField]
    private LoadoutOption ListOptionPrefab;
    [SerializeField]
    private Transform ListParent;
    private List<LoadoutOption> LoadoutOptionGOs = new List<LoadoutOption>();
    private List<LoadoutSlot> LoadedLoadouts = new List<LoadoutSlot>();
    [SerializeField]
    private UnityEngine.UI.InputField NewLoadoutName;


    public class LoadoutSlot
    {
        public string Name;
        public string Loadout;
        public LoadoutSlot(string _Name, string _Loadout)
        {
            Name = _Name;
            Loadout = _Loadout;
        }
    }


    private void Start()
    {
        LoadFromFile();
    }

    private void OnDestroy()
    {
        WriteToFile();
    }

    #region Button Functions
    public void NewLoadOut()
    {
        if(NewLoadoutName.text == "")
        {
            NewLoadoutName.text = "Unnamed Loadout";
        }

        LoadedLoadouts.Add(new LoadoutSlot(NewLoadoutName.text, MAR.ConvertCurrentLoadoutToString()));



        CreateLOGO(NewLoadoutName.text, LoadedLoadouts.Count - 1);


        Debug.Log("Saved loadout '"+NewLoadoutName.text+"'");

        NewLoadoutName.text = "";


    }
    #endregion

    private void CreateLOGO(string Name, int SlotNum) //create loadout option gameobject
    {
        //needs to impliment a object pool system for the options

        LoadoutOption Temp = Instantiate(ListOptionPrefab, ListParent);

        Temp.CreateOption(this, Name, SlotNum);

        LoadoutOptionGOs.Add(Temp);
    }

    public void OverwriteLoad(int SlotNum)
    {
        Debug.Log("Loading Loadout From Slot " + SlotNum);
        MAR.CompleteLoadoutFromString(LoadedLoadouts[SlotNum].Loadout);
    }

    public void OverwriteSave(int SlotNum)
    {
        Debug.Log("Saving Loadout To Slot " + SlotNum);
        LoadedLoadouts[SlotNum].Loadout = MAR.ConvertCurrentLoadoutToString();
    }

    public void Delete(int SlotNum)
    {
        Debug.Log("Deleting Loadout From Slot " + SlotNum);
        LoadedLoadouts.RemoveAt(SlotNum);
        Destroy(LoadoutOptionGOs[SlotNum].gameObject);
        LoadoutOptionGOs.RemoveAt(SlotNum);

        UpdateButtonSlotNums(); // list positions change after removing an option

        Debug.Log(LoadedLoadouts.Count + "-" + LoadoutOptionGOs.Count);
    }

    private void UpdateButtonSlotNums()
    {
        for (int i = 0; i < LoadoutOptionGOs.Count; i++)
        {
            LoadoutOptionGOs[i].UpdateSlotNum(i);
        }
    }

    public void WriteToFile()
    {
        if (LoadedLoadouts.Count > 0)
        {
            string TempWrite = "";
            foreach (LoadoutSlot a in LoadedLoadouts)
            {
                TempWrite += "[SLO]" + a.Name + "[NL]" + a.Loadout;
            }
            SaveLoadManager.SaveData("SavedLoadouts", TempWrite);
        }
    }

    public void LoadFromFile()
    {
        string TempLoad = SaveLoadManager.LoadData("SavedLoadouts");
        if (TempLoad != null && TempLoad != "")
        {
            string[] Loaded = TempLoad.Split(new string[] { "[SLO]" }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string a in Loaded)
            {
                string[] Temp = a.Split(new string[] { "[NL]" }, System.StringSplitOptions.RemoveEmptyEntries);
                string Name = Temp[0];
                string Loadout = Temp[1];

                LoadedLoadouts.Add(new LoadoutSlot(Name, Loadout));
            }

            if(LoadedLoadouts.Count>0)
            {
                for(int i=0;i<LoadedLoadouts.Count;i++)
                {
                    CreateLOGO(LoadedLoadouts[i].Name, i);
                }
            }
        }
    }
}
