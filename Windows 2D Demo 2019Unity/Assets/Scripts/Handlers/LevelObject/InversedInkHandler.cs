using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System;


public class InversedInkHandler : MonoBehaviour
{
    public float FullAmountOfTime;
    public float AmountOfTime;
    public Vector3 FullScale = Vector3.one * 0.5f;
    public Vector3 OriginalScale = Vector3.one;
    public float ParticleEffectScale = 3;
    private Vector3 ScaleIntertia;
    public List<Renderer> thisRenderer;

    public bool 持续激活;
    private bool activated = false;


    [Serializable]
    public class 充满事件 : UnityEvent { }

    [FormerlySerializedAs("充满时")]
    [SerializeField]
    private 充满事件 充满 = new 充满事件();

    [FormerlySerializedAs("未充满时")]
    [SerializeField]
    private 充满事件 未充满 = new 充满事件();


    public 充满事件 充满时
    {
        get { return 充满; }
        set { 充满 = value; }
    }

    public 充满事件 未充满时
    {
        get { return 未充满; }
        set { 未充满 = value; }
    }

    void Start()
    {
        //foreach(Renderer renderererer in thisRenderer)
        //{
        //    string shaderName = renderererer.material.shader.name;
        //    Texture textureUsed = renderererer.material.mainTexture;
        //    renderererer.material = new Material(Shader.Find(shaderName));
        //    renderererer.material.mainTexture = textureUsed;
        //}
        this.transform.localScale = OriginalScale;
        ColorReduction();
    }

    // Update is called once per frame
    void Update()
    {
        ScaleIntertia = Vector3.Lerp(OriginalScale, FullScale, AmountOfTime / FullAmountOfTime);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, ScaleIntertia, 0.02f);
        ColorReduction();


        if (持续激活 && activated)
        {
            if (AmountOfTime >= FullAmountOfTime)
            {
                充满.Invoke();
            }
            else
            {
                activated = false;
            }
        }

        if (AmountOfTime >= FullAmountOfTime && !activated)
        {
            充满.Invoke();
            activated = true;
        }

        if (!activated)
        {
            未充满.Invoke();
        }
    }

    //public void OnCollisionStay(Collision col)
    //{
    //    if (AmountOfTime <= 0)
    //    {
    //        return;
    //    }
    //    if (col.gameObject.GetComponent<PlayerController>())
    //    {
    //        timeController.AddTime(TimePerAdd);
    //        AmountOfTime -= TimePerAdd;
    //        AmountOfTime = Mathf.Clamp(AmountOfTime, 0, float.MaxValue);
    //        //ColorReduction();
    //    }
    //}

    private void ColorReduction()
    {
        foreach (Renderer renderererer in thisRenderer)
        {
            renderererer.material.color = (Color.Lerp(Color.white, Color.black, AmountOfTime / FullAmountOfTime));
        }
    }
}
