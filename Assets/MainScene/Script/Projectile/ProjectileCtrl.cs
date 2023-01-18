using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected Collider2D hit;
    [SerializeField]
    protected float damage = 10f;

    protected int Layer;
    //private Collider2D[] colliders = new Collider2D[10];
    protected RaycastHit2D[] rchs = new RaycastHit2D[20];
    protected ContactFilter2D filter;
    protected float lifetime = -100f;
    protected Functions.Function<GameObject,float> damageSource = (obj) => 0;
    protected Vector3 lastpos;

    private void Awake()
    {
        damageSource = (obj) => damage;
        filter.useLayerMask=true;
        Layer = LayerMask.GetMask("World");
    }

    void Start()
    {
        lastpos = gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        if (HitCheck() || (lifetime <= 0f && lifetime != -100f))
        {
            foreach (RaycastHit2D rch in rchs)
            {
                if (!(rch && !rch.collider.isTrigger)) continue;
                IhasTag tag = rch.collider.gameObject.GetComponent<IhasTag>();
                if (tag != null)
                {
                    if (tag.hasVar("float", "_health"))
                    {
                        float h = tag.getFloat("_health");
                        float d = damageSource(gameObject);
                        h = h - d >= 0 ? h - d : 0;
                        tag.putFloat("_health", h);
                        break;
                    }
                }
            }
            GameObject.Destroy(gameObject, 0f);
        }
        lifetime = Timer.Update(lifetime, Time.deltaTime);
        lastpos = gameObject.transform.position;
    }
    public void Shoot(Vector3 velocity, int layer, float damage, float lifetime)
    {
        this.lifetime = lifetime;
        this.damage = damage;
        Layer = layer;
        Shoot(velocity);
    }

    public void Shoot(Vector3 velocity, int layer, float lifetime)
    {
        this.lifetime = lifetime;
        Layer = layer;
        Shoot(velocity);
    }

    public void Shoot(Vector3 velocity)
    {
        rb.velocity = velocity;
        lastpos = gameObject.transform.position;
    }

    protected bool HitCheck()
    {
        filter.layerMask = Layer;
        return hit.Cast(rb.velocity, filter, rchs,-Vector2.Distance(lastpos,gameObject.transform.position)) > 0;
    }
}
