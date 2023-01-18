using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFireBall : ExplodeProjectileCtrl
{
    private void Awake()
    {
        damageSource = (obj) => damage;
        expfield.enabled = false;
        filter.useLayerMask = true;
        Layer = LayerMask.GetMask("World");
    }
    void Start()
    {
        lastpos = gameObject.transform.position;
    }
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
                    else if (tag.hasVar("float", "_fire"))
                    {
                        float h = tag.getFloat("_fire");
                        float d = damageSource(gameObject);
                        h = h - d >= 0 ? h - d : 0;
                        tag.putFloat("_fire", h);
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
