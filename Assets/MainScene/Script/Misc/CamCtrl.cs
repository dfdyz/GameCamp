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

    void Update()
    {
        float dt = Time.deltaTime;
        if (PLAYERTAG.getBool("RUN"))
        {
            if(run_fov - cam.m_Lens.OrthographicSize > fov_speed * dt) cam.m_Lens.OrthographicSize += fov_speed * dt;
            else cam.m_Lens.OrthographicSize = run_fov;
        }
        else
        {
            if (cam.m_Lens.OrthographicSize-walk_fov > fov_speed * dt) cam.m_Lens.OrthographicSize -= fov_speed * dt;
            else cam.m_Lens.OrthographicSize = walk_fov;
        }
    }
}
