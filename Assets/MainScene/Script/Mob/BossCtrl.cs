using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCtrl : MonoBehaviour
{
    [Header("Instance")]
    [SerializeField]
    private IhasTag ITag;
    [SerializeField]
    private CamCtrl CamCtrl;

    [Header("Sensor")]
    [SerializeField]
    private GameObject SensorRoot;
    [SerializeField]
    private Collider2D TargetSensor;
    [SerializeField]
    private Collider2D AtkField;

    [Header("Args")]
    [SerializeField]
    private GameObject ProjectileObj;
    [SerializeField]
    private float AttackCoolDown = 1f;
    [SerializeField]
    private float AttackDamage = 20f;
    [SerializeField]
    private float ShootDamage = 10f;
    [SerializeField]
    private float ProjectileSpeed = 10f;
    [SerializeField]
    private float MaxHP = 1000f;
    [SerializeField]
    private float HalfAtkDistance = 9f;
    [SerializeField]
    private float atkTime = 9f;
    [SerializeField]
    private float atkPrevDelay = 1f;
    [SerializeField]
    private AnimationCurve Atk12AnimCurve;
    [SerializeField]
    private int ShootCount1 = 9;
    [SerializeField]
    private int ShootBranch1 = 5;
    [SerializeField]
    private float HalfShootAng1 = 45f;
    [SerializeField]
    private int ShootCount2 = 18;
    [SerializeField]
    private int ShootBranch2 = 8;
    [SerializeField]
    private float HalfShootAng2 = 60f;
    [SerializeField]
    private float ShootDelay = 0.2f;
    [SerializeField]
    private float HalfShootRotAng = 30f;
    [SerializeField]
    private float StunTime = 4f;

    private Collider2D[] c = new Collider2D[4];
    private RaycastHit2D[] rch = new RaycastHit2D[4]; 
    private float ScanTimer = 0f;
    private float attackCoolDownTimer = 0f;
    private int lm;
    private Vector3 atkfpos;
    private float atkTimer = 0f;
    private float atkPrevTime = 0f;
    private float atkPrevDelayTimer = 0f;
    private Hashtable atked = new Hashtable();
    private ContactFilter2D filter;
    private int shootCounter = 0;

    private enum BossState
    {
        Idle = -1,
        Idle0 = 0,
        ATK1 = 1,
        Idle1 = 2,
        ATK2 = 3,
        Idle2 = 4,
        ATK3 = 5,
        Idle3 = 6,
        ATK4 = 7,
        Idle4 = 8,
        STUN = 9
    };

    private BossState state;


    // Start is called before the first frame update
    void Start()
    {
        lm = LayerMask.GetMask("Player");
        filter.useLayerMask = true;
        filter.layerMask = lm;
        filter.useTriggers = false;
        atkfpos = AtkField.gameObject.transform.localPosition;
        AtkField.gameObject.SetActive(false);
        ITag.onChangeEvent = (k, v) =>
        {
            if (k == "float:_health" && state != BossState.STUN)
            {
                return ITag.getFloat("_health");
            }


            return v;
        };
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        float stun = ITag.getFloat("STUN");
        
        if (ScanTimer <= 0f && stun <= 0f)
        {
            if (Hit.Overlap(TargetSensor, lm, c) > 0)
            {
                if(state == BossState.Idle)
                {
                    atkPrevDelayTimer = atkPrevDelay;
                    state = BossState.Idle0;
                }
                CamCtrl.setMul(1.5f);
            }
            else
            {
                state = BossState.Idle;
                atkTimer = 0;
                atkPrevDelayTimer = 0;
                CamCtrl.setMul(1);
            }
            ScanTimer = 0.3f;
        }


        //state handler
        if(state == BossState.Idle)
        {
            ITag.putFloat("_health", Mathf.Clamp(ITag.getFloat("_health") + dt * 50f, 0, MaxHP));
        }
        else if (state == BossState.Idle0)
        {
            if (atkPrevDelayTimer <= 0)
            {
                atkTimer = atkTime;
                atked.Clear();
                state = BossState.ATK1;
            }
        }
        else if(state == BossState.ATK1)
        {
            if(atkTimer > 0)
            {
                AtkField.gameObject.SetActive(true);
                Vector3 p = AtkField.gameObject.transform.localPosition;
                p.x = atkfpos.x + Atk12AnimCurve.Evaluate(atkPrevTime / atkTime) * HalfAtkDistance * 2 - HalfAtkDistance;
                AtkField.gameObject.transform.localPosition = p;
                if (AtkField.Cast(Vector2.right, filter, rch, 2*(Atk12AnimCurve.Evaluate(atkPrevTime / atkTime) - Atk12AnimCurve.Evaluate(atkTimer / atkTime))* HalfAtkDistance) > 0)
                {
                    if (atked[rch[0].collider.gameObject] == null)
                    {
                        IhasTag tags = rch[0].collider.gameObject.GetComponent<IhasTag>();
                        Battle.Damage(tags, AttackDamage);
                        atked[rch[0].collider.gameObject] = true;
                    }
                }
            }
            else
            {
                AtkField.gameObject.SetActive(false);
                atkPrevDelayTimer = atkPrevDelay;
                state = BossState.Idle1;
            }
        }
        else if(state == BossState.Idle1)
        {
            if (atkPrevDelayTimer <= 0)
            {
                atkTimer = atkTime;
                atked.Clear();
                state = BossState.ATK2;
            }
        }
        else if (state == BossState.ATK2)
        {
            if (atkTimer > 0)
            {
                Vector3 p = AtkField.gameObject.transform.localPosition;
                p.x = atkfpos.x - Atk12AnimCurve.Evaluate(atkPrevTime / atkTime) * HalfAtkDistance * 2 + HalfAtkDistance;
                AtkField.gameObject.transform.localPosition = p;
                AtkField.gameObject.SetActive(true);
                if (AtkField.Cast(Vector2.left, filter, rch, 2 * (Atk12AnimCurve.Evaluate(atkPrevTime / atkTime) - Atk12AnimCurve.Evaluate(atkTimer / atkTime)) * HalfAtkDistance) > 0)
                {
                    if (atked[rch[0].collider.gameObject] == null)
                    {
                        IhasTag tags = rch[0].collider.gameObject.GetComponent<IhasTag>();
                        Battle.Damage(tags, AttackDamage);
                        atked[rch[0].collider.gameObject] = true;
                    }
                }
            }
            else
            {
                AtkField.gameObject.SetActive(false);
                atkPrevDelayTimer = atkPrevDelay;
                state = BossState.Idle2;
            }
        }
        else if (state == BossState.Idle2)
        {
            if (atkPrevDelayTimer <= 0)
            {
                shootCounter = 0;
                atkTimer = ShootDelay;
                state = BossState.ATK3;
            }
        }
        else if(state == BossState.ATK3)
        {
            if (atkTimer <= 0)
            {
                float rot = HalfShootRotAng * 2 / (ShootCount1-1)*shootCounter;
                float angl = HalfShootAng1 * 2 / (ShootBranch1-1);
                for (int i=0; i< ShootBranch1; ++i)
                {
                    Shoot(Quaternion.AngleAxis(HalfShootAng1-angl*i+rot-HalfShootRotAng, Vector3.forward)*Vector3.down);
                }

                shootCounter++;
                atkTimer = ShootDelay;
            }

            if (shootCounter >= ShootCount1)
            {
                atkPrevDelayTimer = atkPrevDelay;
                state = BossState.Idle3;
            }
        }
        else if (state == BossState.Idle3)
        {
            if (atkPrevDelayTimer <= 0)
            {
                shootCounter = 0;
                atkTimer = ShootDelay;
                state = BossState.ATK4;
            }
        }
        else if (state == BossState.ATK4)
        {
            if (atkTimer <= 0)
            {
                float rot = HalfShootRotAng * 2 / (ShootCount2 - 1) * shootCounter;
                float angl = HalfShootAng2 * 2 / (ShootBranch2 - 1);
                for (int i = 0; i < ShootBranch2; ++i)
                {
                    Shoot(Quaternion.AngleAxis(-(HalfShootAng2 - angl * i + rot - HalfShootRotAng), Vector3.forward) * Vector3.down);
                }

                shootCounter++;
                atkTimer = ShootDelay;
            }

            if (shootCounter >= ShootCount2)
            {
                atkPrevDelayTimer = StunTime;
                state = BossState.STUN;
            }
        }
        else
        {
            if (atkPrevDelayTimer <= 0)
            {
                atkPrevDelayTimer = atkPrevDelay;
                state = BossState.Idle0;
            }
        }

        if (state == BossState.STUN && gameObject.GetComponent<IhasTag>().getFloat("_health") <= 0)
        {
            CamCtrl.setMul(1f);
            GameObject.Destroy(gameObject, 0f);
        }

        attackCoolDownTimer = Timer.Update(attackCoolDownTimer, dt);
        ScanTimer = Timer.Update(ScanTimer, dt);
        atkTimer = Timer.Update(atkTimer, dt);
        atkPrevDelayTimer = Timer.Update(atkPrevDelayTimer, dt);

        ITag.putFloat("STUN", Timer.Update(stun, dt));

        atkPrevTime = atkTimer;
    }

    private void Atk(bool lr)
    {

    }


    private void Shoot(Vector3 target)
    {
        Vector3 p = gameObject.transform.position;
        Vector3 v = target;
        p.z = 1f;
        v.z = 0f;
        ProjectileCtrl fbc = Instantiate<GameObject>(ProjectileObj, p, Quaternion.FromToRotation(Vector3.right, target)).GetComponent<ProjectileCtrl>();
        fbc.Shoot(v.normalized * ProjectileSpeed, LayerMask.GetMask("World", "Player"), ShootDamage);
        attackCoolDownTimer = AttackCoolDown;
    }
}
