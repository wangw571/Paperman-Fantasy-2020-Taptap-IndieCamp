using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 玩家移动速度乘子处理器 : MonoBehaviour
{
    public float 移动速度乘子;
    public float 跳跃力度乘子;

    public void OnTriggerStay(Collider col)
    {
        玩家控制器 pc = col.gameObject.GetComponent<玩家控制器>();
        if (pc)
        {
            pc.SetJumpSpeedMultiplier(跳跃力度乘子);
            pc.SetMoveSpeedMultiplier(移动速度乘子);
        }
    }

    void OnCollisionStay(Collision col)
    {
        玩家控制器 pc = col.gameObject.GetComponent<玩家控制器>();
        if (pc)
        {
            pc.SetJumpSpeedMultiplier(跳跃力度乘子);
            pc.SetMoveSpeedMultiplier(移动速度乘子);
        }
    }
}
