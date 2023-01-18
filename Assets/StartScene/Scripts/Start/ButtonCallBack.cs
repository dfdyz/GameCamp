using Assets.Global.Scrpits;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonCallBack : MonoBehaviour
{
    [SerializeField]
    Button saves;

    private void Start()
    {
        saves.gameObject.SetActive(Directory.Exists(Application.dataPath + "/save"));
    }

    public void Botton_Start()
    {
        SavesMgr.StartSave = false;
        SceneManager.LoadScene("Main");
    }

    public void Botton_Saves()
    {
        SavesMgr.StartSave = true;
        SceneManager.LoadScene("Main");
    }

    public void Botton_Exit()
    {
        Application.Quit();
    }

}
