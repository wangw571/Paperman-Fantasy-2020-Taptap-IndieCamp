using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 时间控制器 : MonoBehaviour
{
    public float 完整时间;
    public float 剩余时间;
    public 玩家控制器 playerController;
    public GameObject EndText;
    public GameObject 重生键;
    public Text AddTimeText;
    public Text LostTimeText;

    public float TipDisplayTime = 3;

    private float LastShowTimeAdd = 0;
    private float AddedTime = 0;
    private float LastShowTimeLost = 0;
    private float LostTime = 0;


    public float getLastShowTimeAdd() { return LastShowTimeAdd; }
    public Text displayText;

    private bool gameEnded = false;

    private 简单动画器 AddAnimate;
    private 简单动画器 LostAnimate;

    private Vector3 worldPos;

    public Vector2 thisPos;

    private void Start()
    {
        AddAnimate = AddTimeText.gameObject.GetComponent<简单动画器>();
        LostAnimate = LostTimeText.gameObject.GetComponent<简单动画器>();
        worldPos = Camera.main.ScreenToWorldPoint(thisPos);
    }
    void Update()
    {
        剩余时间 = Mathf.Clamp(剩余时间, 0, 完整时间);
        worldPos = Camera.main.ScreenToWorldPoint(thisPos);
        worldPos = new Vector3(worldPos.x, worldPos.y, -5);
        剩余时间 -= Time.deltaTime;
        剩余时间 = Mathf.Clamp(剩余时间, 0, 99999999);
        LastShowTimeAdd -= Time.deltaTime;
        LastShowTimeLost -= Time.deltaTime;
        displayText.text = 剩余时间.ToString("F2");
        playerController.ColorLerpValue = 剩余时间 / 完整时间;
        if(剩余时间 <= 0)
        {
            endGame("超时");
        }

        if(LastShowTimeAdd > 0)
        {
            if (AddTimeText.text == "") { AddTimeText.text = "0"; }
            float AddTimeTemp = float.Parse(AddTimeText.text);
            AddTimeText.text = "+" + Mathf.Lerp(AddTimeTemp, AddedTime, 0.08f).ToString("F2");
            //AddTimeText.text = "+" + AddedTime.ToString("F2");
            AddAnimate.go();
        }
        else if (LastShowTimeAdd < -0.1f)
        {
            AddTimeText.text = "";
            AddedTime = 0;
        }

        if (LastShowTimeLost > 0)
        {
            if (LostTimeText.text == "") { LostTimeText.text = "0"; }
            float LostTimeTemp = float.Parse(LostTimeText.text);
            LostTimeText.text = Mathf.Lerp(LostTimeTemp, LostTime, 0.08f).ToString("F2");
            LostAnimate.go();
        }
        else if(LastShowTimeLost < -0.1f)
        {
            LostTimeText.text = "";
            LostTime = 0;
        }
    }

    public void AddTime(float timeAdded=1)
    {
        剩余时间 += timeAdded;
        float sizePerBall = 0.1f;
        float DistributionValue = 15 * Mathf.Abs(timeAdded);
        if(timeAdded >= 0)
        {
            LastShowTimeAdd = TipDisplayTime;
            AddedTime += timeAdded;
            while (timeAdded > 0)
            {
                GameObject newBall = h.GenerateBall(playerController.transform.position, Color.black, true, sizePerBall, 0.6f);

                墨水球动画器 animator = newBall.AddComponent<墨水球动画器>();
                newBall.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                animator.ToPosGameObj = this.gameObject;
                animator.ToScale = Vector3.one * sizePerBall * 4;
                animator.Interpolate = 0.1f;
                animator.Interpolate2 = 0.5f;
                animator.ToPositionRandomShift = new Vector3(Random.Range(-DistributionValue, DistributionValue), Random.Range(-DistributionValue, DistributionValue), 0);
                timeAdded -= sizePerBall;
            }

        }
        else
        {
            LastShowTimeLost = TipDisplayTime;
            LostTime += timeAdded;
            timeAdded = -timeAdded;
            while (timeAdded > 0)
            {
                GameObject newBall = h.GenerateBall(this.gameObject.transform.position, Color.black, true, sizePerBall, 0.6f);
                newBall.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                墨水球动画器 animator = newBall.AddComponent<墨水球动画器>();
                animator.ToPosGameObj = playerController.gameObject;
                animator.ToScale = Vector3.one * sizePerBall * 4;
                animator.Interpolate = 0.1f;
                animator.Interpolate2 = 0.5f;
                animator.ToPositionRandomShift = new Vector3(Random.Range(-DistributionValue, DistributionValue), Random.Range(-DistributionValue, DistributionValue), 0);
                timeAdded -= sizePerBall;
            }
        }
    }
    public void endGame(string deathReason)
    {
        gameEnded = true;
        //TODO deathReason
        EndText.SetActive(true);
        重生键.SetActive(true);
        EndText.GetComponent<Text>().text = deathReason;
        Time.timeScale = 0;

        if (重生键.gameObject.GetComponent<AudioSource>())
        {
            重生键.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public bool GameEnded()
    {
        return gameEnded;
    }
}
