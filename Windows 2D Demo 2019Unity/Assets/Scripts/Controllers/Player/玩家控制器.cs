using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class 玩家控制器 : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody myRigidbody;
    public 简单动画器 inkStrawAnimator;
    public 时间控制器 timeController;
    public 脚处理器 jio处理器;

    public float blockSize = 4;

    //public GameObject MoveLButtonGameObject;
    //public GameObject MoveRButtonGameObject;

    //private RectTransform MoveLButton;
    //private RectTransform MoveRButton;

    public float BaseMoveSpeed = 8.5f;
    public float BaseUpAcceleration = 20f;
    public float JumpInkConsumption = 3f;


    public Renderer thisRenderer;
    private Material thisMaterial;

    [HideInInspector]
    public float ColorLerpValue;


    private float TargetYScale = 1;
    private float MoveSpeedMultiplier = 1;
    private float JumpSpeedMultiplier = 1;

    public float 速度乘子恢复速度;

    [HideInInspector]
    public bool InPlaneMode;
    [HideInInspector]
    public bool InBallastMode;

    void Start()
    {
        thisMaterial = thisRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementDetectKeyboard();
        this.transform.eulerAngles = Vector3.zero;
        this.myRigidbody.angularVelocity = Vector3.zero;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        ColorLerpValue = 0;
        thisMaterial.color = Color.Lerp(Color.white, Color.black, ColorLerpValue);
    }

    void FixedUpdate()
    {
        Update();
        MoveSpeedMultiplier = Mathf.Lerp(MoveSpeedMultiplier, 1, 速度乘子恢复速度);
        JumpSpeedMultiplier = Mathf.Lerp(JumpSpeedMultiplier, 1, 速度乘子恢复速度);
        this.transform.localScale = new Vector3(1, Mathf.Lerp(this.transform.localScale.y, TargetYScale, 0.2f), 1);
        this.myRigidbody.velocity = new Vector3(this.myRigidbody.velocity.x, this.myRigidbody.velocity.y, 0);
    }

    void PlayerMovementDetectKeyboard()
    {
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Plane();
        }
        else
        {
            UnPlane();
        }
        if (Input.GetKey(KeyCode.S))
        {
            Ballast();
        }
        else
        {
            UnBallast();
        }
        if (Input.GetKey(KeyCode.L))
        {
            inkStrawAnimator.go();
        }
        
    }
    public void MoveLeft()
    {
        if (gameObject.GetComponent<藤蔓抓爬标签>())
        {
            this.myRigidbody.AddForce(Vector3.left * BaseMoveSpeed * (this.InPlaneMode ? 3 : 1));

        }
        else
        {
            this.myRigidbody.velocity = new Vector3(-BaseMoveSpeed * MoveSpeedMultiplier, this.myRigidbody.velocity.y, this.myRigidbody.velocity.z);
        }
    }
    public void MoveRight()
    {
        if (gameObject.GetComponent<藤蔓抓爬标签>())
        {
            this.myRigidbody.AddForce(Vector3.right * BaseMoveSpeed * (this.InPlaneMode?3:1));

        }
        else
        {
            this.myRigidbody.velocity = new Vector3(BaseMoveSpeed * MoveSpeedMultiplier, this.myRigidbody.velocity.y, this.myRigidbody.velocity.z);
        }
    }
    public void Jump()
    {
        if (jio处理器.getIsOnFeet() && this.myRigidbody.velocity.y < BaseUpAcceleration) { this.myRigidbody.AddForce(Vector3.up * BaseUpAcceleration * 100 * JumpSpeedMultiplier); timeController.AddTime(-1 * JumpInkConsumption); }
        if (gameObject.GetComponent<藤蔓抓爬标签>())
        {
            gameObject.GetComponent<藤蔓抓爬标签>().父藤蔓处理器.断开();
            Destroy(gameObject.GetComponent<藤蔓抓爬标签>());
            this.myRigidbody.AddForce(Vector3.up * BaseUpAcceleration);
        }
    }
    public void Plane()
    {
        InPlaneMode = true;
        myRigidbody.useGravity = false;
        myRigidbody.velocity = Vector3.Lerp(myRigidbody.velocity, new Vector3(myRigidbody.velocity.x, 0, myRigidbody.velocity.z), 0.002f);
        myRigidbody.drag = 0.3f;
        myRigidbody.AddForce(new Vector3(0, -1, 0));

    }
    public void UnPlane()
    {
        InPlaneMode = false;
        myRigidbody.useGravity = true;
        myRigidbody.drag = 1f;
    }
    public void Ballast()
    {
        InBallastMode = true;
        SetJumpSpeedMultiplier(0);
        SetMoveSpeedMultiplier(0);
        this.myRigidbody.mass = 300;
        this.timeController.AddTime(-0.3f);
    }
    public void UnBallast()
    {
        InBallastMode = false;
        this.myRigidbody.mass = 1;
    }

    public void TriggerDeath(string DeathReason)
    {
        timeController.endGame(DeathReason);
    }

    public void GenerateNewBlock()
    {
        timeController.AddTime(-5);
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.transform.localScale = Vector3.zero;
        block.transform.position = this.transform.position;
        block.transform.parent = GameObject.Find("Level").transform;
        Rigidbody rgdb = block.AddComponent<Rigidbody>();
        rgdb.mass = 9999999;
        rgdb.drag = 5;
        rgdb.isKinematic = true;
        block.GetComponent<Renderer>().material.color = new Color(0, 0, 0);

        block.AddComponent<Grabable>();
        简单动画器 SMA = block.AddComponent<简单动画器>();
        SMA.Interpolate = 0.2f;
        SMA.Interpolate2 = 0.2f;
        SMA.FromPosition = thisRenderer.transform.position;
        SMA.ToPosition = thisRenderer.transform.position;
        SMA.FromRotation = Vector3.zero;
        SMA.ToRotation = Vector3.zero;
        SMA.ToScale = Vector3.zero;
        SMA.FromScale = Vector3.one * blockSize;
    }

    public void OnCollisionStay(Collision collision)
    {
        //isColliding = true;
    }

    public void SetMoveSpeedMultiplier(float mul)
    {
        MoveSpeedMultiplier = mul;
    }

    public void SetJumpSpeedMultiplier(float mul)
    {
        JumpSpeedMultiplier = mul;
    }
}
