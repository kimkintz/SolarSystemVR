using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour 
{

    public float orbitSpeed;
    public float rotateSpeed;
    public float radius;
    private bool motionOn = true;
    private bool zoomed = false;

    // Inspector parameters
    [Tooltip("A button required to be pressed to activate steering.")]
    public CommonButton button;

    [Tooltip("The space that is translated by this interaction. Usually set to the physical tracking space.")]
    public CommonSpace space;

    private Vector3 sun_position;
    private float angle;
    private bool buttonPress;
    private bool prevButtonPress;
    // Use this for initialization
    void Start()
    {
        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        radius = Vector3.Distance(this.transform.position, sun.transform.position);
        string r = radius.ToString();
        sun_position = GameObject.FindGameObjectWithTag("Sun").transform.position;
    }

    // Update is called once per frame
    void Update () 
    {
        //Debug.Log(motionOn);
        //buttonPress = button.GetPress();

        //if (!buttonPress &&  prevButtonPress)
        //{
        //    motionOn = !motionOn;
        //}

            
        if (!motionOn)
        {
            //angle += orbitSpeed * 0;
            //var offset = new Vector3(Mathf.Sin(angle),0 , Mathf.Cos(angle)) * radius;
            //transform.position = sun_position + offset;

            ////Rotation
            //transform.Rotate(Vector3.down, rotateSpeed * 0);
        }
        else if(!zoomed)
        {
            angle += orbitSpeed * Time.deltaTime;
            var offset = new Vector3(Mathf.Sin(angle),0 , Mathf.Cos(angle)) * radius;
            transform.position = sun_position + offset;

            //Rotation
            transform.Rotate(Vector3.down, rotateSpeed * Time.deltaTime);
        }


        //prevButtonPress = buttonPress;

    }

    public void ToggleMotion()
    {
        motionOn = !motionOn;
    }

    public void DisableMotionControl()
    {
        motionOn = false;
        zoomed = true;
    }

    public void EnableMotionControl()
    {
        zoomed = false;
    }
}
