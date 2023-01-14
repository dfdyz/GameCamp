using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBarCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private RectTransform mask;
    [SerializeField]
    private GameObject sp;
    [SerializeField]
    private float RollingSpeed = 1f;
    [SerializeField]
    private float SPRate = 0f;
    private float realRate = 0f;
    
    private float org = 0f;
    private Vector3 v;
    void Start()
    {
        v = sp.transform.localPosition;
        org = v.x;
    }

    // Update is called once per frame
    void Update()
    {
        float R = RollingSpeed*Time.unscaledDeltaTime;
        if(Mathf.Abs(SPRate-realRate) > R)
        {
            realRate += (SPRate - realRate > 0 ? 1f : -1f) * R;
        }
        else
        {
            realRate = SPRate; 
        }
        v.x = org - mask.rect.width*Mathf.Clamp(1- realRate, 0f,1f);
        sp.transform.localPosition = v;
    }

    public void setSPRate(float r)
    {
        SPRate = Mathf.Clamp(r, 0f, 1f);
    }

}
