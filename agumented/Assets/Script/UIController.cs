using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIController : MonoBehaviour {

    public event Action<bool> StartParticle;

    public GameObject UI;
    public TriggerChecker CentertriggerChecker;
    public TriggerChecker UptriggerChecker;
    public TriggerChecker DowntriggerChecker;
    public TriggerChecker LefttriggerChecker;
    public TriggerChecker RighttriggerChecker;

    GameObject[] pictures;
    GameObject Cube;

    private bool CubeState = false;
    
    private int pageNum = 0;
    private bool isTilt = true;
    private bool isParticle = false;
    private int maximumPage;

    public GameObject picture;

    private enum UiType : int
    {
        Center = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    private enum Direction: int
    {
        Forward = 1,
        Backward = -1,
        Next = 1,
        Previous = -1
    }

    // Use this for initialization
    void Start () {
        Cube = GameObject.FindGameObjectWithTag("Cube");
        if (Cube != null)
        {
            Cube.SetActive(false);
        }
        else
        {
            Debug.Log("Cube is null");
        }
        if (UI == null)
        {
            Debug.Log("UI Object is NULL");
        }
        if (picture != null)
        {
            maximumPage = picture.GetComponent<PictureController>().materials.Count;
        }
        else
        {
            Debug.Log("There is no picture object");
            maximumPage = 0;
        }
        if (CentertriggerChecker == null || UptriggerChecker == null || DowntriggerChecker == null || LefttriggerChecker == null || RighttriggerChecker == null)
        {
            Debug.Log("Fill TriggerChecker. please");
        }
        else
        {
            CentertriggerChecker.StartAction += OnEvent;
            UptriggerChecker.StartAction += OnEvent;
            DowntriggerChecker.StartAction += OnEvent;
            LefttriggerChecker.StartAction += OnEvent;
            RighttriggerChecker.StartAction += OnEvent;
        }
    }
	
	// Update is called once per frame
	void Update () {
        UI.transform.position = GameObject.Find("SpineMid").transform.position + new Vector3(-230.0f, 0.0f, 0.0f);
        Cube.transform.position = GameObject.Find("HandLeft").transform.position + new Vector3(-5.0f, 0.0f, 0.0f);
    }
    
    void OnEvent(int UItype)
    {
        switch(UItype)
        {
            //center
            case (int)UiType.Center:
                Debug.Log("Center");
                CenterButtonAction();
                break;
            //up
            case (int)UiType.Up:
                Debug.Log("Up");
                UpButtonAction();
                break;
            //down
            case (int)UiType.Down:
                Debug.Log("Down");
                DownButtonAction();
                break;
            //left
            case (int)UiType.Left:
                Debug.Log("Left");
                LeftButtonAction();
                break;
            //right
            case (int)UiType.Right:
                Debug.Log("Right");
                RightButtonAction();
                break;
        }
    }

    void CenterButtonAction()
    {
        TiltImage();
        Debug.Log("CenterButtonAction");
    }
    void UpButtonAction()
    {
        CubeController();
        Debug.Log("UpButtonAction");
    }
    void DownButtonAction()
    {
        StartParticle(isParticle);
        FlipIsParticle();
        Debug.Log("DownButtonAction");
    }
    void LeftButtonAction()
    {
        StartCoroutine(PreviousPage());
        Debug.Log("LeftButtonAction");
    }
    void RightButtonAction()
    {
        StartCoroutine(NextPage());
        Debug.Log("RightButtonAction");
    }

    IEnumerator NextPage()
    {
        if (isTilt)
        {
            TiltImage();
        }
        float fadeTime = transform.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        ChangePage((int)Direction.Next);
        fadeTime = transform.GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    IEnumerator PreviousPage()
    {
        if (isTilt)
        {
            TiltImage();
        }
        float fadeTime = transform.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        ChangePage((int)Direction.Previous);
        fadeTime = transform.GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    void ChangePage(int direction)
    {
        pageNum += direction;
        if (pageNum < maximumPage && pageNum >= 0)
        {
            picture.GetComponent<Renderer>().material.mainTexture = picture.GetComponent<PictureController>().materials[pageNum];
        }
        else if (pageNum < 0)
        {
            pageNum = 0;
        }
        else if(pageNum > maximumPage)
        {
            pageNum = maximumPage;
        }
    }
    void TiltImage()
    {
        if(isTilt)
        {
            StartCoroutine(DisplayMoved(-55));
        }
        else
        {
            StartCoroutine(DisplayMoved(55));
        }
    }

    IEnumerator DisplayMoved(float Dir)
    {
        GameObject obj = GameObject.FindGameObjectWithTag("PictureFrame");
        Vector3 initposition = obj.transform.position;
        Quaternion start = obj.transform.rotation;
        Vector3 startEuler = start.eulerAngles;
        Quaternion end = Quaternion.Euler(startEuler.x, startEuler.y + Dir, startEuler.z);
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            Quaternion pos = obj.transform.rotation;
            pos = Quaternion.Slerp(start, end, t);
            obj.transform.rotation = pos;
            yield return 0;
        }
        FlipIsTlit();
        //회전 후 위치 재조정
        if (isTilt)
        {
            obj.transform.position = initposition + new Vector3(470.0f, 0.0f, 0.0f);
        }
        else
        {
            obj.transform.position = initposition - new Vector3(470.0f, 0.0f, 0.0f);
        }

    }

    void CubeController()
    {
        if (CubeState)
        {
            Cube.SetActive(false);
            CubeState = false;
        }
        else
        {
            Cube.SetActive(true);
            CubeState = true;
        }       
    }

    void FlipIsTlit()
    {
        isTilt = !isTilt;
    }

    void FlipIsParticle()
    {
        isParticle = !isParticle;
    }    
    void FlipDirection(int i_Direction)
    {
        if(i_Direction == (int)Direction.Forward)
        {
            i_Direction = (int)Direction.Backward;
        }
        else if(i_Direction == (int)Direction.Backward)
        {
            i_Direction = (int)Direction.Forward;
        }
    }
}
