using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechMovement : MonoBehaviour
{

    [SerializeField]
    Transform CameraAnchor;
    [SerializeField]
    Transform RightArm;
    [SerializeField]
    Transform LeftArm;


    [SerializeField]
    float TurnSpeed;
    [SerializeField]
    float MoveForce;
    [SerializeField]
    float SpeedLimit;
    [SerializeField]
    float BoostSpeedLimit;
    [SerializeField]
    float BoostMultiplier;
    [SerializeField]
    float BoostCost;
    [SerializeField]
    float ImpulseBoostForce;
    [SerializeField]
    float ImpulseCost;
    [SerializeField] //SFT (Serialized for testing)
    public bool Boosting;
    bool WasBoosting;
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
    private float FloatForce;

    [SerializeField]
    private LayerMask GroundDetection;
    [SerializeField]
    private Transform GroundDetectionsite;
    [SerializeField]
    private float GroundDetectionRadius;

    [SerializeField]
    private List<ParticleSystem> BoostExhausts;
    [SerializeField]
    private List<ParticleSystem> BoostImpulses;
    [SerializeField]
    private ParticleSystem JumpEffect;
    [SerializeField]
    private List<ParticleSystem> FloatThrusters;



    float JumpCooldown = 0.2f;
    float JCRemaining;
    public Vector3 MovementInput;
    private Rigidbody MyRB;


    private void Start()
    {
        MyRB = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        JCRemaining = 0;
        CurrentBoostjuice = BoostJuiceCapacity;
    }


    private void Update()
    {
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
                BoostEffect(false);
            return;
        }

        bool ShouldBeBoosting = false;

        float AirMultiplier = 1; //mid-air manuvering is less effective unless boosting, under test
        if (!grounded())
            AirMultiplier = 0.1f;
        else if (!grounded() && Boosting)
            AirMultiplier = 0.7f;


        if (Boosting && AttemptUseBoostJuice(BoostCost * Time.deltaTime))
        {
            MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * MoveForce * BoostMultiplier * AirMultiplier, ForceMode.Force);
            ShouldBeBoosting = true;
        }
        else
            MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * MoveForce * AirMultiplier, ForceMode.Force);



        if (!WasBoosting && ShouldBeBoosting)
        {
            BoostEffect(true);

            if (AttemptUseBoostJuice(ImpulseCost))
            {
                ImpulseBoostEffect();
                MyRB.AddForce(transform.TransformDirection(PlanarInput).normalized * ImpulseBoostForce, ForceMode.Impulse);
            }
        }
        else if (!ShouldBeBoosting && WasBoosting)
            BoostEffect(false);

        WasBoosting = ShouldBeBoosting;
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
        if (Physics.OverlapSphere(GroundDetectionsite.position, GroundDetectionRadius, GroundDetection).Length > 0)
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

    public void Rotate(Vector3 Rot)
    {
        transform.Rotate((new Vector3(0, Rot.y, 0)) * TurnSpeed);

        Vector3 VerticalRotation = (new Vector3(Rot.x, 0, 0)) * TurnSpeed;
        CameraAnchor.transform.Rotate(VerticalRotation);
        RightArm.transform.Rotate(VerticalRotation);
        LeftArm.transform.Rotate(VerticalRotation);
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
}
