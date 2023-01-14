using Assets.Global.Scrpits;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header(header: "Instance")]
    public IPhysics physics;
    public IhasTag ITag;
    [SerializeField]
    private HealthBarCtrl healthBar;
    [SerializeField]
    private GameObject fireball;
    [SerializeField]
    private GameObject bigfireball;
    [SerializeField]
    private CinemachineVirtualCamera cam;
    [Header(header: "SensorField")]
    [SerializeField]
    private GameObject SensorRoot;
    [SerializeField]
    private Collider2D GroundSensor;
    [SerializeField]
    private Collider2D BasicAtkSensor;
    [SerializeField]
    private Collider2D BasicAtk2Sensor;
    [SerializeField]
    private GameObject visual;
    [SerializeField]
    private GameObject ShootCenter;

    [Header(header: "Arguments")]
    [SerializeField]
    private float WalkSpeed = 10.0f;
    [SerializeField]
    private float WalkAcceleration = 100.0f;
    [SerializeField]
    private float RunSpeed = 15f;
    [SerializeField]
    private float MaxRunTime = 1f;

    [SerializeField]
    private float JumpSpeed = 25f;
    [SerializeField]
    private float JumpSpeedTime = 0.02f;
    [SerializeField]
    private float JumpKeyBuffertime = 0.2f;
    [SerializeField]
    private float JumpKeyPressTime = 0.05f;
    [SerializeField]
    private float MaxGroundBuffer = 0.08f;

    [SerializeField]
    private float BasicAtkCoolDown = 0.25f;
    [SerializeField]
    private float ShootDamage = 10f;
    [SerializeField]
    private float FireBallShootSpeed = 25f;
    [SerializeField]
    private float ShootCoolDown = 0.25f;
    [SerializeField]
    private float ShootCost = 10f;

    [SerializeField]
    private float hpMax = 100f;

    [SerializeField]
    private float mpMax = 100f;
    [SerializeField]
    private float mpRebornSpeed = 10f;
    [SerializeField]
    private float mpRebornDelay = 0.4f;
    [SerializeField]
    private float spMax = 100f;

    [Header(header: "Visual")]
    [SerializeField]
    private float VisualRotSpeed = 25f;

    #region Var
    private Collider2D[] colliderHited = new Collider2D[30];
    private float CurrentSpeedH = 0f;
    private float JumpKeyBuffer = 0f;
    private float JumpKeyPress = 0f;
    private float GroundBuffer = 1f;
    private Vector3 lastpos;
    private float visual_s = 1f;
    private float visual_st = 1f;
    private float RunTimer = 0f;
    private bool CanDoubleJump = false;
    private bool onGroundLast = false;
    private float BasicAtkCoolDownTimer = 0f;
    private float ShootCoolDownTimer = 0f;
    private float basicAtkDmg = 50f;
    private float mpRebornTimer = 0f;
    private ContactFilter2D filter;

    #endregion

    void Start()
    {
        Random.InitState(114514);
        lastpos = gameObject.transform.position;
        filter.useLayerMask = true;
        BasicAtkSensor.enabled = false;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        bool onGround = groundCheck();
        float AxisH = Input.GetAxisRaw("Horizontal");
        //int AxisV = (int)Input.GetAxisRaw("Vertical");
        float stun = ITag.getFloat("STUN");

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && RunTimer <= 0f)
        {
            RunTimer = MaxRunTime;
        }

        if(AxisH == 0f)
        {
            RunTimer = 0f;
        }

        if (onGround)
        {
            float targetSpeed = (RunTimer > 0 ? RunSpeed:WalkSpeed)*AxisH;
            if (AxisH == 0f)
            {
                targetSpeed = 0f;
            }
            if(stun > 0)
            {
                targetSpeed = 0f;
                CurrentSpeedH = physics.getVelocity().x;
            }

            if (CurrentSpeedH * AxisH < 0) CurrentSpeedH = 0;
            if (Mathf.Abs(targetSpeed - CurrentSpeedH) < WalkAcceleration * dt) CurrentSpeedH = targetSpeed;
            else CurrentSpeedH += (targetSpeed - CurrentSpeedH > 0 ? 1f : -1f) * dt * WalkAcceleration;
            if (stun <= 0)
            {
                physics.setVelocityH(CurrentSpeedH);
            }
            GroundBuffer = MaxGroundBuffer;

            CanDoubleJump = false;
        }
        else
        {
            CurrentSpeedH = physics.getVelocity().x;
            if(onGroundLast == true)
            {
                CanDoubleJump = true;
            }
        }

        //visual rot
        if (AxisH != 0) visual_st = AxisH > 0 ? 1f : -1f;
        Vector3 _s = SensorRoot.transform.localScale;
        _s.x = visual_st;
        SensorRoot.transform.localScale = _s;
        if (Mathf.Abs(visual_st - visual_s) < VisualRotSpeed * dt) visual_s = visual_st;
        else visual_s += (visual_st - visual_s > 0 ? 1f : -1f) * VisualRotSpeed * dt;
        _s = visual.transform.localScale;
        _s.x = Mathf.Sin(Mathf.PI*0.5f*visual_s);
        visual.transform.localScale = _s;

        //Jump Key handle
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpKeyBuffer = JumpKeyBuffertime;
            if (ITag.getBool("hasWing") && CanDoubleJump)
            {
                physics.setVelocityH((RunTimer > 0 ? RunSpeed : WalkSpeed) * AxisH);
                GroundBuffer = MaxGroundBuffer;
                CanDoubleJump = false;
            }
        }
        if (Input.GetKey(KeyCode.Space) && JumpKeyBuffer==-1f)
        {
            if(JumpKeyPress <= JumpKeyPressTime) JumpKeyPress += dt;
        }
        else
        {
            JumpKeyPress = 0;
        }

        //Jump
        if (JumpKeyBuffer > 0  && GroundBuffer > 0 && physics.getVelocity().y <= 0.01f)
        {
            GroundBuffer = 0f;
            JumpKeyBuffer = -1f;
            physics.setVelocityOverrideV(JumpSpeed, JumpSpeedTime);
        }
        if(JumpKeyPress > 0 && JumpKeyPress<= JumpKeyPressTime)
        {
            physics.setVelocityOverrideV(JumpSpeed, JumpSpeedTime);
        }

        //attack handler
        if (Input.GetMouseButton(0))
        {
            if (BasicAtkCoolDownTimer <= 0f)
            {
                BasicAtk(ITag.getInt("BasicAtk") > 0);
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            int wandLevel = ITag.getInt("Wand");
            if (wandLevel > 0)
            {
                if (ShootCoolDownTimer <= 0f)
                {
                    float _mp = ITag.getFloat("_magic");
                    if (_mp >= ShootCost)
                    {
                        _mp -= ShootCost;
                        if (_mp < 0) _mp = 0;
                        ITag.putFloat("_magic", _mp);
                        mpRebornTimer = mpRebornDelay;
                        FireBallAtk(wandLevel > 1);
                    }
                }
            }
        }



        if(Input.GetMouseButton(2)) {
            

            
        }



        //handle buffer;
        JumpKeyBuffer = Timer.Update(JumpKeyBuffer, dt);
        GroundBuffer = Timer.Update(GroundBuffer, dt);
        BasicAtkCoolDownTimer = Timer.Update(BasicAtkCoolDownTimer, dt);
        ShootCoolDownTimer = Timer.Update(ShootCoolDownTimer, dt);
        mpRebornTimer = Timer.Update(mpRebornTimer, dt);

        ITag.putBool("RUN", RunTimer > 0);

        RunTimer = Timer.Update(RunTimer, dt);
        
        if (stun <= 0f)
        {
            if(CurrentSpeedH == 0)
            {
                Vector3 p = gameObject.transform.position;
                p.x = lastpos.x;
                gameObject.transform.position = p;
            }
        }
        lastpos = gameObject.transform.position;


        ITag.putFloat("STUN", Timer.Update(stun, dt));

        float mp = ITag.getFloat("_magic");
        if (mpRebornTimer <= 0)
        {
            mp += mpRebornSpeed * dt;
            if (mp > mpMax) mp = mpMax;
            ITag.putFloat("_magic", mp);
        }
        healthBar.setMPRate(mp / mpMax);

        float hp = ITag.getFloat("_health");
        healthBar.setHPRate(hp / hpMax);
        onGroundLast = onGround;
    }

    private void FireBallAtk(bool isBig)
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) - ShootCenter.transform.position;
        Vector3 p = ShootCenter.transform.position;
        p.z = 1f;
        v.z = 0f;
        if(isBig)
        {
            ExplodeProjectileCtrl fbc = Instantiate<GameObject>(bigfireball, p, Quaternion.FromToRotation(Vector3.right, v)).GetComponent<ExplodeProjectileCtrl>();
            fbc.Shoot(v.normalized * FireBallShootSpeed, LayerMask.GetMask("World", "Mob"), ShootDamage,5f);
        }
        else
        {
            ProjectileCtrl fbc = Instantiate<GameObject>(fireball, p, Quaternion.FromToRotation(Vector3.right, v)).GetComponent<ProjectileCtrl>();
            fbc.Shoot(v.normalized * FireBallShootSpeed, LayerMask.GetMask("World", "Mob"), ShootDamage*1.2f, 5f);
        }
        ShootCoolDownTimer = ShootCoolDown;
    }

    private void BasicAtk(bool LevelUp)
    {
        print("BA");
        Collider2D ATKS = LevelUp ? BasicAtk2Sensor : BasicAtkSensor;
        ATKS.enabled = true;
        Hit.Overlap(ATKS, LayerMask.GetMask("World", "Mob"), colliderHited);
        Hashtable atked = new Hashtable();
        foreach (Collider2D c in colliderHited)
        {
            if (!(c && !c.isTrigger)) continue;
            if (atked[c.gameObject] != null) continue;
            IhasTag TAG = c.gameObject.GetComponent<IhasTag>();
            if (!TAG) continue;
            if (TAG.hasVar("float","_health"))
            {
                float h = TAG.getFloat("_health");
                h = h-basicAtkDmg>=0 ? h-basicAtkDmg : 0;
                TAG.putFloat("_health", h);
                atked[c.gameObject] = true;
                //break;
            }
        }
        BasicAtkCoolDownTimer = BasicAtkCoolDown;
        ATKS.enabled = false;
    }

    private bool groundCheck()
    {
        return Hit.Overlap(GroundSensor, LayerMask.GetMask("World"), colliderHited) > 0;
    }

    private void addSp(float s)
    {
        float sp = ITag.getFloat("_soul");
        sp += s;
        if (sp > spMax) sp = spMax;
        ITag.putFloat("_soul", sp);
    }

    private void addMp(float m)
    {
        float mp = ITag.getFloat("_magic");
        mp += m;
        if (mp > mpMax) mp = mpMax;
        ITag.putFloat("_magic", mp);
    }

    private void SpToMp(float s)
    {
        float mp = ITag.getFloat("_magic");
        float sp = ITag.getFloat("_soul");
        float rs = s;
        if (mp+rs > mpMax) rs = mpMax-mp;
        if (sp - rs < 0) rs = sp;
        mp -= rs;
        sp -= rs;
        ITag.putFloat("_soul", sp);
        ITag.putFloat("_magic", mp);
    }

    public void TP(Vector3 pos)
    {
        lastpos = pos;
        gameObject.transform.position = lastpos;
    }

    void FixedUpdate()
    {
        
    }
}
