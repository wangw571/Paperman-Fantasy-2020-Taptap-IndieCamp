using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 重生位置容器 : MonoBehaviour
{
    public string 场景名称;
    public List<Vector3> 重生位置集;

    public void 重生()
    {
        RespawnController RC = FindObjectOfType<RespawnController>();
        RC.Respawn();
    }
    public void 更新位置(int 位置)
    {
        RespawnController RC = FindObjectOfType<RespawnController>();
        RC.UpdateSave(位置);
    }
    public void 更新位置(string 下一个关卡的名字, int 位置)
    {
        RespawnController RC = FindObjectOfType<RespawnController>();
        RC.UpdateSave(new SaveDataContainer(下一个关卡的名字, 位置));
    }
    public void Start()
    {
        RespawnController RC = FindObjectOfType<RespawnController>();
        //RC.UpdateSave(new SaveDataContainer(场景名称, RC.DataLoaded.RespawnPosIndex > 0 ? RC.DataLoaded.RespawnPosIndex : 0));
    }
}
