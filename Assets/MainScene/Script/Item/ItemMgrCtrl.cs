using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgrCtrl : MonoBehaviour
{
    [SerializeField]
    private IhasTag tags;
    [SerializeField]
    private ItemGenerator[] generators;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (ItemGenerator i in generators)
        {
            tags.putBool(i.gameObject.name,true);
        }
    }

    public void Generate()
    {
        foreach (ItemGenerator i in generators)
        {
            if (tags.getBool(i.gameObject.name))
            {
                i.Generate();
            }
            else
            {
                i.DestroyI();
            }
        }
    }

    public void ScanItem()
    {
        foreach (ItemGenerator i in generators)
        {
            if (tags.getBool(i.gameObject.name))
            {
                tags.putBool(i.gameObject.name, i.check());
            }
        }
    }
}
