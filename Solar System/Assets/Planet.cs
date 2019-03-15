using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour 
{

    public float speed;
    public float radius;

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
        //Orbit
        angle += speed * Time.deltaTime;
        var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
        transform.position = sun_position + offset;

        //Rotation
    }
}
