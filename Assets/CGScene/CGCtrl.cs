using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CGCtrl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public void Start()
    {
        animator.SetBool("CG", true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

}
