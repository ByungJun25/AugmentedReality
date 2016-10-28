using UnityEngine;
using System.Collections;

public class ParticleControl : MonoBehaviour {

    public UIController UIcontroller;
    public ParticleSystem particle;
    // Use this for initialization
    void Start () {
        if(particle == null)
        {
            Debug.Log("particle null");
        } 
        if(UIcontroller != null)
        {
            UIcontroller.StartParticle += ShowParticle;
        }
    }

    public void ShowParticle(bool isParticle)
    {
        if (isParticle)
        {
            particle.Play();
        }
        else
        {
            HideParticle();
        }
    }

    public void HideParticle()
    {
        particle.Stop();
        particle.Clear();
    }
}
