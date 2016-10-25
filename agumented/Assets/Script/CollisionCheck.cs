using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class CollisionCheck : MonoBehaviour {

    public GameObject CollisionObject;
    private static bool check;

    void Start()
    {

    }
    
    void Update()
    {

    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == CollisionObject.name)
        {
            check = true;
        }
    }

    public bool getCheck()
    {
        return check;
    }
}
