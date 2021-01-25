using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 墨水区域处理器 : MonoBehaviour
{
    public string 死亡原因;
    public Renderer thisRenderer;
    public float DamagePerSecond = 3;
    // Start is called before the first frame update

    public void OnTriggerStay(Collider col)
    {
        玩家控制器 pc = col.gameObject.GetComponent<玩家控制器>();
        if (pc)
        {
            pc.timeController.AddTime(-1 * DamagePerSecond * Time.fixedDeltaTime);
            if (pc.timeController.剩余时间 < 0)
            {
                pc.TriggerDeath(死亡原因);
            }
        }
    }
}
