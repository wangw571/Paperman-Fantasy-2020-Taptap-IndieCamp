using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 墨水球动画器 : MonoBehaviour
{
    public GameObject ToPosGameObj;
    public Vector3 ToScale = Vector3.zero;
    public Vector3 ToPositionRandomShift = Vector3.zero;

    public bool TargetUsingScreenPos = false;

    private Vector3 ToPosition;
    private Vector3 TargetPosition;
    private Vector3 TargetScale;

    private Vector3 ToPos2;
    public float Interpolate;
    public float Interpolate2;
    void Start()
    {
        ToPosition = TargetUsingScreenPos ? Camera.main.ScreenToWorldPoint(ToPosGameObj.transform.position) : ToPosGameObj.transform.position;
        TargetPosition = this.transform.localPosition;
        TargetScale = this.transform.localScale;
        ToPos2 = ToPosition + ToPositionRandomShift;
    }

    void Update()
    {
        //h.l(ToPosition);
        ToPosition = TargetUsingScreenPos ? Camera.main.ScreenToWorldPoint(ToPosGameObj.transform.position) : ToPosGameObj.transform.position;
        ToPos2 = Vector3.Lerp(ToPos2, ToPosition, Interpolate2);
        TargetPosition = Vector3.Lerp(TargetPosition, ToPos2, Interpolate);
        TargetScale = Vector3.Lerp(TargetScale, ToScale, Interpolate);

        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, TargetPosition, Interpolate2);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, TargetScale, Interpolate2);
    }

}
