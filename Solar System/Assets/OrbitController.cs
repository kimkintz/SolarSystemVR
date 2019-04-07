using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour {

    public CommonButton button;
    public CommonSpace space;
    private bool buttonPress;
    private bool prevButtonPress;

    private GameObject[] planets;

    public CommonAxis scaleJoystrick;

    public float scalingRate = 0.001f;

    public GameObject solarSystem;

    Vector3 curScale;

    private bool isStopped = false;

    // Use this for initialization
    void Start () 
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        Debug.Log(planets);

        curScale = solarSystem.transform.localScale;
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

        //Vector3 scalingRateV3 = new Vector3(scalingRate, scalingRate, scalingRate);
        //Vector3 finalScale = curScale + scaleJoystrick.GetAxis().y * scalingRateV3;
        //if (finalScale.x <= 0 || finalScale.y <= 0 || finalScale.z <= 0)
        //{
        //    finalScale = new Vector3(0.002F, 0.002F, 0.002F);
        //}
        //Debug.Log("y is " + scaleJoystrick.GetAxis().y);
        //Debug.Log("curScale is " + curScale * 1000);
        //Debug.Log("finalScale is " + finalScale * 1000);
        //solarSystem.transform.localScale = finalScale;

        prevButtonPress = buttonPress;
    }
}
