using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 怪物纹理动画器 : MonoBehaviour
{
    public Texture 待机满;
    public float 待机滚动速度;
    public int 待机帧数;
    public Texture 待机半;
    //public float 待机半滚动速度;
    //public int 待机半帧数;
    public Texture 待机35;
    //public float 待机35滚动速度;
    //public int 待机35帧数;
    public Texture 跑步;
    public float 跑步滚动速度;
    public int 跑步帧数;
    public Texture 跳跃满;
    public Texture 跳跃半;
    public Texture 跳跃35;
    public float 跳跃滚动速度;
    public int 跳跃帧数;
    public Texture 抓取;
    public float 抓取滚动速度;
    public int 抓取帧数;
    public Texture 藤蔓攀爬;
    public float 藤蔓滚动速度;
    public int 藤蔓帧数;
    public Texture 吸取;
    public float 吸取滚动速度;
    public int 吸取帧数;

    public Texture 纸飞机;
    public float 纸飞机滚动速度;
    public int 纸飞机帧数;

    public Texture 铁坨;
    public float 铁坨滚动速度;
    public int 铁坨帧数;

    public 脚处理器 jio;
    public 玩家控制器 玩家;
    public InkSuckController 吸墨控制器;
    public Renderer 玩家图像;

    private Material mat;

    private float offsetX = 0.0f;
    private float offsetY = 0.0f;

    private float scrollSpeed;
    private float countX;
    private float countY = 1;

    public float localTime;

    private void Start()
    {
        mat = 玩家图像.material;
        localTime = 0;
    }

    public bool 朝向右方 = true;
    void Update()
    {
        localTime += Time.deltaTime;
        if (朝向右方)
        {
            if (玩家.myRigidbody.velocity.x < -0.2f)
            {
                朝向右方 = false;
                玩家图像.transform.localScale = new Vector3(-1 * Mathf.Abs(玩家图像.transform.localScale.x), 玩家图像.transform.localScale.y, 玩家图像.transform.localScale.z);
            }
        }
        else
        {
            if (玩家.myRigidbody.velocity.x > 0.2f)
            {
                朝向右方 = true;
                玩家图像.transform.localScale = new Vector3(Mathf.Abs(玩家图像.transform.localScale.x), 玩家图像.transform.localScale.y, 玩家图像.transform.localScale.z);
            }
        }

        if (jio.getNotOnFeet())
        {
            if (玩家.InPlaneMode)
            {
                if (UpdateTex(纸飞机, 纸飞机帧数))
                {
                    localTime = 0;
                }
                mat.mainTexture = 纸飞机;
                scrollSpeed = 纸飞机滚动速度;
                Draw(Mathf.Floor(localTime * scrollSpeed));
            }
            else
            {
                float 时间比 = (玩家.timeController.剩余时间 / 玩家.timeController.完整时间);
                Texture 真跳跃纹理 = 时间比 > 0.7f ? 跳跃满 : (时间比 > 0.35f ? 跳跃半 : 跳跃35);
                if (UpdateTex(真跳跃纹理, 跳跃帧数))
                {
                    localTime = 0;
                }
                mat.mainTexture = 真跳跃纹理;
                scrollSpeed = 跳跃滚动速度;
                if (玩家.myRigidbody.velocity.y > 0)
                {
                    Draw(0);
                }
                else
                {
                    Draw(1);
                }
            }
        }
        else
        {
            if (Mathf.Abs(玩家.myRigidbody.velocity.x) > 0.5f)
            {
                if (UpdateTex(跑步, 跑步帧数))
                {
                    localTime = 0;
                }
                scrollSpeed = 跑步滚动速度;
                Draw(Mathf.Floor(localTime * scrollSpeed));
            }
            else
            {

                if (吸墨控制器.吸墨中)
                {
                    float emm = (4-1) / scrollSpeed;
                    if (UpdateTex(吸取, 吸取帧数) || localTime > emm)
                    {
                        localTime = 0;
                    }
                    scrollSpeed = 吸取滚动速度;
                    localTime = Mathf.Clamp(localTime, 0, emm);
                    float Progress = Mathf.Floor(localTime * scrollSpeed);
                    Draw(Progress);
                }
                else
                {
                    if (mat.mainTexture == 吸取)
                    {
                        float Progress = Mathf.Floor(localTime * scrollSpeed);
                        Draw(Progress);
                        if (Progress > 吸取帧数 - 1)
                        {
                            UpdateTex(待机满, 待机帧数);
                            localTime = 0;
                        }
                    }
                    else
                    {
                        float 时间比 = (玩家.timeController.剩余时间 / 玩家.timeController.完整时间);
                        Texture 真待机纹理 = 时间比 > 0.7f ? 待机满 : (时间比 > 0.35f ? 待机半 : 待机35);
                        if (UpdateTex(真待机纹理, 待机帧数))
                        {
                            localTime = 0;
                        }
                        scrollSpeed = 待机滚动速度;
                        Draw(Mathf.Floor(localTime * scrollSpeed));
                    }
                }
            }
        }

    }

    void Draw(float frame)
    {
        //var frame = Mathf.Floor(Time.time * scrollSpeed);
        offsetX = frame / countX;
        //offsetY = -(frame - frame % countX) / countY / countX;
        offsetY = 0;
        mat.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }

    bool UpdateTex(Texture New, int CountX)
    {
        countX = CountX;
        Vector2 singleTexSize = new Vector2(1.0f / countX, 1.0f / countY);
        bool equalsss = mat.mainTexture != New;
        if (equalsss)
        {
            mat.mainTexture = New;
        }
        mat.mainTextureScale = singleTexSize;
        return equalsss;
    }
}
