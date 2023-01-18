using Assets.Global.Scrpits;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject FloatText;
    [SerializeField]
    private Collider2D sensor;
    [SerializeField]
    private GameObject TargetPos;
    [SerializeField]
    private MaskCtrl mc;
    [SerializeField]
    private GameObject field;
    private Collider2D[] c = new Collider2D[4];
    private float timer = 0;
    private bool tping = false;

    // Update is called once per frame
    void Update()
    {
        timer = Timer.Update(timer,Time.deltaTime);
        if (timer <= 0) 
        {
            timer = 0.3f;
            FloatText.SetActive( Hit.Overlap(sensor, LayerMask.GetMask("Player"), c) > 0 );
        }
        if (FloatText.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            if (!tping)
            {
                if (Hit.Overlap(sensor, LayerMask.GetMask("Player"), c) > 0)
                {
                    StartCoroutine(TP(c[0].gameObject));
                }
            }
        }

    }

    IEnumerator TP(GameObject p)
    {
        tping = true;
        mc.setTrans(true, 0.3f);
        ResetMob();
        yield return new WaitForSeconds(0.3f);
        p.GetComponent<PlayerCtrl>().TP(TargetPos.transform.position);
        yield return new WaitForSeconds(0.5f);
        mc.setTrans(false,0.2f);
        yield return new WaitForSeconds(0.2f);
        tping = false;
    }

    private void ResetMob()
    {
        if (!field) return;
        GameObject[] obj = GameObject.FindGameObjectsWithTag("field_entity");

        GameObject fieldInstance = null;
        foreach (GameObject o in obj)
        {
            if (o.name == field.name) { fieldInstance = o; break; }
        }
        if (!fieldInstance) return;
        GameObject.Destroy(fieldInstance);
        fieldInstance = GameObject.Instantiate<GameObject>(field, Vector3.zero, Quaternion.identity);
        fieldInstance.name = field.name;
        fieldInstance.tag = "field_entity";
        fieldInstance.SetActive(true);
    }

}
