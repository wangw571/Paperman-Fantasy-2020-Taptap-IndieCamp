using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public SceneController 场景控制器;
    public SaveDataContainer DataLoaded;
    private bool MovePlayer = false;

    void Start()
    {
        DataLoaded = SaveDataHelper.LoadByJson(0);
        if (DataLoaded == null) return;
        玩家控制器 player = FindObjectOfType<玩家控制器>();
        重生位置容器 container;
        foreach (重生位置容器 subcontainer in FindObjectsOfType<重生位置容器>())
        {
            if (subcontainer.场景名称 == DataLoaded.SceneName)
            {
                container = subcontainer;
                player.transform.position = container.重生位置集[DataLoaded.RespawnPosIndex];
                return;
            }
        }
    }
    void Update()
    {
        //SaveDataContainer temp = SaveDataHelper.LoadByJson(0);
        //h.l(temp.RespawnPosIndex);

    }
    void LateUpdate()
    {
        //if (MovePlayer)
        //{
        //    MovePlayer = false;
        //    玩家控制器 player = FindObjectOfType<玩家控制器>();
        //    重生位置容器 container;
        //    foreach (重生位置容器 subcontainer in FindObjectsOfType<重生位置容器>())
        //    {
        //        if (subcontainer.场景名称 == DataLoaded.SceneName)
        //        {
        //            container = subcontainer;
        //            player.transform.position = container.重生位置集[DataLoaded.RespawnPosIndex];
        //            return;
        //        }
        //    }
        //}
    }
    public void UpdateSave(SaveDataContainer newSaveData)
    {
        DataLoaded = newSaveData;
        try
        {
            SaveDataHelper.SaveByJson(DataLoaded, 0);
        }
         catch
        {

            //h.GenerateBall(Camera.main.ScreenToWorldPoint(new Vector3(0,0, 5)), Color.yellow, true, 5, 1f);
        }
    }
    public void UpdateSave(int 重生索引位置)
    {
        DataLoaded = new SaveDataContainer(SceneStaticController.SC.现在的场景名, 重生索引位置);
        try
        {
            SaveDataHelper.SaveByJson(DataLoaded, 0);
        }
        catch
        {
            //h.GenerateBall(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 5)), Color.yellow, true, 5, 1f);
        }
    }
    public void Respawn()
    {
        if(DataLoaded == null)
        {
            UpdateSave(0);
        }
        场景控制器.LoadTargetScene(DataLoaded.SceneName);
        MovePlayer = true;
    }
}
