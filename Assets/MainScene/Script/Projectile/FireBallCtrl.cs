using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCtrl : ProjectileCtrl
{
    private void Awake()
    {
        damageSource = (obj) => damage;
        filter.useLayerMask = true;
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
                    else if (tag.hasVar("float", "_fire"))
                    {
                        float h = tag.getFloat("_fire");
                        float d = damageSource(gameObject);
                        h = h - d >= 0 ? h - d : 0;
                        tag.putFloat("_fire", h);
                        break;
                    }
                }
            }
            GameObject.Destroy(gameObject, 0f);
        }
        lifetime = Timer.Update(lifetime, Time.deltaTime);
        lastpos = gameObject.transform.position;
    }
}
