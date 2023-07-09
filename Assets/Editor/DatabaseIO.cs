using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class DatabaseIO : EditorWindow
{
    private static string _DefaultSavePath = "";
    private static string DefaultSavedPath
    {
        get
        {
            if (_DefaultSavePath == "")
                _DefaultSavePath = Application.dataPath;
            return _DefaultSavePath;
        }
    }

    [Serializable]
    public class DBObject
    {
        public string PrefName = "";
        public PartSwitchManager.BigCataGory LOPCata;
        public string LOPName;
        public string LOPClass;
        public string LOPDes;
        public bool LOPHide;
        public List<string> LOPUnlock;

        public DBObject(string SheetData)
        {
            List<string> Temp = new List<string>();
            Temp.AddRange(SheetData.Split(new string[] { "," }, System.StringSplitOptions.None));
            PrefName = Temp[0];
            LOPCata = (PartSwitchManager.BigCataGory)int.Parse(Temp[1]);
            LOPName = Temp[2];
            LOPClass = Temp[3];
            LOPDes = Temp[4];
            LOPHide = bool.Parse(Temp[5]);

            LOPUnlock = new List<string>();
            LOPUnlock.AddRange(Temp[6].Split(new string[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries));
        }
        public DBObject(LoadOutPart LOPData)
        {
            PrefName = LOPData.gameObject.name;
            LOPCata = LOPData.PartCatagory;
            LOPName = LOPData.Name;
            LOPClass = LOPData.Classification;
            LOPDes = LOPData.Description;
            LOPHide = LOPData.HideForPlayer;
            LOPUnlock = LOPData.UnlockRequiredTags;
        }
        public virtual string GetString()
        {
            string Temp = "";

            Temp += PrefName + ",";
            Temp += LOPCata.GetHashCode() + ",";
            Temp += LOPName + ",";
            Temp += LOPClass + ",";
            Temp += LOPDes + ",";
            Temp += LOPHide + ",";
            Temp += string.Join("|", LOPUnlock) + ",";

            return Temp;
        }

        public virtual void ApplyToLOP(LoadOutPart LOP)
        {
            LOP.gameObject.name = PrefName;
            LOP.PartCatagory = LOPCata;
            LOP.Name = LOPName;
            LOP.Classification = LOPClass;
            LOP.Description = LOPDes;
            LOP.HideForPlayer = LOPHide;
            LOP.UnlockRequiredTags = LOPUnlock;
        }
    }

    [Serializable]
    public class DBMechPart : DBObject
    {
        public float PWeight;
        public float PHealth;

        public DBMechPart(string SheetData) :base(SheetData)
        {
            List<string> Temp = new List<string>();
            Temp.AddRange(SheetData.Split(new string[] { "," }, System.StringSplitOptions.None));

            PWeight = float.Parse(Temp[7]);
            PHealth = float.Parse(Temp[8]);
        }

        public DBMechPart(LOPMechPart LOPData):base(LOPData)
        {

            if (LOPData is LOPMechArms)
            {
                PWeight = LOPData.MyPart.Weight + (LOPData as LOPMechArms).MyOtherPart.Weight;
                PHealth = LOPData.MyPart.Health + (LOPData as LOPMechArms).MyOtherPart.Health;
            }
            else
            {
                PWeight = LOPData.MyPart.Weight;
                PHealth = LOPData.MyPart.Health;
            }

        }

        public override string GetString()
        {
            string Temp = base.GetString();

            Temp += PWeight + ",";
            Temp += PHealth + ",";

            return Temp;
        }

        public void ApplyToLOP(LOPMechPart LOP)
        {
            base.ApplyToLOP(LOP);
            if (LOP is LOPMechArms)
            {
                LOP.MyPart.Weight = PWeight / 2;
                (LOP as LOPMechArms).MyOtherPart.Weight = PWeight / 2;

                LOP.MyPart.Health = PHealth / 2;
                (LOP as LOPMechArms).MyOtherPart.Health = PHealth / 2;
            }
            else
            {
                LOP.MyPart.Weight = PWeight;
                LOP.MyPart.Health = PHealth;
            }
        }
    }

    [Serializable]
    public class DBFCS:DBObject
    {
        public int PerLockCount;
        public float LockTime;
        public int MaxLock;
        public float LockRange;
        public float RadarRange;
        public float AimAngle;

        public DBFCS(string SheetData) : base(SheetData)
        {
            List<string> Temp = new List<string>();
            Temp.AddRange(SheetData.Split(new string[] { "," }, System.StringSplitOptions.None));

            PerLockCount = int.Parse(Temp[7]);
            LockTime = float.Parse(Temp[8]);
            MaxLock = int.Parse(Temp[9]);
            LockRange = float.Parse(Temp[10]);
            RadarRange = float.Parse(Temp[11]);
            AimAngle = float.Parse(Temp[12]);
        
        }

        public DBFCS(LOPFCSChip LOPData) : base(LOPData)
        {
            PerLockCount = LOPData.MyChip.GetPerLockCount;
            LockTime = LOPData.MyChip.GetLockTime;
            MaxLock = LOPData.MyChip.GetMaxLock;
            LockRange = LOPData.MyChip.GetLockRange;
            RadarRange = LOPData.MyChip.GetRadarRange;
            AimAngle = LOPData.MyChip.GetAimAngle;
        }

        public override string GetString()
        {
            string Temp = base.GetString();

            Temp += PerLockCount + ",";
            Temp += LockTime + ",";
            Temp += MaxLock + ",";
            Temp += LockRange + ",";
            Temp += RadarRange + ",";
            Temp += AimAngle + ",";

            return Temp;
        }

        public void ApplyToLOP(LOPFCSChip LOP)
        {
            base.ApplyToLOP(LOP);
            LOP.MyChip.SetStats(PerLockCount,LockTime,MaxLock,LockRange,RadarRange,AimAngle);
        }
    }


    List<DBObject> DB = new List<DBObject>();
    List<LoadOutPart> AllLOP = new List<LoadOutPart>();




    private bool MechParts;
    private bool Weapons;
    private bool FCS;
    private bool BoostSystems;
    private bool EXGs;

    [MenuItem("Window/JieningTools/DatabaseIO")]
    public static void ShowWindow()
    {
        GetWindow<DatabaseIO>("DatabaseIO");
    }

    private void OnGUI()
    {
        MechParts = GUILayout.Toggle(MechParts, "MechParts");
        Weapons = GUILayout.Toggle(Weapons, "Weapons");
        FCS = GUILayout.Toggle(FCS, "FCS");
        BoostSystems = GUILayout.Toggle(BoostSystems, "BoostSystems");
        EXGs = GUILayout.Toggle(EXGs, "EXGs");

        if (GUILayout.Button("Prefab >>> Sheet"))
        {
            if (MechParts)
            {
                PTD(typeof(LOPMechPart));
                Debug.Log("Prefab>>"+DB.Count+"MP>>Sheet");
                MP_DTS();
            }
            if (FCS)
            {
                PTD(typeof(LOPFCSChip));
                Debug.Log("Prefab>>" + DB.Count + "FCS>>Sheet");
                DTS(typeof(DBFCS));
            }
        }


        if (GUILayout.Button("Prefab <<< Sheet"))
        {
            if (MechParts)
            {
                MP_STD();
                Debug.Log("Prefab<<"+DB.Count+"MP<<Sheet");
                MP_DTP();
            }

            if (FCS)
            {
                STD(typeof(DBFCS));
                Debug.Log("Prefab>>" + DB.Count + "FCS>>Sheet");
                DTP(typeof(LOPFCSChip));
            }
        }

        if (GUILayout.Button("*** TEST ***"))
        {
        }

    }

    private void LoadAllLOP(Type a)
    {
        AllLOP.Clear();
        if(a ==typeof(LOPMechPart))
            AllLOP.AddRange(Resources.LoadAll<LOPMechPart>(""));
        else if(a == typeof(LOPFCSChip))
            AllLOP.AddRange(Resources.LoadAll<LOPFCSChip>(""));
    }


    private void PTD(Type T)
    {
        LoadAllLOP(T);
        DB.Clear();

        if (T == typeof(LOPMechPart))
        {
            foreach (LOPMechPart a in AllLOP)
                DB.Add(new DBMechPart(a));
        }
        else if(T == typeof(LOPFCSChip))
        {
            foreach (LOPFCSChip a in AllLOP)
                DB.Add(new DBFCS(a));
        }


    }

    private void DTS(Type T)
    {
        string Temp = "";

        if (T == typeof(DBMechPart))
        {
            Temp = "MPDatabase.csv";
        }
        else if (T == typeof(DBFCS))
        {
            Temp = "FCSDatabase.csv";
        }

        using (FileStream Stream = new FileStream(Path.Combine(DefaultSavedPath, "Prefabs/Resources/"+Temp), FileMode.Create))
        {
            TextWriter TW = new StreamWriter(Stream);

            if (T == typeof(DBMechPart))
            {
                TW.WriteLine("Pref_Name,LOP_Cata,LOP_Name,LOP_Class,LOP_Description,LOP_Hide,LOP_UnlockTags,P_Weight,P_Health");
                foreach (DBMechPart a in DB)
                {
                    TW.WriteLine(a.GetString());
                    TW.Flush();
                }
            }
            else if (T == typeof(DBFCS))
            {
                TW.WriteLine("Pref_Name,LOP_Cata,LOP_Name,LOP_Class,LOP_Description,LOP_Hide,LOP_UnlockTags,FCS_PLC,FCS_LockTime,FCS_MaxLock,FCS_LockRange,FCS_RadarRange,FCS_AimAngle");
                foreach (DBFCS a in DB)
                {
                    TW.WriteLine(a.GetString());
                    TW.Flush();
                }
            }

        }
    }

    private void STD(Type T)
    {
        DB.Clear();

        string FN = "";

        if (T == typeof(DBMechPart))
        {
            FN = "MPDatabase.csv";
        }
        else if (T == typeof(DBFCS))
        {
            FN = "FCSDatabase.csv";
        }

        TextReader TR = new StreamReader(new FileStream(Path.Combine(DefaultSavedPath, "Prefabs/Resources/" + FN), FileMode.Open));

        TR.ReadLine();//skips the first line of titles
        String Temp = TR.ReadLine();
        while (Temp != null)
        {
            if (Temp[0] == (",")[0]) // i know this is dumb, but it's what i can think of at the time
            {
                if(T == typeof(DBMechPart))
                DB.Add(new DBMechPart(Temp));
                else if (T == typeof(DBFCS))
                DB.Add(new DBFCS(Temp));
            }
            Temp = TR.ReadLine();
        }
    }

    private void DTP(Type T)
    {
        LoadAllLOP(T);

        if (T == typeof(LOPMechPart))
        {
            foreach (DBMechPart a in DB)
            {
                for (int i = 0; i < AllLOP.Count; i++)
                {
                    if (AllLOP[i].gameObject.name == a.PrefName)
                    {
                        a.ApplyToLOP(AllLOP[i]);
                        PrefabUtility.SavePrefabAsset(AllLOP[i].gameObject);
                    }
                }
            }
        }
        else if (T == typeof(LOPFCSChip))
        {
            foreach (DBFCS a in DB)
            {
                for (int i = 0; i < AllLOP.Count; i++)
                {
                    if (AllLOP[i].gameObject.name == a.PrefName)
                    {
                        a.ApplyToLOP(AllLOP[i]);
                        PrefabUtility.SavePrefabAsset(AllLOP[i].gameObject);
                    }
                }
            }
        }


    }


    private void MP_PTD()
    {
        LoadAllLOP(typeof(LOPMechPart));
        DB.Clear();

        foreach (LOPMechPart a in AllLOP)
        {
            DB.Add(new DBMechPart(a));
        }
    }

    private void MP_DTS()
    {
        using (FileStream Stream = new FileStream(Path.Combine(DefaultSavedPath, "Prefabs/Resources/MPDatabase.csv"), FileMode.Create))
        {
            TextWriter TW = new StreamWriter(Stream);
            TW.WriteLine("Pref Name,LOP_Cata,LOP_Name,LOP_Class,LOP_Description,LOP_Hide,LOP_UnlockTags,P_Weight,P_Health");

            foreach (DBMechPart a in DB)
            {

                TW.WriteLine(a.GetString());
                TW.Flush();
            }
        }
    }

    private void MP_STD()
    {
        DB.Clear();

        TextReader TR = new StreamReader(new FileStream(Path.Combine(DefaultSavedPath, "Prefabs/Resources/MPDatabase.csv"), FileMode.Open));

        TR.ReadLine();//skips the first line of titles
        String Temp = TR.ReadLine();
        while (Temp != null)
        {
            if(!Temp.Contains(",,,,,,,,"))
                DB.Add(new DBMechPart(Temp));
            Temp = TR.ReadLine();
        }

       
    }

    private void MP_DTP()
    {
        LoadAllLOP(typeof(LOPMechPart));
        foreach (DBMechPart a in DB)
        {
            for (int i = 0; i < AllLOP.Count; i++)
            {
                if (AllLOP[i].gameObject.name == a.PrefName)
                {
                    a.ApplyToLOP(AllLOP[i]);
                    PrefabUtility.SavePrefabAsset(AllLOP[i].gameObject);
                }
            }
        }


    }


    //private void FCS_PTD()
    //{
    //    LoadAllLOP();
    //    DB.Clear();

    //    foreach (LOPMechPart a in AllLOP)
    //    {
    //        DB.Add(new DBMechPart(a));
    //    }
    //}
}
