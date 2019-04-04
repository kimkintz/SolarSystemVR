using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour {

    public CommonButton button;
    public CommonSpace space;
    private bool buttonPress;
    private bool prevButtonPress;

    private GameObject[] planets;

	// Use this for initialization
	void Start () 
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        Debug.Log(planets);
	}
	
	// Update is called once per frame
	void Update () 
    {
        buttonPress = button.GetPress();
        if (!buttonPress && prevButtonPress)
        {
            foreach(GameObject p in planets)
            {
                p.GetComponent<Planet>().ToggleMotion();
            }
        }
        prevButtonPress = buttonPress;
    }
}
