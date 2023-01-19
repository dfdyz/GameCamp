using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    [SerializeField]
    private IhasTag PLAYERTAG;
    [SerializeField]
    private CinemachineVirtualCamera cam;
    [SerializeField]
    private float walk_fov = 6.38f;
    [SerializeField]
    private float run_fov = 7.5f;
    [SerializeField]
    private float fov_speed = 16.0f;
    [SerializeField]
    private GameObject forest;
    [SerializeField]
    private GameObject cave;

    private float mul = 1f;

    void Update()
    {
        float dt = Time.deltaTime;
        float target = (PLAYERTAG.getBool("RUN") ? run_fov : walk_fov) * mul;

        if (Mathf.Abs(target * mul - cam.m_Lens.OrthographicSize) > fov_speed * dt) cam.m_Lens.OrthographicSize += (target * mul - cam.m_Lens.OrthographicSize > 0 ? 1:-1) * fov_speed * dt;
        else cam.m_Lens.OrthographicSize = target * mul;

        switch (PLAYERTAG.getString("Field"))
        {
            case "Field1":
                forest.SetActive(false);
                cave.SetActive(true);
                break;
            case "Field2":
            case "Field3":
                forest.SetActive(true);
                cave.SetActive(false);
                break;
        }

    }

    public void setMul(float mul)
    {
        this.mul = mul;
    }


}
