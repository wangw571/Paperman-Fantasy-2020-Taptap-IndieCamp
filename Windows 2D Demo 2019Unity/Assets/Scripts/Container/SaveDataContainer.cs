using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class SaveDataContainer
{
    public string SceneName;
    public int RespawnPosIndex;
    public SaveDataContainer(string sceneName, int respawnIndex)
    {
        SceneName = sceneName;
        RespawnPosIndex = respawnIndex;
    }
}

public static class SaveDataHelper
{
    // 0是自动保存的位置
    // 剩下的都是玩家决定
    public static void SaveByJson(SaveDataContainer save, int SaveID)
    {
        string saveJsonStr = JsonConvert.SerializeObject(save);
        PlayerPrefs.SetString(SaveID.ToString(), saveJsonStr);
    }

    public static SaveDataContainer LoadByJson(int SaveID)
    {
        string saveJsonStr = PlayerPrefs.GetString(SaveID.ToString());
        return JsonConvert.DeserializeObject<SaveDataContainer>(saveJsonStr);
    }
}

