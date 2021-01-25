using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 藤蔓处理器 : MonoBehaviour
{
    private ConfigurableJoint 我的关节;
    public float 再次抓取冷却时间 = 1f;
    private float cd = 0;
    private 藤蔓抓爬标签 grab;

    public bool 限制X旋转 = false;
    public bool 限制Y旋转 = true;
    public bool 限制Z旋转 = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cd -= Time.fixedDeltaTime;
    }

    public void OnTriggerEnter(Collider col)
    {
        if(cd > 0) { return; }
        //h.l(col.gameObject.name);
        玩家控制器 pc = col.gameObject.GetComponentInParent<玩家控制器>();
        if (pc && !pc.gameObject.GetComponent<藤蔓抓爬标签>())
        {
            我的关节 = this.gameObject.AddComponent<ConfigurableJoint>();
            我的关节.xMotion = ConfigurableJointMotion.Locked;
            我的关节.yMotion = ConfigurableJointMotion.Locked;
            我的关节.zMotion = ConfigurableJointMotion.Locked;
            我的关节.angularXMotion = !限制X旋转? ConfigurableJointMotion.Free : ConfigurableJointMotion.Locked;
            我的关节.angularYMotion = !限制Y旋转? ConfigurableJointMotion.Free : ConfigurableJointMotion.Locked;
            我的关节.angularZMotion = !限制Z旋转 ? ConfigurableJointMotion.Free : ConfigurableJointMotion.Locked;
            我的关节.anchor = Vector3.zero;
            我的关节.breakForce = Mathf.Infinity;
            我的关节.breakTorque = Mathf.Infinity;
            我的关节.connectedBody = pc.myRigidbody;
            grab = pc.gameObject.AddComponent<藤蔓抓爬标签>();
            grab.父藤蔓处理器 = this;
        }
    }

    public void 断开()
    {
        DestroyImmediate(我的关节);
        DestroyImmediate(grab);
        cd = 再次抓取冷却时间;
    }
}
