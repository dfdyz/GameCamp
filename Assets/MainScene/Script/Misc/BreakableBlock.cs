using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField]
    private IhasTag tags;
    void Update()
    {
        if (tags.getFloat("_health") <= 0)
        {
            GameObject.Destroy(gameObject, 0f);
        }
    }
}
