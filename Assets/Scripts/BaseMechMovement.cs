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

    [SerializeField]
    protected AudioSource BoostPlayer;
    [SerializeField]
    protected AudioSource ImpulsePlayer;


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
    RaycastHit GroundUnder;


    [SerializeField]
    private ParticleSystem JumpEffect;

    //boost effects are now being handled by the base boost system script
    //[SerializeField]
    //private GameObject BoostEffectPrefab;
    //[SerializeField]
    //private GameObject ImpulseBoostPrefab;
    //[SerializeField]
    //private GameObject FloatThrustPrefab;


    float JumpCooldown = 0.2f;
    float JCRemaining;
    public Vector3 MovementInput;

    [SerializeField] //SFT
    private Rigidbody MyRB;
    private BaseMechMain MyBMM;
    private BaseBoostSystem MyBS;

    
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

    //public void SetBoostEffects(GameObject Boost, GameObject Impulse,GameObject Float)
    //{
    //    BoostEffectPrefab = Boost;
    //    ImpulseBoostPrefab = Impulse;
    //    FloatThrustPrefab = Float;
    //}

    public void SetWeight(float a)
    {
        MyRB.mass = a;
    }

    public void ChangeWeight(float a)
    {
        MyRB.mass += a;
    }

    



    public void SetStats(float MF,float SL, float BSL, float BF,float IBF, float FF,float BC,float IC,float FC,float BJC, float BJR,float BJRC ,Vector3 Drag, float JF, BaseBoostSystem BS)
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

        MyBS = BS;
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

        PlanarInput = transform.TransformDirection(PlanarInput).normalized;

        if (grounded())
        {
            //converts planar movement to sloped plane under player 
            PlanarInput = Vector3.ProjectOnPlane(PlanarInput, GroundUnder.normal).normalized;
            Debug.DrawRay(transform.position, PlanarInput * 100,Color.cyan);
        }
        else
        {
            if (Boosting)
                AirMultiplier = 0.7f;
            else
                AirMultiplier = 0.2f;
        }


        //addforce forcemode.force should be independent and doesnot require time.deltatime, this combined with fixedupdate should help with inconsistancies with framerate
        if (Boosting && AttemptUseBoostJuice(BoostCost * Time.deltaTime))
        {
            MyRB.AddForce(PlanarInput * (MoveForce * AirMultiplier + BoostForce), ForceMode.Force);
        }
        else
        {
            if (Boosting)
                BoostControl(false);

            MyRB.AddForce(PlanarInput * MoveForce * AirMultiplier, ForceMode.Force);
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




    public void SetBoostSounds(AudioClip Boost, AudioClip Impulse)
    {
        BoostPlayer.clip = Boost;
        ImpulsePlayer.clip = Impulse;
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
                MyBS.ImpulseBoostEffect();
                if (ImpulsePlayer.isPlaying)
                    ImpulsePlayer.Stop();
                ImpulsePlayer.Play();
                MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * ImpulseBoostForce, ForceMode.Impulse);
            }

            if (CurrentBoostjuice >= BoostCost * Time.deltaTime)
            {
                MyBS.BoostEffect(true);
                BoostPlayer.Play();
                Boosting = true;
            }
        }
        else
        {
            if (Boosting)
            {
                Boosting = false;
                BoostPlayer.Stop();
                MyBS.BoostEffect(false);
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
        //old detection method using overlapsphere, new one helps slope movement
        //if (Physics.OverlapSphere(GroundDetectionSite.position, GroundDetectionRadius, GroundDetection).Length > 0)
        //    return true;
        //else
        //    return false;


        if (Physics.Raycast(GroundDetectionSite.position, -transform.up, out GroundUnder, GroundDetectionRadius,GroundDetection))
        {
            //float Angle = Vector3.Angle(transform.up, GroundUnder.normal); //gets angle of slope

            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if(GroundDetectionSite)
        Debug.DrawRay(GroundDetectionSite.position,-transform.up*GroundDetectionRadius,Color.red);
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

            MyBS.FloatEffect(true);
        }
        else if (!Floating && WasFloating)
            MyBS.FloatEffect(false);

        WasFloating = Floating;
    }





    #region Effects



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
