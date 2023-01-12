using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskCtrl : MonoBehaviour
{
    [SerializeField]
    private RawImage img;

    private float timer = 0f;
    private float mt = 1f;
    private bool state = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color col = img.color;
        if (timer <= 0)
        {
            col.a = state ? 1f:0f;
        }
        else
        {
            col.a = state ? (1-timer / mt) : (timer / mt);
        }
        img.color = col;
        timer = Timer.Update(timer,Time.deltaTime);
    }

    public void setTrans(bool state,float time)
    {
        timer = time;
        mt = time;
        this.state = state;
    }


}
