using UnityEngine;
using System.Collections;

public class HandControl : MonoBehaviour {

    private GameObject RightHand;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(RightHand == null)
        {
            RightHand = GameObject.Find("HandRight");
        }
        else
        {
            gameObject.transform.position = new Vector3(-RightHand.transform.position.x, RightHand.transform.position.y, transform.position.z);
        }
	}
}
