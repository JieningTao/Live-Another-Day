using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( BaseMechMain))]
public class MechLoader : MonoBehaviour
{

    [SerializeField]
    string LoadKey;

    private BaseMechMain MyMech;

    void Start()
    {
        MyMech = GetComponent<BaseMechMain>();

        if (PlayerPrefs.HasKey(LoadKey))
        {
            Debug.Log("Saved Loadout detected, loading...");

            string a = PlayerPrefs.GetString(LoadKey);
            Debug.Log(a);

            List<List<LoadOutPart>> LoadedLoadout = SaveCoder.LoadLoadout(a);

            List<LoadOutPart> LoadoutBodyPart = LoadedLoadout[0];
            List<LoadOutPart> LoadoutMainEquipment = LoadedLoadout[1];
            List<LoadOutPart> LoadoutEXGs = LoadedLoadout[2];


            BaseMechPartHead Head = LoadoutBodyPart[0].GetComponent<BaseMechPartHead>();
            BaseMechPartTorso Torso = LoadoutBodyPart[1].GetComponent<BaseMechPartTorso>();
            //Arms cannot use the get comp method
            BaseMechPartLegs Legs = LoadoutBodyPart[3].GetComponent<BaseMechPartLegs>();
            BaseMechPartPack Pack = LoadoutBodyPart[4].GetComponent<BaseMechPartPack>();
            BaseBoostSystem Boost = LoadoutBodyPart[5].GetComponent<BaseBoostSystem>();
            BasePowerSystem Power = LoadoutBodyPart[6].GetComponent<BasePowerSystem>();

            MyMech.AssignParts(Head, Torso, LoadoutBodyPart[2], Legs, Pack, Boost, Power);

            BaseMainSlotEquipment CP;
            BaseMainSlotEquipment CS;

            if (LoadoutMainEquipment[0])
                CP = LoadoutMainEquipment[0].GetComponent<BaseMainSlotEquipment>();
            else
                CP = null;

            if (LoadoutMainEquipment[1])
                CS = LoadoutMainEquipment[1].GetComponent<BaseMainSlotEquipment>();
            else
                CS = null;

            MyMech.GetFCS().RecieveMainEquipment(CP, CS);

            //loaded null EXG will revert to default loadout, it's ok if the default loadout is empty in that slot

            List<BaseEXGear> EXGs = new List<BaseEXGear>();
            for (int i = 0; i < 6; i++)
            {
                if (LoadoutEXGs[i])
                    EXGs.Add(LoadoutEXGs[i].GetComponent<BaseEXGear>());
                else
                    EXGs.Add(null);
            }

            MyMech.GetFCS().RecieveEXGs(EXGs);

            Debug.Log("Loadout loaded");
        }

        MyMech.InitializeMech();





    }

}
