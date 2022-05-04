using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIDisplay : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text SpeedText;
    [SerializeField]
    Rigidbody PlayerRigidBody;

    [SerializeField]
    UnityEngine.UI.Text JuiceText;
    [SerializeField]
    BaseMechMovement PlayerMechMovement;



    private void Update()
    {
        SpeedText.text = "Speed: \n" + PlayerRigidBody.velocity.magnitude;
        JuiceText.text = "Juice: \n" + PlayerMechMovement.GetBoostJuicePercentage() * 100;
    }
}
