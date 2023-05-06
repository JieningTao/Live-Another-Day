using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    BaseMechMain MyMech;
    BaseMechMovement MyMovement;
    BaseMechFCS MyFCS;

    //[SerializeField]
    //bool Testing;

    private void Start()
    {
        MyMech = GetComponent<BaseMechMain>();
        MyFCS = MyMech.GetFCS();
        MyMovement = MyMech.GetMovement();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        if (Time.timeScale > 0) // controls need to not work when paused
        {
            HandleMovementInput();
            HandleWeaponInput();
            HandleEXGearInput();
        }
        HandlePauseInput();

#if (UNITY_EDITOR)
        HandleDebugInput();
#else

#endif
    }

    private void HandleMovementInput()
    {

        Vector3 TempInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        TempInput.y = Input.GetAxisRaw("UpDown");

        MyMovement.MovementInput = TempInput;

        if(Input.GetButtonDown("Boost"))
            MyMovement.BoostControl(true);
        else if(Input.GetButtonUp("Boost"))
            MyMovement.BoostControl(false);

        MyMech.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0) * Time.deltaTime);
    }

    private void HandleWeaponInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Input.GetButton("AltFire"))
                MyFCS.FirePrimary2(true);
            else
                MyFCS.FirePrimary1(true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            MyFCS.FirePrimary1(false);
            MyFCS.FirePrimary2(false);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Input.GetButton("AltFire"))
                MyFCS.FireSecondary2(true);
            else
                MyFCS.FireSecondary1(true);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            MyFCS.FireSecondary1(false);
            MyFCS.FireSecondary2(false);
        }










        //if (Input.GetButtonDown("AltFire"))
        //{
        //    MyFCS.FirePrimary1(false);
        //    MyFCS.FireSecondary1(false);
        //}

        //if (Input.GetButton("AltFire"))
        //{
        //    if (Input.GetButtonDown("Fire1"))
        //        MyFCS.FirePrimary2(true);
        //    else if (Input.GetButtonUp("Fire1"))
        //        MyFCS.FirePrimary2(false);

        //    if (Input.GetButtonDown("Fire2"))
        //        MyFCS.FireSecondary2(true);
        //    else if (Input.GetButtonUp("Fire2"))
        //        MyFCS.FireSecondary2(false);
        //}
        //else
        //{
        //    if (Input.GetButtonDown("Fire1"))
        //        MyFCS.FirePrimary1(true);
        //    else if (Input.GetButtonUp("Fire1"))
        //        MyFCS.FirePrimary1(false);

        //    if (Input.GetButtonDown("Fire2"))
        //        MyFCS.FireSecondary1(true);
        //    else if (Input.GetButtonUp("Fire2"))
        //        MyFCS.FireSecondary1(false);
        //}

    }

    private void HandleEXGearInput()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetButtonDown("Switch EXGear Pos"))
            MyFCS.SwitchEXGear(false);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetButtonDown("Switch EXGear Neg"))
            MyFCS.SwitchEXGear(true);



        if (Input.GetButtonDown("Trigger EXGear"))
            MyFCS.TriggerEXGear(true);
        else if (Input.GetButtonUp("Trigger EXGear"))
            MyFCS.TriggerEXGear(false);
    }

    private void HandlePauseInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseMiniMenu.Instance.ToggleMenu();
        }
    }

    private void HandleDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        else if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.Mouse3))
            Debug.Break();
    }
}
