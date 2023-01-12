using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject hp_mid;
    [SerializeField]
    private GameObject hp_top;
    [SerializeField]
    private GameObject mp_mid;
    [SerializeField]
    private GameObject mp_top;
    [SerializeField]
    private float RollingDelay = 0.2f;
    [SerializeField]
    private float RollingSpeed = 2f;

    private float HpRollingTimer = 0f;
    private float MpRollingTimer = 0f;
    [SerializeField]
    private float HpRate = 0f;
    [SerializeField]
    private float MpRate = 0f;

    void Update()
    {
        float dt = Time.deltaTime;
        Vector3 v = hp_top.transform.localScale;
        v.x = HpRate;
        hp_top.transform.localScale = v;

        v = mp_top.transform.localScale;
        v.x = MpRate;
        mp_top.transform.localScale = v;

        float dr = RollingSpeed * dt;
        v = hp_mid.transform.localScale;
        if (v.x > HpRate)
        {
            if (HpRollingTimer <= 0)
            {
                v.x -= dr;
            }
        }
        else v.x = HpRate;
        hp_mid.transform.localScale = v;

        v = mp_mid.transform.localScale;
        if (v.x > MpRate)
        {
            if (MpRollingTimer <= 0)
            {
                v.x -= dr;
            }
        }
        else v.x = MpRate;
        mp_mid.transform.localScale = v;

        HpRollingTimer = Timer.Update(HpRollingTimer, dt);
        MpRollingTimer = Timer.Update(MpRollingTimer, dt);
    }

    public void setHPRate(float r)
    {
        if (HpRate <= r && r >= hp_mid.transform.localScale.x)
        {
            HpRollingTimer = RollingDelay;
        }
        HpRate = Mathf.Clamp(r, 0f, 1f);
    }

    public void setMPRate(float r)
    {
        if(MpRate <= r && r >= mp_mid.transform.localScale.x)
        {
            MpRollingTimer = RollingDelay;
        }
        MpRate = Mathf.Clamp(r, 0f,1f);
    }

}
