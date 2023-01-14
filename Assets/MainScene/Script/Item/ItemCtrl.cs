using Assets.Global.Scrpits;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject TEXT;
    [SerializeField]
    private Collider2D sensor;

    public Texture Icon;
    public string name = "Core";
    public string description = "   It's a Core.";

    private float timer = 0f;
    private Collider2D[] c = new Collider2D[3];
    private bool show = false;

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0f)
        {
            show = Hit.Overlap(sensor, LayerMask.GetMask("Player"), c) > 0;
            timer = 0.3f;
        }
        if (show && Input.GetKeyDown(KeyCode.F))
        {
            if (Hit.Overlap(sensor, LayerMask.GetMask("Player"), c) > 0)
            {
                ItemInfoCtrl ItemInfo = GameObject.Find("ItemInfo").GetComponent<ItemInfoCtrl>();
                ItemInfo.Show(this);
                c[0].gameObject.GetComponent<IhasTag>().putBool("hasItem_"+name, true);
                GameObject.Destroy(gameObject, 0f);
            }
        }

        TEXT.SetActive(show);

        timer = Timer.Update(timer, Time.deltaTime);
    }
}
