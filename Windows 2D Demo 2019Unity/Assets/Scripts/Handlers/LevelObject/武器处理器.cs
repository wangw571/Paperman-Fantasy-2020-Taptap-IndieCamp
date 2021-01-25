using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 武器处理器 : MonoBehaviour
{
    public int 武器代码;
    public InkHandler 我的墨水处理器;
    public 玩家控制器 玩家控制器;

    // Update is called once per frame
    void Update()
    {
        if(我的墨水处理器.AmountOfTime <= 0)
        {
            WeaponTag newWeaponTag = 玩家控制器.gameObject.AddComponent<WeaponTag>();
            newWeaponTag.WeaponType = 武器代码;
            Destroy(this);
        }
    }


}
