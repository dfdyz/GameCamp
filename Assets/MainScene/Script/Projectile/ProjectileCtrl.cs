using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ProjectileCtrl : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Collider2D collider;

    private int Layer;
    //private Collider2D[] colliders = new Collider2D[10];
    private RaycastHit2D[] rchs = new RaycastHit2D[20];
    private ContactFilter2D filter;
    private float lifetime = -100f;
    private Functions.FunctionF<GameObject> damageSource = (obj) => 10;
    private Vector3 lastpos;

    private void Awake()
    {
        filter.useLayerMask=true;
        Layer = LayerMask.GetMask("World");
    }

    void Start()
    {
        lastpos = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        if (HitCheck() || (lifetime <= 0f && lifetime != -100f))
        {
            foreach(RaycastHit2D rch in rchs)
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
            GameObject.Destroy(gameObject,0f);
        }
        lifetime = Timer.Update(lifetime,Time.deltaTime);
        lastpos = gameObject.transform.position;
    }
    public void Shoot(Vector3 velocity,int layer,float lifetime)
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

    private bool HitCheck()
    {
        filter.layerMask = Layer;
        return collider.Cast(rb.velocity, filter, rchs,-Vector2.Distance(lastpos,gameObject.transform.position)) > 0;
    }
}
