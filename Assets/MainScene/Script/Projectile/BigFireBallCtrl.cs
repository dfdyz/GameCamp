using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BigFireBallCtrl : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Collider2D hitfield;
    [SerializeField]
    private Collider2D expfield;

    private RaycastHit2D[] rchs = new RaycastHit2D[20];
    private Collider2D[] cld = new Collider2D[35];
    private int Layer;
    private float lifetime = -100f;
    private ContactFilter2D filter;
    private Functions.FunctionF<GameObject> damageSource = (obj) => 50;
    private Vector3 lastpos;

    private void Awake()
    {
        expfield.enabled = false;
        filter.useLayerMask = true;
        Layer = LayerMask.GetMask("World");
    }

    // Start is called before the first frame update
    void Start()
    {
        lastpos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (HitCheck() || (lifetime <= 0f && lifetime != -100f))
        {
            expfield.enabled = true;
            Hit.Overlap(expfield, Layer, cld);
            Hashtable atked = new Hashtable();
            foreach (Collider2D c in cld)
            {
                if (!(c && !c.isTrigger)) continue;
                IhasTag tag = c.gameObject.GetComponent<IhasTag>();
                if (tag != null && atked[c.gameObject] == null)
                {
                    if (tag.hasVar("float", "_health"))
                    {
                        float h = tag.getFloat("_health");
                        float d = damageSource(gameObject);
                        h = h - d >= 0 ? h - d : 0;
                        tag.putFloat("_health", h);
                        atked[c.gameObject] = true;
                    }
                }
            }
            GameObject.Destroy(gameObject, 0f);
        }
        lifetime = Timer.Update(lifetime, Time.deltaTime);
        lastpos = gameObject.transform.position;
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
    private bool HitCheck()
    {
        filter.layerMask = Layer;
        return hitfield.Cast(rb.velocity, filter, rchs, -Vector2.Distance(lastpos, gameObject.transform.position)) > 0;
    }
}
