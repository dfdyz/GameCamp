using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ExplodeProjectileCtrl : ProjectileCtrl
{
    [SerializeField]
    protected Collider2D expfield;
    protected Collider2D[] cld = new Collider2D[35];

    private void Awake()
    {
        damageSource = (obj) => damage;
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
}
