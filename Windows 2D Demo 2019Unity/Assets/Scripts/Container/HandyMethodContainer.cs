using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class h
{
    public static void l(System.Object someObj)
    {
        Debug.Log(someObj);
    }

    public static bool RectInside(Vector2 rectPos, Vector2 rectSize, Vector2 toDetect)
    {
        Rect rectt = new Rect(rectPos, rectSize);
        return rectt.Contains(toDetect);
    }

    public static GameObject GenerateBall(Vector3 WorldPos, Color ballColor, bool IsKinematic, float size, float LiveTime)
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.position = WorldPos;
        ball.transform.localScale = Vector3.one * size;
        ball.GetComponent<Renderer>().material.color = ballColor;
        if (!IsKinematic)
        {
            Rigidbody rb = ball.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }
        else
        {
            GameObject.Destroy(ball.GetComponent<Collider>());
        }
        SD sd = ball.AddComponent<SD>();
        sd.timer = LiveTime;
        return ball;
    }
}

public class SD : MonoBehaviour
{
    public float timer;
    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if(timer <= 0)
        {
            DestroyImmediate(this.gameObject);
        }
    }
}

