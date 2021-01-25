using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 钥匙处理器 : MonoBehaviour
{
    public string 钥匙代码;
    public InkHandler 我的墨水处理器;
    public 玩家控制器 玩家控制器;

    // Update is called once per frame
    void Update()
    {
        if(我的墨水处理器.AmountOfTime <= 0)
        {
            钥匙标签 newKeyTag = 玩家控制器.gameObject.AddComponent<钥匙标签>();
            newKeyTag.钥匙代码 = 钥匙代码;
            Destroy(this);
        }
    }


}
