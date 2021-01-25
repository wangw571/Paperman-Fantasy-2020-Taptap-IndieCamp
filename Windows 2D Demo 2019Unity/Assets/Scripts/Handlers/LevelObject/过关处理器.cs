using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 过关处理器 : MonoBehaviour
{
    public 重生位置容器 本关重生位置容器;
    public string 次关名称;
    public TouchButtonHandler 下一关按钮;
    public Text 过关文字;

    public void OnTriggerEnter(Collider other)
    {
        //h.GenerateBall(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 5)), Color.black, true, 5, 1f);
        玩家控制器 pc = other.gameObject.GetComponent<玩家控制器>();
        if (pc)
        {
            本关重生位置容器.更新位置(次关名称, 0);
            下一关按钮.gameObject.SetActive(true);
        }
    }
}
