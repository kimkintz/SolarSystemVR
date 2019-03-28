using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour 
{

    public float orbitSpeed;
    public float rotateSpeed;
    public float radius;
    private bool motionOn = true;

    private Vector3 sun_position;
    private float angle;
	// Use this for initialization
	void Start () 
    {
        sun_position = GameObject.FindGameObjectWithTag("Sun").transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (motionOn)
        {
            //Orbit
            angle += orbitSpeed * Time.deltaTime;
            var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
            transform.position = sun_position + offset;

            //Rotation
            transform.Rotate(Vector3.right, rotateSpeed * Time.deltaTime);
        }
    }

    public void ToggleMotion()
    {
        motionOn = !motionOn;
    }
}
