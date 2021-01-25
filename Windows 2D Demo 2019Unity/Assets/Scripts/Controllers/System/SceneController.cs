using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string 现在的场景名 = "SampleScene";
    // Start is called before the first frame update
    void LateUpdate()
    {
        if (SceneStaticController.SC == null) {
            DontDestroyOnLoad(this.gameObject);
            SceneStaticController.SC = this;
        }
        else if(SceneStaticController.SC != this)
        {
            Destroy(this.gameObject);
        }

    }

    public void LoadTargetScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        现在的场景名 = SceneName;
    }
    public void LoadTargetScene(Scene scene)
    {
        SceneManager.LoadScene(scene.name);
        现在的场景名 = scene.name;
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("Start");
        现在的场景名 = "Start";
    }
}

public static class SceneStaticController
{
    public static SceneController SC = null;
}
