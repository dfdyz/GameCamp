using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveCtrl : MonoBehaviour
{
    [SerializeField]
    private ItemMgrCtrl mItemMgrCtrl;
    [SerializeField]
    private IhasTag player;
    [SerializeField]
    private IhasTag items;

    void Start()
    {
        if (SavesMgr.StartSave)
        {
            Read();
        }
        mItemMgrCtrl.Generate();
    }

    public void Save()
    {
        Directory.CreateDirectory(Application.dataPath +"/save");
        SavesMgr.saveStr("save/player", player.Serialize());
        SavesMgr.saveStr("save/items",items.Serialize());
    }
    public void Read()
    {
        player.Unserialize(SavesMgr.readStr("save/player"));
        player.putBool("EnableSave", SavesMgr.StartSave);
        items.Unserialize(SavesMgr.readStr("save/items"));
    }
}
