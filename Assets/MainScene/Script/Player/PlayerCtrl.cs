using Assets.Global.Scrpits;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header(header: "Instance")]
    public IPhysics physics;
    public IhasTag ITag;
    [Header(header: "SensorField")]
    [SerializeField]
    private HitField GroundSensor;

    [Header(header: "Arguments")]
    [SerializeField]
    private float WalkSpeed = 10.0f;
    [SerializeField]
    private float WalkAcceleration = 100.0f;

    [SerializeField]
    private float JumpSpeed = 25f;
    [SerializeField]
    private float JumpSpeedTime = 0.02f;
    [SerializeField]
    private float JumpKeyBuffertime = 0.2f;
    [SerializeField]
    private float JumpKeyPressTime = 0.05f;
    [SerializeField]
    private float MaxGroundBuffer = 0.08f;

    #region Var
    private Collider2D[] colliderHited = new Collider2D[10];
    private float CurrentSpeedH = 0f;
    private float JumpKeyBuffer = 0f;
    private float JumpKeyPress = 0f;
    private float GroundBuffer = 0f;
    private Vector3 lastpos;
    #endregion

    void Start()
    {
        lastpos = gameObject.transform.position;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        bool onGround = groundCheck();
        float AxisH = Input.GetAxisRaw("Horizontal");
        //int AxisV = (int)Input.GetAxisRaw("Vertical");

        if (onGround)
        {
            float targetSpeed = AxisH*WalkSpeed;
            if (AxisH == 0f)
            {
                targetSpeed = 0f;
            }

            if (CurrentSpeedH * AxisH < 0) CurrentSpeedH = 0;
            if (Mathf.Abs(targetSpeed - CurrentSpeedH) < WalkAcceleration * dt)
            {
                CurrentSpeedH = targetSpeed;
            }
            else
            {
                CurrentSpeedH += (targetSpeed - CurrentSpeedH > 0 ? 1f : -1f) * dt * WalkAcceleration;
            }
            physics.setVelocityH(CurrentSpeedH);

            GroundBuffer = MaxGroundBuffer;
        }

        //Jump Key handle
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpKeyBuffer = JumpKeyBuffertime;
        }
        if (Input.GetKey(KeyCode.Space) && JumpKeyBuffer==-1f)
        {
            if(JumpKeyPress <= JumpKeyPressTime) JumpKeyPress += dt;
        }
        else
        {
            JumpKeyPress = 0;
        }

        //Jump
        if (JumpKeyBuffer > 0  && GroundBuffer > 0 && physics.getVelocity().y <= 0.01f)
        {
            JumpKeyBuffer = -1f;
            physics.setVelocityOverrideV(JumpSpeed, JumpSpeedTime);
        }

        if(JumpKeyPress > 0 && JumpKeyPress<= JumpKeyPressTime)
        {
            physics.setVelocityOverrideV(JumpSpeed, JumpSpeedTime);
        }


        //handle buffer;
        JumpKeyBuffer = Timer.Update(JumpKeyBuffer, dt);
        GroundBuffer = Timer.Update(GroundBuffer, dt);

        float stun = ITag.getFloat("stun");
        if (stun <= 0f)
        {
            if(CurrentSpeedH == 0)
            {
                Vector3 p = gameObject.transform.position;
                p.x = lastpos.x;
                gameObject.transform.position = p;
            }
        }
        lastpos = gameObject.transform.position;
        ITag.putFloat("stun", Timer.Update(stun, dt));
        


    }

    private bool groundCheck()
    {
        return GroundSensor.OverlapNonAlloc(LayerMask.GetMask("World"),colliderHited) > 0;
    }



    void FixedUpdate()
    {
        
    }
}
