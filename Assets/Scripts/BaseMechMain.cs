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
    protected EnergySignal MyES;

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
    LoadOutPart MPArms;
    BaseMechPartArm MPLArm;
    BaseMechPartArm MPRArm;

    [SerializeField]
    BaseMechPartLegs MPLegs;
    [SerializeField]
    BaseMechPartPack MPPack;

    private List<BaseMechPart> AllParts;
    [Space(10)]
    [SerializeField]
    BaseFCSChip FCSChip;
    [SerializeField]
    BaseBoostSystem BoostSystem;




    [Space(20)]


    public bool PlayerMech = false;
    [SerializeField]
    protected Camera MinimapCam;


    protected BaseEnergySource EnergySystem;
    public AttributeManager.AttributeProfile MyAttProfile { get; protected set; } = new AttributeManager.AttributeProfile();

    //{ get; protected set; }


    protected override void Start()
    {
        if (!GetComponent<MechLoader>())
            InitializeMech();
    }

    public virtual void InitializeMech()
    {
        PlayerMech = (GetComponent<PlayerController>() != null);

        MyFCS.RecieveBMM(this);
        MyMovement.RecieveBMM(this);

        SpawnParts();
        BuildMech();

        //the attribute adding process has been moved to each of the mechpart's assemble function
        //List<AttributeManager.BaseMechAttribute> MechAtts = new List<AttributeManager.BaseMechAttribute>();
        //List<AttributeManager.ExtraAttribute> ExtraAtts = new List<AttributeManager.ExtraAttribute>();
        //foreach (BaseMechPart a in AllParts)
        //{
        //    MechAtts.AddRange(a.Attributes);
        //}
        //ApplyMechAttributes(MechAtts);
        //ApplyExtraAttributes(ExtraAtts);



        


        MyMovement = GetComponent<BaseMechMovement>();
        AssignMovementStats();
        MyMovement.InitializeMechMovement(this, PlayerMech);

        MyFCS = GetComponent<BaseMechFCS>();
        MyFCS.InitializeFCS(this, PlayerMech, MPLArm, MPRArm, FCSChip);

        AssignWeight();
        EnergySystem = MPTorso.GetPowerSystem();

        InitializeIDamageable();

        GetComponent<MechColorAdjuster>().switchColor();

        if (PlayerMech)
        {
            FindObjectOfType<UIInfoPanelManager>().UIInitialize();
            if(UILockManager.Instance!=null)
            UILockManager.Instance.Initialize();
            if(MainLockManagerMK2.Instance!=null)
            MainLockManagerMK2.Instance.Initialize();

            MinimapCam.orthographicSize = FCSChip.GetRadarRange;
        }
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
        if (!MyFCS)
            MyFCS = GetComponent<BaseMechFCS>();
        return MyFCS;
    }

    public BaseMechMovement GetMovement()
    {
        if (!MyMovement)
            MyMovement = GetComponent<BaseMechMovement>();
        return MyMovement;
    }

    public BaseEnergySource GetEnergySystem()
    {
        if (!EnergySystem)
            EnergySystem = MPTorso.GetPowerSystem();
        return EnergySystem;
    }

    public EnergySignal GetEnergySignal()
    {
         return MyES; 
    }


    public void DecreaseWeight(float DroppedWeight)
    {
        MyMovement.ChangeWeight(-DroppedWeight);
    }

    protected override void Destroied()
    {
        base.Destroied();
        if (PlayerMech)
            PauseMiniMenu.Instance.ShowLevelEndUI(false);
    }


    #region Mech loading related

    public void AssignParts(BaseMechPartHead Head, BaseMechPartTorso Torso, LoadOutPart Arms, BaseMechPartLegs Legs, BaseMechPartPack Pack, BaseBoostSystem Boost, BaseFCSChip _FCSChip)
    {
        MPHead = Head;
        MPTorso = Torso;


        MPArms = Arms;
        MPLArm = Arms.GetComponentInChildren<BaseMechPartLArm>();
        MPRArm = Arms.GetComponentInChildren<BaseMechPartRArm>();

        MPLegs = Legs;
        MPPack = Pack;
        BoostSystem = Boost;

        FCSChip = _FCSChip;
    }

    #endregion

    #region Mech Assembly related
    private void BuildMech()
    {


        if (MPTorso == null)
            return;
        MPTorso.Assemble(this, transform);
        MPTorso.AssembleMech(this, MPHead, MPRArm, MPLArm, MPLegs, MPPack);


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
        BoostSystem.CreateBoostAndJumpEffects(BoostPoints, FloatThrustPoints);
        BoostSystem.InitBS(this);

        MyMovement.SetGroundDetection(MPLegs.GetGroundDetection());

        //MyFCS.InitStats(300, 150); // radar and lock range now hard coded to test integration with assembly, if you try to integrate range status without changing this, you're a dumbass future jiening.

        LeftArm = MPLArm.transform;
        RightArm = MPRArm.transform;


    }

    private void SpawnParts()
    {
        MPTorso = Instantiate(MPTorso.gameObject, transform).GetComponent<BaseMechPartTorso>();
        MPHead = Instantiate(MPHead.gameObject, transform).GetComponent<BaseMechPartHead>();
        MPLegs = Instantiate(MPLegs.gameObject, transform).GetComponent<BaseMechPartLegs>();
        MPPack = Instantiate(MPPack.gameObject, transform).GetComponent<BaseMechPartPack>();

        GameObject TempArms = Instantiate(MPArms.gameObject, transform);

        MPLArm = TempArms.GetComponentInChildren<BaseMechPartLArm>();
        MPRArm = TempArms.GetComponentInChildren<BaseMechPartRArm>();

        BoostSystem = Instantiate(BoostSystem.gameObject, transform).GetComponent<BaseBoostSystem>();
        //EnergySystem = Instantiate(EnergySystem.gameObject, transform).GetComponent<BasePowerSystem>();
    }

    private void AssignMovementStats()
    {
        float MF = MPLegs.MovementForce;
        float SL = MPLegs.SpeedCap;
        float JF = MPLegs.JumpForce;

        BoostSystem.OutStats(out float BF, out float IBF, out float FF, out float ESC, out float BC, out float IBC, out float FC, out float BJC, out float BJR, out float BJRC, out BaseBoostSystem BS);

        MyMovement.SetStats(MF, SL, ESC, BF, IBF, FF, BC, IBC, FC, BJC, BJR, BJRC, MPLegs.GetDrag(), JF, BS);
    }

    private void AssignWeight()
    {
        float Weight = 5; //base weight


        foreach (BaseMechPart a in AllParts)
        {
            Weight += a.GetWeight(true);
        }

        MyMovement.SetWeight(Weight);
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
        if (EXGs[3])
            EXGs[3].InitializeGear(this, MPTorso.transform, false);
        EXGs[4] = MPPack.GetBuilInEXG();
        if (EXGs[4])
            EXGs[4].InitializeGear(this, MPPack.transform, false);

        EXGs[5] = MPPack.AttemptEquipEXGAndGet(EXGs[5], true);
        EXGs[6] = MPRArm.AttemptEquipEXGAndGet(EXGs[6]);
        EXGs[7] = MPLegs.AttemptEquipEXGAndGet(EXGs[7], true);

    }

    public void ApplyMechAttributes(List<AttributeManager.BaseMechAttribute> Attributes)
    {
        foreach (AttributeManager.BaseMechAttribute A in Attributes)
        {
            MyAttProfile.ApplyBaseMechAttribute(A);
        }
    }

    public void ApplyExtraAttributes(List<AttributeManager.ExtraAttribute> EAttributes)
    {
        foreach (AttributeManager.ExtraAttribute A in EAttributes)
        {
            MyAttProfile.ApplyExtraAttribute(A);
        }
    }









    //public void GetHands(out Transform Left, out Transform Right) // legacy function
    //{
    //    Left = MPLArm.GetHandSlot() ;
    //    Right = MPRArm.GetHandSlot();
    //}

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
