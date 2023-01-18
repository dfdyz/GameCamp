using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreakBlock : MonoBehaviour
{
    [SerializeField]
    private IhasTag tags;
    void Update()
    {
        if (tags.getFloat("_fire") <= 0)
        {
            GameObject.Destroy(gameObject, 0f);
        }
    }
}
