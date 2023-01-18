using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject FloatText;
    [SerializeField]
    private Collider2D sensor;
    [SerializeField]
    private SaveCtrl saveCtrl;
    [SerializeField]
    private MaskCtrl mc;
    [SerializeField]
    private IhasTag player;
    [SerializeField]
    private GameObject visual;

    private Collider2D[] c = new Collider2D[4];
    private float timer = 0;
    private bool tping = false;

    void Update()
    {
        bool enable = player.getBool("EnableSave");
        visual.SetActive(enable);
        sensor.gameObject.SetActive(enable);
        timer = Timer.Update(timer, Time.deltaTime);
        if (enable && timer <= 0)
        {
            timer = 0.3f;
            FloatText.SetActive(Hit.Overlap(sensor, LayerMask.GetMask("Player"), c) > 0);
        }
        if (FloatText.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            if (!tping)
            {
                if (Hit.Overlap(sensor, LayerMask.GetMask("Player"), c) > 0)
                {
                    StartCoroutine(Save());
                }
            }
        }
    }
    IEnumerator Save()
    {
        tping = true;
        mc.setTrans(true, 0.3f);
        yield return new WaitForSeconds(0.3f);
        saveCtrl.Save();
        yield return new WaitForSeconds(0.5f);
        mc.setTrans(false, 0.2f);
        yield return new WaitForSeconds(0.2f);
        tping = false;
    }
}
