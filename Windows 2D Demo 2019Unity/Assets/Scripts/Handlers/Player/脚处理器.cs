using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 脚处理器 : MonoBehaviour
{
    public float UpdateCD = 0.5f;
    private bool isOnFeet = false;
    private float CD = 0;

    void FixedUpdate()
    {
        CD -= Time.fixedDeltaTime;
        if(CD <= 0)
        {
            isOnFeet = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (CD > 0 || other.isTrigger) return;
        isOnFeet = true;
        CD = UpdateCD;
    }

    public bool getIsOnFeet()
    {
        if (isOnFeet)
        {
            isOnFeet = false;
            return true;
        }
        return isOnFeet;
    }
    public bool getNotOnFeet()
    {
        return !isOnFeet;
    }
}
