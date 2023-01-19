using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class END : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(end());
    }

    IEnumerator end()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3);
        if(Directory.Exists(Application.dataPath + "/save")) Directory.Delete(Application.dataPath + "/save",true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }


}
