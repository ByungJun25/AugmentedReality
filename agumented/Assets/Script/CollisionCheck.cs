using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class CollisionCheck : MonoBehaviour {
    public GameObject gObject;

    void Start()
    {
        if(gObject == null)
        {
            gObject = this.gameObject;
            Debug.Log("object: " + gObject.transform.name);
        }
        else
        {
            Debug.Log("object: " + gObject.transform.name);
        }
    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log("hit: " + col.transform.name);
    }

    /*
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
    }*/
}
