using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutOption : MonoBehaviour
{
    private LoadoutSaveLoader MyManager;
    [SerializeField]
    private UnityEngine.UI.Text LoadoutName;
    [SerializeField]
    private GameObject DefaultDisplayParent;
    [SerializeField]
    private GameObject ConfirmDisplayParent;
    [SerializeField]
    private UnityEngine.UI.Text ConfirmText;

    ConfirmState CurrentState = ConfirmState.None;
    enum ConfirmState
    {
        None,
        OverwiteSave,
        OverwiteLoad,
        Delete,
    }

    int SlotNum;

    #region Button Functions
    public void Save()
    {
        CurrentState = ConfirmState.OverwiteSave;
        DefaultDisplayParent.SetActive(false);
        ConfirmDisplayParent.SetActive(true);
        ConfirmText.text = "Overite saved\nloadout?";
    }

    public void Load()
    {
        CurrentState = ConfirmState.OverwiteLoad;
        DefaultDisplayParent.SetActive(false);
        ConfirmDisplayParent.SetActive(true);
        ConfirmText.text = "Overite Current\nloadout?";
    }

    public void Delete()
    {
        CurrentState = ConfirmState.Delete;
        DefaultDisplayParent.SetActive(false);
        ConfirmDisplayParent.SetActive(true);
        ConfirmText.text = "Delete saved\nloadout?";
    }

    public void Confirm()
    {
        switch (CurrentState)
        {
            case ConfirmState.Delete:
                DeleteOption();
                break;
            case ConfirmState.OverwiteSave:
                OverwriteSave();
                break;
            case ConfirmState.OverwiteLoad:
                LoadSaved();
                break;
        }
        Cancel(); //sets button back to default state
    }

    public void Cancel()
    {
        CurrentState = ConfirmState.None;
        DefaultDisplayParent.SetActive(true);
        ConfirmDisplayParent.SetActive(false);
    }
    #endregion

    public void CreateOption(LoadoutSaveLoader Manager, string Name,int Slot)
    {
        MyManager = Manager;
        SlotNum = Slot;
        LoadoutName.text = Name;

    }

    public void UpdateSlotNum(int a)
    {
        SlotNum = a;
    }

    private void OverwriteSave()
    {
        MyManager.OverwriteSave(SlotNum);
    }

    private void LoadSaved()
    {
        MyManager.OverwriteLoad(SlotNum);
    }

    private void DeleteOption()
    {
        MyManager.Delete(SlotNum);
    }
}
