using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCallBack : MonoBehaviour
{
    public void Botton_Start()
    {
        SceneManager.LoadScene("Main");
    }

    public void Botton_Saves()
    {
        Debug.Log("click");
    }

    public void Botton_Exit()
    {
        Application.Quit();
    }

}
