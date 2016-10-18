using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class FollowJoint : MonoBehaviour {
    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;

    // Use this for initialization
    void Start()
    {
        if (BodySrcManager == null)
        {
            Debug.Log("Assign Game Object with Body Source Manager");

        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }


    }

    void update()
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
                var pos = body.Joints[TrackedJoint].Position;
                this.transform.position = new Vector3(pos.X, pos.Y);
            }
        }
    }
}
