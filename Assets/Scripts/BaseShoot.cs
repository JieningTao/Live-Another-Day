using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShoot : MonoBehaviour
{
    [Header("Shot Base Settings")]
    [SerializeField]
    protected List<Transform> BulletSpawns;

    [SerializeField]
    protected BaseBullet ProjectileScript;

    protected GameObject ProjectilePrefab
    { get { return ProjectileScript.gameObject; } }

    [SerializeField]
    protected float AccuracyDeviation;


    [SerializeField]
    protected float TBS = 0.1f;

    [SerializeField]
    [Tooltip("only useful with a burst amount > 1")]
    protected BurstFireSettings MyBurstSettings;

    [System.Serializable]
    public class BurstFireSettings
    {
        [SerializeField]
        public int BurstAmount = 1;
        [SerializeField]
        public float BurstInterval = 0;
        [HideInInspector]
        public float BurstCD = 0;
        [HideInInspector]
        public int BurstRemaining = 0;
    }

    //[SerializeField]
    //protected float PerShotDamage = 10;

    //[SerializeField]
    //public DamageSystem.DamageType MyDamageType;
    //[SerializeField]
    //public List<DamageSystem.DamageTag> MyDamageTags;

    [SerializeField]
    protected FireMode MyFireMode;
    [SerializeField]
    protected GameObject MuzzleFlarePrefab;
    protected List<ParticleSystem> MuzzleFlares;

    [Space(20)]
    [SerializeField]
    protected Animator MyAnimator;
    [Space(20)]
    [Header("Shot Sound Settings")]
    [SerializeField]
    protected List<AudioClip> ShotSounds;
    protected List<AudioSource> SoundSources;
    [SerializeField]
    protected Vector2 SoundMinMax = new Vector2(5,500);
    [SerializeField]
    protected float Volume = 1;
    [SerializeField]
    protected float Pitch = 1;

    public enum FireMode
    {
        SemiAuto,
        FullAuto,
        Charge,
        MultiLock,
    }

    protected bool Firing = false;
    protected float FireCooldown = 0;
    protected LayerMask BulletHitMask;
    protected int ShotsFired = 0;

    public IDamageSource DamageSource;

    protected virtual void Start()
    {
        foreach (Transform a in BulletSpawns)
            a.gameObject.SetActive(true);
        InitializeBullet();

        if (MuzzleFlarePrefab)
            InitializeMuzzleFlare();

        if (ShotSounds.Count > 0)
        {
            InitializeAudioSources();
        }
        DamageSource = GetComponentInParent<IDamageSource>();
        //MyStatus = WeaponStatus.Normal;
    }

    public virtual void EquipWeapon()
    {
        //SetLayerAndBullet(gameObject.layer);
        ProjectileScript.InitBullet(this);
    }

    protected virtual int GetNextBulletSpawn()
    {
        if (BulletSpawns.Count == 1)
            return 0;

        return GetNextBulletSpawn(true);
    }

    protected virtual int GetNextBulletSpawn(bool count)
    {
        int a = ShotsFired % BulletSpawns.Count;

        if (count)
            ShotsFired++;

        return a;

    }

    protected virtual void InitializeMuzzleFlare()
    {
        //Debug.Log("Ping",this);
        MuzzleFlares = new List<ParticleSystem>();

        foreach (Transform a in BulletSpawns)
        {
            GameObject NewMuzzleFlare = Instantiate(MuzzleFlarePrefab, a.position, MuzzleFlarePrefab.transform.rotation, a.transform);
            NewMuzzleFlare.transform.localRotation = MuzzleFlarePrefab.transform.localRotation;
            ParticleSystem NewPS = NewMuzzleFlare.GetComponent<ParticleSystem>();
            AdjustEffectScale(NewPS, a.localScale);
            MuzzleFlares.Add(NewPS);

        }
    }

    public void AdjustEffectScale(ParticleSystem a, Vector3 Scale)
    {
        foreach (ParticleSystem b in a.GetComponentsInChildren<ParticleSystem>())
        {
            b.transform.localScale = Scale;
        }
    }

    protected virtual void InitializeBullet()
    {

        ProjectileScript = ProjectilePrefab.GetComponent<BaseBullet>();

        ProjectileScript.InitBullet(this);

        //int SetLayer = 0;

        //if (gameObject.layer == 9)
        //    SetLayer = 10;
        //else if (gameObject.layer == 11)
        //    SetLayer = 12;


        //ProjectileScript.SetLayerAndMask(SetLayer);
        //ProjectileScript.SetDamageSource();
    }

    protected virtual void SetLayerAndBullet(int Layer)
    {
        gameObject.layer = Layer;

        if (ProjectileScript == null)
            ProjectileScript = ProjectilePrefab.GetComponent<BaseBullet>();

        int SetLayer = 0;

        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        ProjectileScript.SetLayerAndMask(SetLayer);

    }

    public virtual void Trigger(bool Fire)
    {

        //Debug.Log(Fire +"\n TBS "+ FireCooldown, this);

        if (Fire && MyFireMode == FireMode.SemiAuto && FireCooldown <= 0)
        {
            if (MyBurstSettings.BurstAmount > 1)
                MyBurstSettings.BurstRemaining = MyBurstSettings.BurstAmount;
            else
                Fire1();
        }
        //else if (Fire && MyBurstSettings.BurstAmount > 1 && FireCooldown <= 0)
        //{

        //}
        else
            Firing = Fire;
    }

    protected virtual void Update()
    {
        if (FireCooldown > 0)
            FireCooldown -= Time.deltaTime;

        if (MyFireMode == FireMode.FullAuto)
        {

            if (FireCooldown <= 0)
            {
                if (Firing)
                {
                    //Debug.Log(MyBurstSettings.BurstAmount);

                    if (MyBurstSettings.BurstAmount > 1)
                    {
                        if (MyBurstSettings.BurstInterval <= 0)
                        {
                            for (int i = 0; i < MyBurstSettings.BurstAmount; i++)
                            {
                                Fire1();
                            }
                        }
                        else
                        MyBurstSettings.BurstRemaining = MyBurstSettings.BurstAmount;
                    }
                    else
                        Fire1();

                    FireCooldown = TBS;
                }
            }
        }

        if (MyBurstSettings.BurstAmount > 1)
        {
            if (MyBurstSettings.BurstCD > 0)
            {
                MyBurstSettings.BurstCD -= Time.deltaTime;
            }
            else if(MyBurstSettings.BurstRemaining>0)
            {
                if (MyBurstSettings.BurstCD <= 0)
                {
                    if (MyBurstSettings.BurstInterval > 0)
                    {
                        Fire1();
                        MyBurstSettings.BurstCD = MyBurstSettings.BurstInterval;
                    }
                    else
                    {
                        while (MyBurstSettings.BurstRemaining > 0)
                            Fire1();
                    }

                }
            }


        }

    }



    //actual executioon of firing 1 projectile
    protected virtual void Fire1()
    {
        int SlotNum = GetNextBulletSpawn();
        GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, BulletSpawns[SlotNum].position, BulletSpawns[SlotNum].rotation);
        NewBullet.SetActive(true);
        NewBullet.transform.Rotate(new Vector3(Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.Self);

        if (MuzzleFlarePrefab != null)
        {
            //Debug.Log(MuzzleFlares.Count);
            MuzzleFlares[SlotNum].Play();
        }

        if (ShotSounds.Count > 0)
            PlayShotSound(SlotNum);

        FireCooldown = TBS;

        if (MyBurstSettings.BurstAmount > 1)
            MyBurstSettings.BurstRemaining--;
        

        if (MyAnimator)
            MyAnimator.SetTrigger("Fire");
    }

    #region Editor Tool Stuff

    public virtual void RecieveBSs(List<Transform> a)
    {
        BulletSpawns = a;
    }

    #endregion

    #region Audio Related

    private void InitializeAudioSources()
    {
        //Debug.Log("IAS for " + gameObject.name, this);
        SoundSources = new List<AudioSource>();


        //foreach (Transform a in BulletSpawns)
        //{
        //    AudioSource Temp = a.GetComponent<AudioSource>();
        //    if(Temp)
        //    Temp.loop = false;
        //    Temp.playOnAwake = false;
        //    Temp.spatialBlend = 1;
        //    Temp.Stop();
        //    SoundSources.Add(Temp);

        //}

        //this creates audio sources
        foreach (Transform a in BulletSpawns)
        {
            AudioSource Temp = a.gameObject.AddComponent<AudioSource>();
            //Debug.Log("Ping "+ gameObject.name, Temp);
            Temp.loop = false;
            Temp.playOnAwake = false;
            Temp.Stop();
            Temp.spatialBlend = 1;
            Temp.volume = Volume;
            Temp.pitch = Pitch;
            Temp.minDistance = SoundMinMax.x;
            Temp.maxDistance = SoundMinMax.y;
            SoundSources.Add(Temp);

        }
    }

    protected void PlayShotSound(int SpawnNum, AudioClip Clip)
    {
        //Debug.Log(SpawnNum + Clip.name, SoundSources[SpawnNum]);
        if (ShotSounds.Count >= 1 && SoundSources[SpawnNum] != null)
        {
            if (SoundSources[SpawnNum].isPlaying)
                SoundSources[SpawnNum].Stop();

            SoundSources[SpawnNum].clip = Clip;

            SoundSources[SpawnNum].Play();
        }
    }

    protected void PlayShotSound(int SpawnNum)
    {
        if(ShotSounds.Count>1)
            PlayShotSound(SpawnNum, ShotSounds[Random.Range(0, ShotSounds.Count)]);
        else
            PlayShotSound(SpawnNum, ShotSounds[Random.Range(0, 0)]);
    }

    


    #endregion

    #region Info Request stuff


    public virtual float GetAmmoGauge()
    {
        return 1;
    }

    public virtual bool GetFirable()
    {
        return true;
    }

    public virtual string GetAmmoText()
    {
        return "";
    }

    public virtual bool LowAmmoWarning()
    {
        return false;
    }

    public virtual bool LowEnergyWarning()
    {
        return false;
    }

    public virtual float GetProjectileSpeed()
    {
        return ProjectileScript.GetSpeed();
    }

    #region Loadoutpart request info stuff

    public virtual string GetDamage
    { get { return ProjectileScript.GetDamage; } }

    public virtual string GetAccuracy
    { get { return AccuracyDeviation+ "°"; } }

    public virtual string GetFireRate
    { get { return (1f/TBS).ToString("F2")+"/s"; } }

    public virtual string GetFireMode
    {
        get {
            if (MyFireMode == FireMode.FullAuto)
                return "Auto";
            else if (MyFireMode == FireMode.SemiAuto)
                return "Semi";
            else if (MyFireMode == FireMode.Charge)
                return "Charge";
            else
                return "Lock";
        }
    }

    public virtual string GetMag
    { get { return "Err"; } }

    public virtual string GetReload
    { get { return "Err"; } }

    #endregion

    #endregion

}
