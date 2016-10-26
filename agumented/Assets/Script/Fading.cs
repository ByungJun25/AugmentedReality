using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{

    public GameObject gObject;
    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    void Start()
    {
        if(gObject == null)
        {
            gObject = this.gameObject;
        }
        Debug.Log(gObject.transform.localScale.x+", "+gObject.transform.localScale.y);
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(720,0, gObject.transform.localScale.x, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        Debug.Log("beginFadeOut");
        fadeDir = direction;
        return (fadeSpeed);
    }
}
