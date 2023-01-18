using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Mob1Ctrl : MonoBehaviour
{
    [Header("Instance")]
    [SerializeField]
    private IhasTag ITag;
    [SerializeField]
    private IPhysics physics;
    [SerializeField]
    private KillMgrCtrl killMgr;
    [SerializeField]
    private Animator animator;
    [Header("Sensor")]
    [SerializeField]
    private GameObject SensorRoot;
    [SerializeField]
    private Collider2D TargetSensor;
    [SerializeField]
    private Collider2D AttackSensor;
    [SerializeField]
    private Collider2D AttackField;

    [Header("Args")]
    [SerializeField]
    private float MoveSpeed = 10f;
    [SerializeField]
    private float AttackCoolDown = 1f;
    [SerializeField]
    private float AttackDamage = 10f;
    [SerializeField]
    private float AttackPreDelay = 0.2f; //¹¥»÷Ç°Ò¡
    [SerializeField]
    private float DashSpeed = 25f;
    [SerializeField]
    private float DashTime = 0.225f;


    private Collider2D[] c = new Collider2D[4];
    private RaycastHit2D[] rch = new RaycastHit2D[4];
    private float ScanTimer = 0f;
    private float targetX = 0f;
    private bool hasTarget = false;
    private bool attacking = false;
    private Vector3 lastpos;
    private float velocityH = 0f;
    private bool preDelaying = false;
    private float attackCoolDownTimer = 0f;
    private ContactFilter2D filter;
    private bool FaceRight = true;
    private Vector3 scl;
    private Hashtable atked = new Hashtable();
    private bool die = false;

    // Start is called before the first frame update
    void Start()
    {
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Player");
        filter.useTriggers = false;
        lastpos = gameObject.transform.position;
        targetX = lastpos.x;
        scl = SensorRoot.transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (die) return;
        float dt = Time.deltaTime;
        float stun = ITag.getFloat("STUN");
        Vector3 pos = gameObject.transform.position;

        if (ScanTimer <= 0 && stun <= 0 && !attacking && attackCoolDownTimer <= 0f)
        {
            hasTarget = Hit.Overlap(TargetSensor, LayerMask.GetMask("Player"), c) > 0;
            if (hasTarget)
            {
                targetX = c[0].gameObject.transform.position.x;
                FaceRight = targetX - pos.x > 0;
                scl.x = FaceRight ? 1 : -1;
                SensorRoot.transform.localScale = scl;
            }
            else
            {
                physics.setVelocityH(0);
                targetX = gameObject.transform.position.x;
            }
            ScanTimer = 0.2f;
        }

        if (hasTarget && attackCoolDownTimer <= 0f && Hit.Overlap(AttackSensor, LayerMask.GetMask("Player"), c) > 0 && !attacking)
        {
            targetX = c[0].gameObject.transform.position.x;
            StartCoroutine(ATK());
        }


        if (stun > 0)
        {
            attacking = false;
            physics.setVelocityH(0);
        }
        else if (attacking)
        {
            if(preDelaying) physics.setVelocityH(0);
            else
            {
                physics.setVelocityH(DashSpeed * (FaceRight ? 1f : -1f));
                if (AttackField.Cast(physics.getVelocity(), filter, rch, -Vector2.Distance(pos, lastpos)) > 0)
                {
                    if (atked[rch[0].collider.gameObject] == null)
                    {
                        if (Battle.Damage(rch[0].collider.gameObject.GetComponent<IhasTag>(), AttackDamage))
                            atked[rch[0].collider.gameObject] = true;
                    }
                }
            }
        }
        else if (hasTarget && attackCoolDownTimer <= 0f)
        {
            if (Mathf.Abs(targetX - pos.x) > 0.3f)
            {
                FaceRight = targetX - pos.x > 0;
                physics.setVelocityH((targetX - pos.x > 0f ? 1f : -1f) * MoveSpeed);
            }
            else
            {
                physics.setVelocityH(0);
            }
        }
        else
        {
            physics.setVelocityH(0);
        }

        if (gameObject.GetComponent<IhasTag>().getFloat("_health") <= 0)
        {
            killMgr.AddKill("Kill_Mob1");
            StartCoroutine(DIE());
        }

        animator.SetBool("walk", Mathf.Abs(physics.getVelocity().x) >= 0.05f);
        
        attackCoolDownTimer = Timer.Update(attackCoolDownTimer, dt);
        ScanTimer = Timer.Update(ScanTimer, dt);

        ITag.putFloat("STUN", Timer.Update(stun, dt));
        lastpos = gameObject.transform.position;
    }

    IEnumerator ATK()
    {
        preDelaying = true;
        AttackField.enabled = true;
        AttackSensor.enabled = false;
        atked.Clear();
        attacking = true;
        attackCoolDownTimer = AttackPreDelay + DashTime + AttackCoolDown;
        yield return new WaitForSeconds(AttackPreDelay);
        preDelaying = false;
        yield return new WaitForSeconds(DashTime);
        AttackField.enabled = false;
        AttackSensor.enabled = true;
        attacking = false;
        attackCoolDownTimer = AttackCoolDown;
    }

    IEnumerator DIE()
    {
        die = true;
        animator.SetBool("die", true);
        yield return new WaitForSeconds(0.4f);
        GameObject.Destroy(gameObject, 0f);
    }

}
