using Assets.Global.Scrpits;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Utils/IPhysics")]
public class IPhysics : MonoBehaviour
{
    [Header(header: "Physics Instance")]
#pragma warning disable CS0108 // ��Ա���ؼ̳еĳ�Ա��ȱ�ٹؼ��� new
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // ��Ա���ؼ̳еĳ�Ա��ȱ�ٹؼ��� new

    [Header(header: "Physics Arguments")]
    [SerializeField]
    private AnimationCurve Gravity;
    [SerializeField]
    private float GravityScale = 2f;
    [SerializeField]
    private float VelocityScale = 0.78f;

    private Functions.Function<float,float> GravityModifier = (g)=>g;
    private Vector2 VelocityOverrideV = new Vector2(0f,0f);
    private Vector2 v;

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime;
        rigidbody.gravityScale = getGravity();
        v = rigidbody.velocity;
        if (VelocityOverrideV.y > 0f)
        {
            v.y = VelocityOverrideV.x;
            VelocityOverrideV.y -= dt;
            if (VelocityOverrideV.y < 0) VelocityOverrideV.y = 0;
        }
        else if(VelocityOverrideV.x < 0)
        {
            v.y = VelocityOverrideV.x;
            VelocityOverrideV.y = 0;
        }
        rigidbody.velocity = v;
    }

    public float getGravity()
    {
        return GravityModifier(getGravityRaw());
    }

    public float getGravityRaw()
    {
        return Gravity.Evaluate(rigidbody.velocity.y * VelocityScale) * GravityScale;
    }

    public void setGravityModifier(Functions.Function<float,float> function)
    {
        GravityModifier = function;
    }

    /// <summary>
    /// ���ô�ֱ�ٶȸ�д
    /// </summary>
    /// <param name="v">�ٶ�</param>
    /// <param name="time">ʱ��(��С��0������������)</param>
    public void setVelocityOverrideV(float v, float time)
    {
        VelocityOverrideV.x = v;
        VelocityOverrideV.y = time;
    }

    public Vector2 getVelocity()
    {
        return v;
    }

    public void setVelocityH(float v)
    {
        Vector3 vvv = rigidbody.velocity;
        vvv.x = v;
        rigidbody.velocity = vvv;
    }
}
