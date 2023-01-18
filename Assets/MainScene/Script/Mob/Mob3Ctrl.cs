using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob3Ctrl : MonoBehaviour
{

    [Header("Instance")]
    [SerializeField]
    private IhasTag ITag;
    [SerializeField]
    private KillMgrCtrl killMgr;

    [Header("Sensor")]
    [SerializeField]
    private GameObject SensorRoot;
    [SerializeField]
    private Collider2D TargetSensor;


    [Header("Args")]
    [SerializeField]
    private GameObject ProjectileObj;
    [SerializeField]
    private float AttackCoolDown = 1f;
    [SerializeField]
    private float AttackDamage = 10f;
    [SerializeField]
    private float ProjectileSpeed = 20f;

    private Collider2D[] c = new Collider2D[4];
    private float ScanTimer = 0f;
    private float attackCoolDownTimer = 0f;
    private int lm;
    private Vector3 rayOff;

    // Start is called before the first frame update
    void Start()
    {
        lm = LayerMask.GetMask("Player");
        rayOff = Vector3.up*0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        float stun = ITag.getFloat("STUN");
        if (ScanTimer <= 0f && stun <= 0f && attackCoolDownTimer <= 0f && Hit.Overlap(TargetSensor, lm, c) > 0)
        {
            Vector3 v = c[0].gameObject.transform.position;
            Vector3 p = gameObject.transform.position;
            v.z = 0;
            RaycastHit2D rch = Physics2D.Raycast(p, v - p, Vector2.Distance(v, p),LayerMask.GetMask("World","Player"));
            RaycastHit2D rch2 = Physics2D.Raycast(p, v - p + rayOff, Vector2.Distance(v + rayOff, p), LayerMask.GetMask("World", "Player"));
            RaycastHit2D rch3 = Physics2D.Raycast(p, v - p - rayOff, Vector2.Distance(v - rayOff, p), LayerMask.GetMask("World", "Player"));
            if (rch && rch.collider && rch.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Atk(rch.collider.gameObject.transform.position);
            }
            else if (rch2 && rch2.collider && rch2.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Atk(rch2.collider.gameObject.transform.position);
            }
            else if (rch3 && rch3.collider && rch3.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Atk(rch3.collider.gameObject.transform.position);
            }

            ScanTimer = 0.2f;
        }

        if (gameObject.GetComponent<IhasTag>().getFloat("_health") <= 0)
        {
            killMgr.AddKill("Kill_Mob3");
            GameObject.Destroy(gameObject, 0f);
        }

        attackCoolDownTimer = Timer.Update(attackCoolDownTimer, dt);
        ScanTimer = Timer.Update(ScanTimer, dt);

        ITag.putFloat("STUN", Timer.Update(stun, dt));
    }

    private void Atk(Vector3 target)
    {
        Vector3 p = gameObject.transform.position;
        Vector3 v = target - p;
        p.z = 1f;
        v.z = 0f;
        ProjectileCtrl fbc = Instantiate<GameObject>(ProjectileObj, p, Quaternion.FromToRotation(Vector3.right, v)).GetComponent<ProjectileCtrl>();
        fbc.Shoot(v.normalized * ProjectileSpeed, LayerMask.GetMask("World", "Player"), AttackDamage,5f);

        attackCoolDownTimer = AttackCoolDown;
    }
}
