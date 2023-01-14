using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerEffCtrl : MonoBehaviour
{
    [SerializeField]
    private RawImage v1;
    [SerializeField]
    private RawImage v2;
    [SerializeField]
    private HealthBarCtrl hpSource;
    [SerializeField]
    private float speed = 2f;

    private Color col = Color.white;

    void Start()
    {
        col.a = 0f;
    }

    void Update()
    {
        float dt = Time.unscaledDeltaTime;
        float hpr = hpSource.getHPRate();
        if (hpr <= 0.3f)
        {
            col.a = Mathf.Clamp01(col.a+dt*speed);
        }
        else
        {
            col.a = Mathf.Clamp01(col.a-dt*speed);
        }
        v1.color = col;
        v2.color = col;
    }



}
