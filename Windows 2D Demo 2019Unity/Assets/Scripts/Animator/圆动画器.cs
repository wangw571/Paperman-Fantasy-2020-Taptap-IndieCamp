using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 圆动画器 : MonoBehaviour
{
    public Vector3 v;
    public float R;
    public int positionCount;
    float angle;
    Quaternion q;
    public LineRenderer line;

    void Start()
    {
        //v = this.transform.position;
        angle = 360f / (positionCount - 1);
        line.positionCount = positionCount;
    }
    void Update()
    {
        //v = this.transform.position;
        DrawCircle();
    }
    void DrawCircle()
    {
        line.positionCount = positionCount;
        for (int i = 0; i < positionCount; i++)
        {
            if (i != 0)
            {
                q = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, q.eulerAngles.z + angle);
            }
            Vector3 forwardPosition = (Vector3)v + q * Vector3.down * R;
            line.SetPosition(i, forwardPosition);
        }
    }
    public void UpdatePosition(Vector3 newPos, float radius)
    {
        this.v = newPos;
        this.R = radius;
    }
}
