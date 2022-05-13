using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : MonoBehaviour
{
    [SerializeField]
    protected float TurnSpeed;

    [SerializeField]
    protected Transform TurretBase;
    [SerializeField]
    protected Transform TurretHead;
    [SerializeField]
    protected Transform RestAim;

    [SerializeField]
    public GameObject Target;
    public EnergySignal TargetSignal;


    protected Quaternion TurretBaseRotation;
    protected Quaternion TurretHeadRotation;


    private void Start()
    {
        TurretBaseRotation = TurretBase.localRotation;
        TurretHeadRotation = TurretHead.localRotation;
    }

    protected void Update()
    {
        if (Target)
            TurnToTarget(Target.transform.position);
        else
            Target = RestAim.gameObject;


    }



    protected virtual void TurnToTarget(Vector3 TargetPosition)
    {
        if (Vector3.Angle(TurretHead.forward, TargetPosition - TurretHead.position) == 0)
            return;

        Vector3 BaseDir = Vector3.RotateTowards(TurretBase.forward, TargetPosition - TurretBase.transform.position, TurnSpeed * Time.deltaTime, 0.0f);

        TurretBaseRotation = Quaternion.LookRotation(BaseDir, this.transform.up);
        TurretBase.rotation = TurretBaseRotation;
        //the following portion removes all rotation excpt Y axis
        TurretBaseRotation = TurretBase.localRotation;
        TurretBaseRotation.x = 0;
        TurretBaseRotation.z = 0;
        TurretBase.localRotation = TurretBaseRotation;




        Vector3 HeadDir = Vector3.RotateTowards(TurretHead.forward, TargetPosition - TurretHead.position, TurnSpeed * Time.deltaTime, 0.0f);

        TurretHeadRotation = Quaternion.LookRotation(HeadDir, this.transform.up);
        TurretHead.rotation = TurretHeadRotation;
        //need to do the same for turret head or it will rotate y on top of base y rotation
        TurretHeadRotation = TurretHead.localRotation;
        TurretHeadRotation.y = 0;
        TurretHeadRotation.z = 0;
        TurretHead.localRotation = TurretHeadRotation;
    }

    public float GetTargetAngleDeviation()
    {
        return Vector3.Angle(TurretHead.forward, Target.transform.position - TurretHead.position);
    }

    public void TurnToRest()
    {
        Target = RestAim.gameObject;
    }

    public bool IsResting()
    {
        return Target == RestAim.gameObject;
    }
}
