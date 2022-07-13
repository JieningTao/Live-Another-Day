using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechMain : ICoatedDamagable
{
    [SerializeField]
    protected BaseMechMovement MyMovement;
    [SerializeField]
    protected BaseMechFCS MyFCS;

    [SerializeField]
    public Transform CameraAnchor;
    [SerializeField]
    Transform RightArm;
    [SerializeField]
    Transform LeftArm;
    [SerializeField]
    float TurnSpeed;
    [SerializeField]
    Vector2 CameraTopDownLimit = new Vector2(-80, 80);

    [Space(20)]

    [SerializeField]
    BaseMechPartHead MPHead;
    [SerializeField]
    BaseMechPartTorso MPTorso;
    [SerializeField]
    BaseMechPartArm MPLArm;
    [SerializeField]
    BaseMechPartArm MPRArm;
    [SerializeField]
    BaseMechPartLegs MPLegs;
    [SerializeField]
    BaseMechPartPack MPPack;

    private List<BaseMechPart> AllParts;
    [Space(10)]

    [SerializeField]
    BaseBoostSystem BoostSystem;
    [SerializeField]
    BaseEnergySource EnergySystem;

    [Space(20)]


    protected bool PlayerMech = false;

    //{ get; protected set; }


    protected override void Start()
    {

        PlayerMech = (GetComponent<PlayerController>() != null);

        Debug.Log(PlayerMech);

        MyFCS = GetComponent<BaseMechFCS>();
        MyFCS.InitializeFCS(this,PlayerMech,MPLArm,MPRArm);

        MyMovement = GetComponent<BaseMechMovement>();
        MyMovement.InitializeMechMovement(this,PlayerMech);



        BuildMech();

        InitializeIDamageable();
    }

    

    public void Rotate(Vector3 Rot)
    {
        transform.Rotate((new Vector3(0, Rot.y, 0)) * TurnSpeed);

        Vector3 VerticalRotation = (new Vector3(Rot.x, 0, 0)) * TurnSpeed;

        CameraAnchor.transform.Rotate(VerticalRotation);

        Vector3 Temp = CameraAnchor.transform.localEulerAngles;

        //Debug.Log(Temp);

        if (Temp.x < 180)
            Temp.x = Mathf.Clamp(Temp.x, CameraTopDownLimit.x, CameraTopDownLimit.y);
        else
            Temp.x = Mathf.Clamp(Temp.x, 360 + CameraTopDownLimit.x, 360 + CameraTopDownLimit.y);

        Temp.y = 0;
        Temp.z = 0;

        CameraAnchor.localEulerAngles = Temp;

        RightArm.transform.rotation = CameraAnchor.rotation;
        LeftArm.transform.rotation = CameraAnchor.rotation;
    }

    public BaseMechFCS GetFCS()
    {
        return MyFCS;
    }

    public BaseMechMovement GetMovement()
    {
        return MyMovement;
    }

    public BaseEnergySource GetEnergySystem()
    {
        return EnergySystem;
    }




    #region Mech Assembly related
    private void BuildMech()
    {
        if (MPTorso == null)
            return;
        MPTorso.Assemble(this, transform);
        MPTorso.AssembleMech(this, MPHead,MPRArm,MPLArm,MPLegs,MPPack);

        AllParts = new List<BaseMechPart>();
        AllParts.Add(MPTorso);
        AllParts.Add(MPLArm);
        AllParts.Add(MPRArm);
        AllParts.Add(MPLegs);
        AllParts.Add(MPHead);
        AllParts.Add(MPPack);

        MaxHealth = 0;
        float Weight = 0;
        List<Transform> BoostPoints = new List<Transform>();
        List<Transform> FloatThrustPoints = new List<Transform>();

        foreach (BaseMechPart a in AllParts)
        {
            if (a)
            {
                MaxHealth += a.Health;
                Weight += a.Weight;

                BoostPoints.AddRange(a.BoostThrusters);
                FloatThrustPoints.AddRange(a.FloatThrusters);

                a.gameObject.layer = gameObject.layer;
            }
        }

        BoostSystem.transform.parent = transform;

        MyMovement.SetBoostEffects(BoostSystem.GetBoostExhaust(), BoostSystem.GetImpulseBoost(),BoostSystem.GetFloatThrust());
        MyMovement.CreateBoostAndJumpEffects(BoostPoints, FloatThrustPoints);
        MyMovement.SetGroundDetection(MPLegs.GetGroundDetection());

        MyFCS.InitStats(300, 150); // radar and lock range now hard coded to test integration with assembly, if you try to integrate range status without changing this, you're a dumbass future jiening.

        LeftArm = MPLArm.transform;
        RightArm = MPRArm.transform;

        AssignMovementStats();
    }

    private void AssignMovementStats()
    {
        float MF = MPLegs.MovementForce;
        float SL = MPLegs.SpeedCap;
        float JF = MPLegs.JumpForce;

        BoostSystem.OutStats(out float BF, out float IBF, out float FF, out float BC, out float IBC, out float FC, out float BJC, out float BJR, out float BJRC);

        MyMovement.SetStats(MF, SL, BF, IBF, FF, BC, IBC, FC, BJC, BJR, BJRC, MPLegs.GetDrag(), JF);
    }

    public void EXGInstall(ref BaseEXGear[] EXGs)
    {
        //MPLegs.EquipLegEXGs(EXGs[0], EXGs[7]);
        //MPLArm.EquipEXG(EXGs[1]);
        //MPRArm.EquipEXG(EXGs[6]);
        //MPTorso.EquipShoulderEXGs(EXGs[2], EXGs[5]);

        if (EXGs[0])
            EXGs[0].InitializeGear(this, MPLegs.GetLeftEXGSlot(), false);
        if (EXGs[1])
            EXGs[1].InitializeGear(this, MPLArm.GetSideMountEXGSlot(), false);
        if (EXGs[2])
            EXGs[2].InitializeGear(this, MPTorso.GetLeftShoulderEXGSlot(), false);

            EXGs[3] = MPTorso.GetBuilInEXG();
            EXGs[4] = MPPack.GetBuilInEXG();

        if (EXGs[5])
            EXGs[5].InitializeGear(this, MPTorso.GetRightShoulderEXGSlot(), true);
        if (EXGs[6])
            EXGs[6].InitializeGear(this, MPRArm.GetSideMountEXGSlot(), true);
        if (EXGs[7])
            EXGs[7].InitializeGear(this, MPLegs.GetRightEXGSlot(), true);
    }

    public void GetHands(out Transform Left, out Transform Right)
    {
        Left = MPLArm.GetHandSlot() ;
        Right = MPRArm.GetHandSlot();
    }

    //public void GetLegEXGS(out Transform Left, out Transform Right)
    //{
    //    Left = MPLegs.GetLeftEXGSlot();
    //    Right = MPLegs.GetRightEXGSlot();
    //}

    //public void GetShoulderEXGS(out Transform Left, out Transform Right)
    //{
    //    Left = MPTorso.GetLeftShoulderEXGSlot();
    //    Right = MPTorso.GetRightShoulderEXGSlot();
    //}

    //public void GetArmEXGS(out Transform Left, out Transform Right)
    //{
    //    Left = MPLArm.GetSideMountEXGSlot();
    //    Right = MPRArm.GetSideMountEXGSlot();
    //}

    //public void GetBuiltInEXGS(out BaseEXGear Torso, out BaseEXGear BackPack)
    //{
    //    Torso = MPTorso.GetBuilInEXG();
    //    BackPack = MPPack.GetBuilInEXG();
    //}

    #endregion
}
