using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 瞬间伤害处理器 : MonoBehaviour
{
    //public TimeController timeController;
    //public float FullAmountOfTime;
    //public float AmountOfTime;
    //public float TimePerAdd;
    //public GameObject EndText;
    public float 伤害值 = 20;
    public bool 弹开 = true;
    public float 弹开力度 = 40;

    public float 伤害冷却时间 = 1;
    private float tempCD = 1;
    public string 死亡原因;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tempCD -= Time.deltaTime;
    }

    public void OnCollisionEnter(Collision col)
    {
        玩家控制器 pc = col.gameObject.GetComponent<玩家控制器>();
        if (pc && tempCD <- 0)
        {
            pc.timeController.AddTime(-1 * 伤害值);
            tempCD = 伤害冷却时间;
            if (弹开)
            {
                pc.myRigidbody.AddForce((this.transform.position - pc.transform.position).normalized * 弹开力度);
            }
            if (pc.timeController.剩余时间 < 0)
            {
                pc.TriggerDeath(死亡原因);
            }
        }
    }


}
