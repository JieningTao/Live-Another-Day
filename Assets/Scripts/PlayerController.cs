using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    BaseMechMovement MyMech;
    BaseMechFCS MyFCS;
   

    private void Start()
    {
        MyMech = GetComponent<BaseMechMovement>();
        MyFCS = GetComponent<BaseMechFCS>();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleWeaponInput();
    }

    private void HandleMovementInput()
    {

        Vector3 TempInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        TempInput.y = Input.GetAxisRaw("UpDown");

        MyMech.MovementInput = TempInput;

        MyMech.Boosting = Input.GetButton("Boost");

        MyMech.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0) * Time.deltaTime);
    }

    private void HandleWeaponInput()
    {
        if (Input.GetButtonDown("AltFire"))
        {
            MyFCS.FirePrimary1(false);
            MyFCS.FireSecondary1(false);
        }

        if (Input.GetButton("AltFire"))
        {
            if (Input.GetButtonDown("Fire1"))
                MyFCS.FirePrimary2(true);
            else if (Input.GetButtonUp("Fire1"))
                MyFCS.FirePrimary2(false);

            if (Input.GetButtonDown("Fire2"))
                MyFCS.FireSecondary2(true);
            else if (Input.GetButtonUp("Fire2"))
                MyFCS.FireSecondary2(false);
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
                MyFCS.FirePrimary1(true);
            else if (Input.GetButtonUp("Fire1"))
                MyFCS.FirePrimary1(false);

            if (Input.GetButtonDown("Fire2"))
                MyFCS.FireSecondary1(true);
            else if (Input.GetButtonUp("Fire2"))
                MyFCS.FireSecondary1(false);
        }

    }
}
