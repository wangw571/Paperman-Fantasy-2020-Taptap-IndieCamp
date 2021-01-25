using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class 门处理器 : MonoBehaviour
{
    //public Text DEBUG;
    public string 钥匙代码;

    [Serializable]
    public class 解锁事件 : UnityEvent { }

    [FormerlySerializedAs("解锁时")]
    [SerializeField]
    private 解锁事件 解锁 = new 解锁事件();

    [FormerlySerializedAs("未解锁时")]
    [SerializeField]
    private 解锁事件 未解锁 = new 解锁事件();

    void Start()
    {
        
    }
    void Update()
    {
        未解锁.Invoke();
    }
    private void FixedUpdate()
    {
        未解锁.Invoke();
    }

    public 解锁事件 解锁时
    {
        get { return 解锁; }
        set { 解锁 = value; }
    }

    public 解锁事件 未解锁时
    {
        get { return 未解锁; }
        set { 未解锁 = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        钥匙标签 keyKode = other.gameObject.GetComponent<钥匙标签>();
        if (keyKode != null && keyKode.钥匙代码 == this.钥匙代码)
        {
            Destroy(keyKode);
            Destroy(this);
            try {
                解锁.Invoke();
            }
            catch(Exception e)
            {
                //DEBUG.text = e.ToString();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        钥匙标签 keyKode = other.gameObject.GetComponent<钥匙标签>();
        if (keyKode != null && keyKode.钥匙代码 == this.钥匙代码)
        {
            Destroy(keyKode);
            Destroy(this);
            try
            {
                解锁.Invoke();
            }
            catch (Exception e)
            {
                //DEBUG.text = e.ToString();
            }
        }
    }
}
