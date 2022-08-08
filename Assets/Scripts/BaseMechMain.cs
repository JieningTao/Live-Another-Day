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

        SpawnParts();
        BuildMech();


        MyFCS = GetComponent<BaseMechFCS>();
        MyFCS.InitializeFCS(this,PlayerMech,MPLArm,MPRArm);


        MyMovement = GetComponent<BaseMechMovement>();
        MyMovement.InitializeMechMovement(this,PlayerMech);




        InitializeIDamageable();

        FindObjectOfType<UIInfoPanelManager>().UIInitialize();
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

    private void SpawnParts()
    {
        MPTorso = Instantiate(MPTorso.gameObject, transform).GetComponent<BaseMechPartTorso>();
        MPHead = Instantiate(MPHead.gameObject, transform).GetComponent<BaseMechPartHead>();
        MPLegs = Instantiate(MPLegs.gameObject, transform).GetComponent<BaseMechPartLegs>();
        MPPack = Instantiate(MPPack.gameObject, transform).GetComponent<BaseMechPartPack>();
        MPLArm = Instantiate(MPLArm.gameObject, transform).GetComponent<BaseMechPartArm>();
        MPRArm = Instantiate(MPRArm.gameObject, transform).GetComponent<BaseMechPartArm>();

        BoostSystem = Instantiate(BoostSystem.gameObject, transform).GetComponent<BaseBoostSystem>();
        EnergySystem = Instantiate(EnergySystem.gameObject, transform).GetComponent<BasePowerSystem>();
    }

    private void AssignMovementStats()
    {
        float MF = MPLegs.MovementForce;
        float SL = MPLegs.SpeedCap;
        float JF = MPLegs.JumpForce;

        BoostSystem.OutStats(out float BF, out float IBF, out float FF, out float ESC, out float BC, out float IBC, out float FC, out float BJC, out float BJR, out float BJRC);

        MyMovement.SetStats(MF, SL, ESC, BF, IBF, FF, BC, IBC, FC, BJC, BJR, BJRC, MPLegs.GetDrag(), JF);
    }

    public void EXGInstall(ref BaseEXGear[] EXGs)
    {
        //MPLegs.EquipLegEXGs(EXGs[0], EXGs[7]);
        //MPLArm.EquipEXG(EXGs[1]);
        //MPRArm.EquipEXG(EXGs[6]);
        //MPTorso.EquipShoulderEXGs(EXGs[2], EXGs[5]);


        EXGs[0] = MPLegs.AttemptEquipEXGAndGet(EXGs[0], false);
        EXGs[1] = MPLArm.AttemptEquipEXGAndGet(EXGs[1]);
        EXGs[2] = MPPack.AttemptEquipEXGAndGet(EXGs[2], false);
        EXGs[3] = MPTorso.GetBuilInEXG();
        EXGs[4] = MPPack.GetBuilInEXG();
        EXGs[5] = MPPack.AttemptEquipEXGAndGet(EXGs[5], true);
        EXGs[6] = MPRArm.AttemptEquipEXGAndGet(EXGs[6]);
        EXGs[7] = MPLegs.AttemptEquipEXGAndGet(EXGs[7], true);

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
