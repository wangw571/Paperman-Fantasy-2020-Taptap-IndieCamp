using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 相机控制器 : MonoBehaviour
{
    public float 跟随高度Max = 15;
    public float 跟随高度Min = 8;
    private float followH;
    public float 固定高度 = 0;
    public bool 固定相机高度 = false;
    public 玩家控制器 playerController;


    public float minSize = 10;
    public float maxSize = 20;
    public float speedRatio = 10;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float calcSpeedRatio = playerController.myRigidbody.velocity.magnitude / speedRatio;
        if (!固定相机高度)
        {
            float CurrentFollowH = Mathf.Lerp(跟随高度Min, 跟随高度Max, calcSpeedRatio);
            followH = Mathf.Lerp(followH, CurrentFollowH, 0.003f);
            this.transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y + followH, -30);
        }
        else
        {
            this.transform.position = new Vector3(playerController.transform.position.x, 固定高度, -30);
        }

        if (Camera.main.orthographic)
        {
            float toLerp = Mathf.Lerp(minSize, maxSize, calcSpeedRatio);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, toLerp, 0.003f);
        }

    }
    private void FixedUpdate()
    {
        Update();
    }
}
