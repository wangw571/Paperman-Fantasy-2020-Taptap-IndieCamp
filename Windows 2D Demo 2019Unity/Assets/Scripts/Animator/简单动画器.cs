using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 简单动画器 : MonoBehaviour
{
    public Vector3 FromPosition;
    public Vector3 ToPosition;
    public Vector3 FromRotation;
    public Vector3 ToRotation;
    public Vector3 FromScale = Vector3.one;
    public Vector3 ToScale = Vector3.one;

    private Vector3 TargetPosition;
    private Vector3 TargetRotation;
    private Vector3 TargetScale;
    public float Interpolate = 0.2f;
    public float Interpolate2 = 0.2f;
    private bool GoingTo = false;

    public bool isGoing()
    {
        return GoingTo;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GoingTo)
        {
            TargetPosition = Vector3.Lerp(TargetPosition, ToPosition, Interpolate);
            TargetRotation = Vector3.Lerp(TargetRotation, ToRotation, Interpolate);
            TargetScale = Vector3.Lerp(TargetScale, ToScale, Interpolate);

        }
        else
        {
            TargetPosition = Vector3.Lerp(TargetPosition, FromPosition, Interpolate);
            TargetRotation = Vector3.Lerp(TargetRotation, FromRotation, Interpolate);
            TargetScale = Vector3.Lerp(TargetScale, FromScale, Interpolate);
        }
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, TargetPosition, Interpolate2);
        this.transform.localEulerAngles = Vector3.Lerp(this.transform.localEulerAngles, TargetRotation, Interpolate2);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, TargetScale, Interpolate2);
        GoingTo = false;
    }

    public void go()
    {
        GoingTo = true;
    }
}
