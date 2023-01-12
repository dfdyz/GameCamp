using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<IhasTag>().getFloat("_health") <= 0)
        {
            GameObject.Destroy(gameObject,0f);
        }
    }
}
