using Assets.Global.Scrpits;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob2Ctrl : MonoBehaviour
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

    private Collider2D[] c = new Collider2D[4];
    private float ScanTimer = 0f;
    private float targetX = 0f;
    private bool hasTarget = false;
    private bool attacking = false;
    private Vector3 lastpos;
    private float velocityH = 0f;
    private float attackCoolDownTimer = 0f;
    private int lm;
    private bool FaceRight = true;
    private Vector3 scl;
    private bool died = false;

    // Start is called before the first frame update
    void Start()
    {
        lm = LayerMask.GetMask("Player");
        ITag.onChangeEvent = (k, v) =>
        {
            if(k == "float:_health" && !attacking && !died)
            {
                animator.Play("attacked");
            }
            
            return v;
        };
        lastpos = gameObject.transform.position;
        targetX = lastpos.x;
        scl = SensorRoot.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (died)
        {
            physics.setVelocityH(0);
            return;
        }
        float dt = Time.deltaTime;
        float stun = ITag.getFloat("STUN");
        Vector3 pos = gameObject.transform.position;

        if (ScanTimer <= 0 && stun <= 0 && !attacking && attackCoolDownTimer <= 0f)
        {
            hasTarget = Hit.Overlap(TargetSensor, lm, c) > 0;
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

        if (hasTarget && attackCoolDownTimer <= 0f && Hit.Overlap(AttackSensor, lm, c) > 0 && !attacking)
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
            physics.setVelocityH(0);
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
            StartCoroutine(Die());
        }

        attackCoolDownTimer = Timer.Update(attackCoolDownTimer, dt);
        ScanTimer = Timer.Update(ScanTimer, dt);

        ITag.putFloat("STUN", Timer.Update(stun, dt));
        lastpos = gameObject.transform.position;
    }

    IEnumerator ATK()
    {
        AttackField.enabled = true;
        AttackSensor.enabled = false;
        attacking = true;

        animator.Play("attack");

        attackCoolDownTimer = AttackCoolDown + 0.28f;
        yield return new WaitForSeconds(0.28f);

        if (Hit.Overlap(AttackField,lm,c) > 0)
        {
            Battle.Damage(c[0].gameObject.GetComponent<IhasTag>(), AttackDamage);
        }

        attackCoolDownTimer = AttackCoolDown + 0.28f;
        //attackCoolDownTimer = DashTime + AttackCoolDown;
        yield return null;
        AttackField.enabled = false;
        AttackSensor.enabled = true;
        attacking = false;
        attackCoolDownTimer = AttackCoolDown;
    }

    IEnumerator Die()
    {
        died = true;
        animator.Play("die");
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(1.2f);
        killMgr.AddKill("Kill_Mob2");
        GameObject.Destroy(gameObject, 0f);
    }

}
