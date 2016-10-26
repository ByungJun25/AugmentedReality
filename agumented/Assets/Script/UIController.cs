using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
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
    private int tiltDir = -1;
    private bool isTilt = true;

    private enum UiType : int
    {
        Center = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    // Use this for initialization
    void Start () {

        Cube = GameObject.FindGameObjectWithTag("Cube");

        pageNum = 0;
        tiltDir = -1;
        if (CentertriggerChecker == null || UptriggerChecker == null || DowntriggerChecker == null || LefttriggerChecker == null || RighttriggerChecker == null)
        {
            Debug.Log("Fill TriggerChecker. please");
        }
        else {
            CentertriggerChecker.StartAction += OnEvent;
            UptriggerChecker.StartAction += OnEvent;
            DowntriggerChecker.StartAction += OnEvent;
            LefttriggerChecker.StartAction += OnEvent;
            RighttriggerChecker.StartAction += OnEvent;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (UI == null)
        {
            Debug.Log("UI Object is NULL");
        }
        UI.transform.position = GameObject.Find("SpineMid").transform.position + new Vector3(-200.0f, 0.0f, 0.0f);
        Cube.transform.position = GameObject.Find("HandLeft").transform.position + new Vector3(-200.0f, 0.0f, 0.0f);

    }
    
    void OnEvent(int UItype)
    {
        switch(UItype)
        {
            //center
            case (int)UiType.Center:
                CenterButtonAction();
                break;
            //up
            case (int)UiType.Up:
                UpButtonAction();
                break;
            //down
            case (int)UiType.Down:
                DownButtonAction();
                break;
            //left
            case (int)UiType.Left:
                LeftButtonAction();
                break;
            //right
            case (int)UiType.Right:
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
        if(isTilt)
        {
            TiltImage();
        }
        float fadeTime = transform.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        ChangePage(-1);
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
        ChangePage(1);
        fadeTime = transform.GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    void ChangePage(int direction)
    {
        //ppt 부분 기
        pageNum += direction;
        //페이지 교체 소스 넣기
    }
    void TiltImage()
    {
        if(tiltDir == 1)
        {
            StartCoroutine(DisplayMoved(55));
            isTilt = true;
        }
        else if(tiltDir == -1)
        {
            StartCoroutine(DisplayMoved(-55));
            isTilt = false;
        }
        FlipTiltDir();
    }

    void FlipTiltDir()
    {
        if(tiltDir == 1)
        {
            tiltDir = -1;
        } else if(tiltDir == -1)
        {
            tiltDir = 1;
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
        if (isTilt)
        {
            obj.transform.position = initposition + new Vector3(300.0f,0.0f,0.0f);
        } else
        {
            obj.transform.position = initposition - new Vector3(300.0f, 0.0f, 0.0f);
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
}
