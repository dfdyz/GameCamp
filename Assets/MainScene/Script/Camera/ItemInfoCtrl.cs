using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject root;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text description;
    [SerializeField]
    private RawImage Icon;

    private float timer = 0f;
    private bool show = false;

    void Update()
    {
        root.SetActive(show);
        if (timer <= 0f && Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 1f;
            show = false;
        }
        timer = Timer.Update(timer,Time.unscaledDeltaTime);
    }

    public void Show(ItemCtrl item)
    {
        Time.timeScale = 0f;
        show = true;
        title.text = item.name;
        description.text = item.description;
        Icon.texture = item.Icon;
        timer = 0.2f;
    }
    public void Close()
    {
        Time.timeScale = 1f;
        show = false;
        root.SetActive(show);
    }

}
