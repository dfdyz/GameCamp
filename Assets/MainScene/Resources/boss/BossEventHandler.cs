using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossCtrl;

public class BossEventHandler : MonoBehaviour
{
    [SerializeField]
    private BossCtrl bossCtrl;

    public void Atk1()
    {
        bossCtrl.Atk1();
    }

    public void Atk2()
    {
        bossCtrl.Atk2();
    }

    public void Atk3()
    {
        bossCtrl.Atk3();
    }

    public void ChangeState(BossState state)
    {
        bossCtrl.state = state;
        bossCtrl.atkPrevDelayTimer = bossCtrl.atkPrevDelay;
    }

    public void SetIdle()
    {
        bossCtrl.atkPrevDelayTimer = bossCtrl.atkPrevDelay * 3;
        bossCtrl.animator.SetBool("Idle",false);
    }
}
