using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitField : MonoBehaviour
{
    public int OverlapNonAlloc(int mask,Collider2D[] res)
    {
        //print(res.Length);
        return Physics2D.OverlapBoxNonAlloc(gameObject.transform.position, gameObject.transform.lossyScale / 2, gameObject.transform.eulerAngles.z, res, mask); ;
    }

    public Collider2D[] Overlap(int mask)
    {
        return Physics2D.OverlapBoxAll(gameObject.transform.position, gameObject.transform.lossyScale / 2, gameObject.transform.eulerAngles.z, mask);
    }
}
