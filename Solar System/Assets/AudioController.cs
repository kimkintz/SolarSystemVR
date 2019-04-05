using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    GameObject AudioSource;
    public AudioClip[] clips;
    private ArrayList planetNames = new ArrayList();
	// Use this for initialization
	void Start () 
    {
        AudioSource = GameObject.Find("AudioSource");
        planetNames.Add("Mercury");
        planetNames.Add("Venus");
        planetNames.Add("Earth");
        planetNames.Add("Mars");
        planetNames.Add("Jupiter");
        planetNames.Add("Saturn");
        planetNames.Add("Uranus");
        planetNames.Add("Neptune");


    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SetAudio(int pageNumber, GameObject planet)
    {
        int planetIndex = planetNames.IndexOf(planet.name);
        Debug.Log(planetIndex);
        AudioSource.GetComponent<Sound>().audioClip = clips[(planetIndex*3) + (pageNumber-1)];
        AudioSource.GetComponent<Sound>().play = true;
    }

    public void StopAudio()
    {
        AudioSource.GetComponent<Sound>().stop = true;
    }
}
