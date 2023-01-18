using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventHandler : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void setJump(bool s)
    {
        animator.SetBool("jump", s);
    }

}
