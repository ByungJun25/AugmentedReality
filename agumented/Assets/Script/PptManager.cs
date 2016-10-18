using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class PptManager : MonoBehaviour {
    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;
    public GameObject Left;
    private bool Leftbool;
    private int LeftCount = 0;
    public int LeftEndCount = 500;
    public GameObject Right;
    private bool Rightbool;
    private int RightCount = 0;
    public int RightEndCount = 500;

    // Use this for initialization
    void Start () {
        if (BodySrcManager == null)
        {
            Debug.Log("Assign Game Object with Body Source Manager");

        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
        Leftbool = false;
        Rightbool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bodyManager == null)
        {
            return;
        }
        bodies = bodyManager.GetData();

        if (bodies == null)
        {
            return;
        }
        foreach (var body in bodies)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                if(LeftCount == LeftEndCount)
                {
                    print("왼쪽 시작");
                    LeftCount = 0;
                    Leftbool = false;
                    return;
                }
                if(RightCount == RightEndCount)
                {
                    print("오른쪽 시작");
                    RightCount = 0;
                    Rightbool = false;
                    return;
                }
               
                
            }
        }
    }
}
