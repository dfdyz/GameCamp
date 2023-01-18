using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMgrCtrl : MonoBehaviour
{
    [SerializeField]
    private IhasTag PlayerTag;
    [SerializeField]
    private PlayerCtrl pc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKill(string type)
    {
        PlayerTag.putFloat("_soul", Mathf.Clamp(PlayerTag.getFloat("_soul")+1f,0,pc.spMax));
        PlayerTag.putInt(type, PlayerTag.getInt(type)+1);
    }

    public int getKill(string type) {
        return PlayerTag.getInt(type);
    }
}
