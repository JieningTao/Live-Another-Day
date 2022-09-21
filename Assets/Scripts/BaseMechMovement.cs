using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechMovement : MonoBehaviour
{

    [SerializeField]
    float MoveForce;

    [SerializeField]
    float SpeedLimit;
    [SerializeField]
    float BoostSpeedLimit;

    [SerializeField]
    float BoostForce;
    [SerializeField]
    float BoostCost;

    [SerializeField]
    float ImpulseBoostForce;
    [SerializeField]
    float ImpulseCost;

    [SerializeField]
    private float FloatForce;
    [SerializeField]
    float FloatCost;

    [SerializeField] //SFT (Serialized for testing)
    public bool Boosting;
    [SerializeField]
    protected float BoostJuiceCapacity;
    protected float CurrentBoostjuice;
    [SerializeField]
    protected float BoostJuiceRecovery;
    [SerializeField]
    protected float BoostJuiceRecoveryCooldown;
    protected float BoostRecoveryCooldownRemaining;


    private bool Floating
    {
        get { return MovementInput.y > 0; }
    }
    private bool WasFloating;

    [SerializeField]
    private float MovingDrag;
    [SerializeField]
    private float StoppingDrag;
    [SerializeField]
    private float OverSpeedDrag;

    [SerializeField]
    private float JumpForce;


    [SerializeField]
    private LayerMask GroundDetection;
    [SerializeField]
    private Transform GroundDetectionSite;
    [SerializeField]
    private float GroundDetectionRadius;

    private List<ParticleSystem> BoostExhausts;
    private List<ParticleSystem> BoostImpulses;
    private List<ParticleSystem> FloatThrusters;
    [SerializeField]
    private ParticleSystem JumpEffect;

    [SerializeField]
    private GameObject BoostEffectPrefab;
    [SerializeField]
    private GameObject ImpulseBoostPrefab;
    [SerializeField]
    private GameObject FloatThrustPrefab;


    float JumpCooldown = 0.2f;
    float JCRemaining;
    public Vector3 MovementInput;

    [SerializeField] //SFT
    private Rigidbody MyRB;
    private BaseMechMain MyBMM;

    
    //private void Start()
    //{
    //    MyRB = GetComponent<Rigidbody>();

    //    Cursor.lockState = CursorLockMode.Locked;
    //    JCRemaining = 0;
    //    CurrentBoostjuice = BoostJuiceCapacity;
    //}

    public void InitializeMechMovement(BaseMechMain BMM,bool Player)
    {
        MyBMM = BMM;
        MyRB = GetComponent<Rigidbody>();
        //Debug.Log(MyRB, this);
        Cursor.lockState = CursorLockMode.Locked;
        JCRemaining = 0;
        CurrentBoostjuice = BoostJuiceCapacity;
    }

    public void SetBoostEffects(GameObject Boost, GameObject Impulse,GameObject Float)
    {
        BoostEffectPrefab = Boost;
        ImpulseBoostPrefab = Impulse;
        FloatThrustPrefab = Float; //currently boost system will use the same effect as boost for float
    }

    public void SetWeight(float a)
    {
        MyRB.mass = a;
    }

    public void ChangeWeight(float a)
    {
        MyRB.mass += a;
    }

    public void CreateBoostAndJumpEffects(List<Transform> BoostPoints,List<Transform> FloatThrustPoints)
    {
        BoostExhausts = new List<ParticleSystem>();
        BoostImpulses = new List<ParticleSystem>();

        FloatThrusters = new List<ParticleSystem>();

        for (int i = 0; i < BoostPoints.Count; i++)
        {
            ParticleSystem Boost = Instantiate(BoostEffectPrefab, BoostPoints[i]).GetComponentInChildren<ParticleSystem>();
            AdjustEffectScale(Boost, BoostPoints[i].transform.localScale);
            BoostExhausts.Add(Boost);

            ParticleSystem Impulse = Instantiate(ImpulseBoostPrefab, BoostPoints[i]).GetComponentInChildren<ParticleSystem>();
            AdjustEffectScale(Impulse, BoostPoints[i].transform.localScale);
            BoostImpulses.Add(Impulse);
        }
        for (int i = 0; i < FloatThrustPoints.Count; i++)
        {
            ParticleSystem Float = Instantiate(FloatThrustPrefab, FloatThrustPoints[i]).GetComponentInChildren<ParticleSystem>();
            AdjustEffectScale(Float, FloatThrustPoints[i].transform.localScale);
            FloatThrusters.Add(Float);
        }
    }

    public void AdjustEffectScale(ParticleSystem a,Vector3 Scale)
    {
        foreach (ParticleSystem b in a.GetComponentsInChildren<ParticleSystem>())
        {
            b.transform.localScale = Scale;
        }
    }

    public void SetStats(float MF,float SL, float BSL, float BF,float IBF, float FF,float BC,float IC,float FC,float BJC, float BJR,float BJRC ,Vector3 Drag, float JF)
    {
        MoveForce = MF;
        SpeedLimit = SL;
        BoostSpeedLimit = SL + BSL;

        BoostForce = BF;
        ImpulseBoostForce = IBF;
        FloatForce = FF;

        BoostCost = BC;
        ImpulseCost = IC;
        FloatCost = FC;

        BoostJuiceCapacity = BJC;
        BoostJuiceRecovery = BJR;
        BoostJuiceRecoveryCooldown = BJRC;

        MovingDrag = Drag.x;
        StoppingDrag = Drag.y;
        OverSpeedDrag = Drag.z;

        JumpForce = JF;
    }

    public void SetGroundDetection(Transform Site)
    {
        GroundDetectionSite = Site;
        JumpEffect.transform.position = Site.position;
    }

    private void FixedUpdate()
    {
        //force added with planar movement and maybe verticle movement seems inconsistant with framerate, putting them in fixed update seems to help with this
        DragChange();
        PlanarMovement();
        VerticleMovement();
        RecoverBoost();



        //Debug.Log("Boost Juice: " + CurrentBoostjuice);

        if (JCRemaining > 0)
            JCRemaining -= Time.deltaTime;

    }

    private void PlanarMovement()
    {
        Vector3 PlanarInput = MovementInput;

        PlanarInput.y = 0;

        if (PlanarInput == Vector3.zero)
        {
            if (Boosting)
                BoostControl(false);
            return;
        }

        float AirMultiplier = 1; //mid-air manuvering is less effective unless boosting, under test
        if (!grounded())
            AirMultiplier = 0.2f;
        else if (!grounded() && Boosting)
            AirMultiplier = 0.7f;


        //addforce forcemode.force should be independent and doesnot require time.deltatime, this combined with fixedupdate should help with inconsistancies with framerate
        if (Boosting && AttemptUseBoostJuice(BoostCost * Time.deltaTime))
        {
            MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * (MoveForce * AirMultiplier + BoostForce), ForceMode.Force);
        }
        else
        {
            if (Boosting)
                BoostControl(false);
            


            MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * MoveForce * AirMultiplier, ForceMode.Force);
        }



        //if (!WasBoosting && ShouldBeBoosting)
        //{
        //    BoostEffect(true);

        //    if (AttemptUseBoostJuice(ImpulseCost))
        //    {
        //        ImpulseBoostEffect();
        //        MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * ImpulseBoostForce, ForceMode.Impulse);
        //    }
        //}
        //else if (!ShouldBeBoosting && WasBoosting)
        //    BoostEffect(false);

        //WasBoosting = ShouldBeBoosting;
    }

    public void BoostControl(bool Start)
    {
        if (Start)
        {
            Vector3 PlanarInput = MovementInput;

            PlanarInput.y = 0;

            if (PlanarInput == Vector3.zero)
                return;
            

            if (AttemptUseBoostJuice(ImpulseCost))
            {
                ImpulseBoostEffect();
                MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * ImpulseBoostForce, ForceMode.Impulse);
            }

            if (CurrentBoostjuice >= BoostCost * Time.deltaTime)
            {
                BoostEffect(true);
                Boosting = true;
            }
        }
        else
        {
            if (Boosting)
            {
                Boosting = false;
                BoostEffect(false);
            }
        }


    }

    #region Boost juice related stuff
    private void RecoverBoost()
    {
        if (BoostRecoveryCooldownRemaining > 0)
            BoostRecoveryCooldownRemaining -= Time.deltaTime;
        else
        {
            if (CurrentBoostjuice < BoostJuiceCapacity && !Boosting)
            {
                CurrentBoostjuice += BoostJuiceRecovery * Time.deltaTime;
                Mathf.Clamp(CurrentBoostjuice, 0, BoostJuiceCapacity);
            }
        }
    }

    private bool AttemptUseBoostJuice(float Amount)
    {
        if (CurrentBoostjuice >= Amount)
        {
            CurrentBoostjuice -= Amount;
            BoostRecoveryCooldownRemaining = BoostJuiceRecoveryCooldown;
            return true;
        }
        else
            return false;

    }

    public void RestoreBoostJuice(float Amount)
    {
        CurrentBoostjuice += Amount;
        if (CurrentBoostjuice > BoostJuiceCapacity)
            CurrentBoostjuice = BoostJuiceCapacity;
    }

    public float GetBoostJuicePercentage()
    {
        return CurrentBoostjuice / BoostJuiceCapacity;
    }
    #endregion

    private void DragChange()
    {
        if (MovementInput == Vector3.zero && grounded())
            MyRB.drag = StoppingDrag;
        else
            MyRB.drag = MovingDrag;

        if (Boosting)
        {
            if (MyRB.velocity.magnitude > BoostSpeedLimit)
            {
                MyRB.drag = OverSpeedDrag;
            }
        }
        else
        {
            if (MyRB.velocity.magnitude > SpeedLimit)
            {
                MyRB.drag = OverSpeedDrag;
            }
        }
    }

    public bool grounded()
    {
        if (Physics.OverlapSphere(GroundDetectionSite.position, GroundDetectionRadius, GroundDetection).Length > 0)
            return true;
        else
            return false;
    }

    public void Jump()
    {
        JCRemaining = JumpCooldown;
        //Debug.Log(transform.up * JumpForce);
        MyRB.AddForce(transform.up * JumpForce, ForceMode.Impulse);

        JumpEffect.Play();
    }

    private void VerticleMovement()
    {
        if (Floating)
        {
            if (AttemptUseBoostJuice(BoostCost * Time.deltaTime))
                MyRB.AddForce(transform.up * FloatForce, ForceMode.Force);
        }

        if (Floating && !WasFloating)
        {
            if (grounded() && JCRemaining <= 0)
                Jump();

            FloatEffect(true);
        }
        else if (!Floating && WasFloating)
            FloatEffect(false);

        WasFloating = Floating;
    }



    private void OnDisable()
    {
        BoostEffect(false);
        FloatEffect(false);
    }

    #region Effects

    private void ImpulseBoostEffect()
    {
        foreach (ParticleSystem a in BoostImpulses)
        {
            a.Play();
        }
    }

    private void BoostEffect(bool boost)
    {
        foreach (ParticleSystem a in BoostExhausts)
        {
            if (boost)
                a.Play();
            else
                a.Stop();
        }
    }

    private void FloatEffect(bool _float)
    {
        foreach (ParticleSystem a in FloatThrusters)
        {
            if (_float)
                a.Play();
            else
                a.Stop();
        }
    }

    #endregion

    #region Display related
    public string GetBoostText()
    {
        return CurrentBoostjuice.ToString("F2");
    }

    public float GetBoostPercent()
    {
        return (float)CurrentBoostjuice / (float)BoostJuiceCapacity;
    }

    public string GetSpeedText()
    {
        return MyRB.velocity.magnitude.ToString("F2");
    }

    public float GetSpeedPercent()
    {
        return (float)MyRB.velocity.magnitude / ((float)BoostSpeedLimit*1.2f);
    }

    public float GetNormSpeedLimitRatio()
    {
        return (float)SpeedLimit / BoostSpeedLimit;
    }
    #endregion
}
