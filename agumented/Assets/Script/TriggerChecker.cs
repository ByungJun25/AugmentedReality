using UnityEngine;
using System;
using System.Collections;

public class TriggerChecker : MonoBehaviour {

    public event Action<int> StartAction;
    private enum UIType:int
    {
        Center = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
    public float fLimitTime = 1.0f;
    private float fCheckTime;
    private int UItype;
    private bool isAction = false;
    
    void Start()
    {
        if (transform.name == "Center")
        {
            UItype = (int)UIType.Center;
        }
        else if(transform.name == "Up")
        {
            UItype = (int)UIType.Up;
        }
        else if (transform.name == "Down")
        {
            UItype = (int)UIType.Down;
        }
        else if (transform.name == "Left")
        {
            UItype = (int)UIType.Left;
        }
        else if (transform.name == "Right")
        {
            UItype = (int)UIType.Right;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "HandTipRight" || col.name == "HandTipLeft" || col.name == "HandRight" || col.name == "HandLeft")
        {
            fCheckTime += Time.deltaTime;
            Debug.Log("Object[" + col.transform.name + "] hit");
            if (fCheckTime >= fLimitTime && !isAction)
            {
                fCheckTime = 0.0f;
                isAction = true;
                Debug.Log("Start");
                OnAction();
            } else if(isAction)
            {
                fCheckTime = 0.0f;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        isAction = false;
        fCheckTime = 0.0f;
        if (col.name == "HandTipRight" || col.name == "HandTipLeft")
        {
            Debug.Log("Exit: Object[" + col.transform.name + "] hit");
        }
    }
    
    void OnAction()
    {
        Debug.Log("OnAction");
        if(StartAction != null)
        {
            StartAction(UItype);
        }
    }
}
