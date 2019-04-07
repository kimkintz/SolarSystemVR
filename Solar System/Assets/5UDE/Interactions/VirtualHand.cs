/*
Copyright ©2017. The University of Texas at Dallas. All Rights Reserved. 

Permission to use, copy, modify, and distribute this software and its documentation for 
educational, research, and not-for-profit purposes, without fee and without a signed 
licensing agreement, is hereby granted, provided that the above copyright notice, this 
paragraph and the following two paragraphs appear in all copies, modifications, and 
distributions. 

Contact The Office of Technology Commercialization, The University of Texas at Dallas, 
800 W. Campbell Road (AD15), Richardson, Texas 75080-3021, (972) 883-4558, 
otc@utdallas.edu, https://research.utdallas.edu/otc for commercial licensing opportunities.

IN NO EVENT SHALL THE UNIVERSITY OF TEXAS AT DALLAS BE LIABLE TO ANY PARTY FOR DIRECT, 
INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS, ARISING 
OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF THE UNIVERSITY OF TEXAS AT 
DALLAS HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

THE UNIVERSITY OF TEXAS AT DALLAS SPECIFICALLY DISCLAIMS ANY WARRANTIES, INCLUDING, BUT 
NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE. THE SOFTWARE AND ACCOMPANYING DOCUMENTATION, IF ANY, PROVIDED HEREUNDER IS 
PROVIDED "AS IS". THE UNIVERSITY OF TEXAS AT DALLAS HAS NO OBLIGATION TO PROVIDE 
MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
*/

using UnityEngine;
using System.Collections;

public class VirtualHand : MonoBehaviour
{

    // Enumerate states of virtual hand interactions
    public enum VirtualHandState
    {
        Open,
        Touching,
        Holding,
        Zoomed
    };

    // Inspector parameters
    [Tooltip("The tracking device used for tracking the real hand.")]
    public CommonTracker tracker;

    [Tooltip("The interactive used to represent the virtual hand.")]
    public Affect hand;

    [Tooltip("The button required to be pressed to grab objects.")]
    public CommonButton button;

    [Tooltip("The button required to be pressed to return to solar system view.")]
    public CommonButton exitButton;

    public CommonButton touchPadButton;

    public CommonAxis joystick;



    [Tooltip("The speed amplifier for thrown objects. One unit is physically realistic.")]
    public float speed = 1.0f;

    private static int pageNumber;
    private static int previousPageNumber;
    private bool touchButtonPress;
    private bool prevTouchButtonPress = false;
    public float scaleRate;
    private bool zoomedIn = false;
    GameObject[] planets;

    GameObject planet;
    public GameObject solarSystem;
    // Private interaction variables
    VirtualHandState state;
    FixedJoint grasp;

    // Called at the end of the program initialization
    void Start()
    {

        // Set initial state to open
        state = VirtualHandState.Open;

        // Ensure hand interactive is properly configured
        hand.type = AffectType.Virtual;

        pageNumber = 0;
        planets = GameObject.FindGameObjectsWithTag("Planet");
    }

    // FixedUpdate is not called every graphical frame but rather every physics frame
    void FixedUpdate()
    {
        if(solarSystem.transform.localScale.x >=.0021f)
        {
            foreach (GameObject p in planets)
            {
                p.GetComponent<Planet>().DisableMotionControl();

            }
        }
        else
        {
            foreach (GameObject p in planets)
            {
                p.GetComponent<Planet>().EnableMotionControl();
            }
        }

        // If state is open
        if (state == VirtualHandState.Open)
        {
            Debug.Log("Hand is open");
            // If the hand is touching something
            if (hand.triggerOngoing)
            {
                // Change state to touching
                state = VirtualHandState.Touching;
            }

            // Process current open state
            else
            {
                if(exitButton.GetPress())
                {
                    solarSystem.transform.localScale = new Vector3(0.002f, 0.002f, .002f);
                    solarSystem.transform.position = new Vector3(0, 1.2f, 0.5f);
                }
            }
        }

        // If state is touching
        else if (state == VirtualHandState.Touching)
        {
            Debug.Log("Hand is touching");
            // If the hand is not touching something
            if (!hand.triggerOngoing)
            {

                // Change state to open
                state = VirtualHandState.Open;
            }

            // If the hand is touching something and the button is pressed
            else if (hand.triggerOngoing && button.GetPress())
            {
                state = VirtualHandState.Holding;
            }

            // Process current touching state
            else
            {
                Collider t = hand.ongoingTriggers[0];
                planet = t.gameObject;
                Vector3 planetLocation = planet.transform.position;
                Vector3 locationDiff;
                if (touchPadButton.GetPress())
                {
                    Vector3 curScale = solarSystem.transform.localScale;
                    if (joystick.GetAxis().y > 0 && curScale.x < .03f)
                    {
                        solarSystem.transform.localScale = new Vector3(curScale.x, curScale.y, curScale.z) + new Vector3(scaleRate, scaleRate, scaleRate);
                        locationDiff = planetLocation - planet.transform.position;
                        solarSystem.transform.Translate(locationDiff);
                        Debug.Log(solarSystem.transform.localScale.x * 1.0f);
                    }
                    else if (joystick.GetAxis().y < 0 && curScale.x > .0021f)
                    {
                        solarSystem.transform.localScale = new Vector3(curScale.x, curScale.y, curScale.z) - new Vector3(scaleRate, scaleRate, scaleRate);
                        locationDiff = planetLocation - planet.transform.position;
                        solarSystem.transform.Translate(locationDiff);
                        Debug.Log(solarSystem.transform.localScale.x * 1.0f);
                    }
                }
                if (exitButton.GetPress())
                {
                    solarSystem.transform.localScale = new Vector3(0.002f, 0.002f, .002f);
                    solarSystem.transform.position = new Vector3(0, 1.2f, 0.5f);
                }
            }
        }

        // If state is holding
        else if (state == VirtualHandState.Holding)
        {
            Debug.Log("Hand is holding");
            Collider t = hand.ongoingTriggers[0];
            planet = t.gameObject;
            GameObject zc = GameObject.Find("ZoomControl");
            zc.GetComponent<ZoomControl>().ZoomToPlanet(planet);
            state = VirtualHandState.Zoomed;
            // If grasp has been broken
            if (grasp == null)
            {

                // Update state to open
                //state = VirtualHandState.Open;
            }

            // If button has been released and grasp still exists
            else if (!button.GetPress() && grasp != null)
            {

                // Get rigidbody of grasped target
                Rigidbody target = grasp.GetComponent<Rigidbody>();
                // Break grasp
                DestroyImmediate(grasp);

                // Apply physics to target in the event of attempting to throw it
                target.velocity = hand.velocity * speed;
                target.angularVelocity = hand.angularVelocity * speed;

                // Update state to open
                //state = VirtualHandState.Open;
            }

            // Process current holding state
            else
            {

                // Nothing to do for holding
            }
        }
        else if(state == VirtualHandState.Zoomed)
        {
            GameObject ac = GameObject.Find("AudioController");
            previousPageNumber = pageNumber;
            touchButtonPress = touchPadButton.GetPress();
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            //Navigate right page
            if(pageNumber < 3 && joystick.GetAxis().x > 0.0f && !touchButtonPress && prevTouchButtonPress)
            {
                pageNumber++;
            }
            //Navigate left page
            else if(pageNumber > 1 && joystick.GetAxis().x < 0.0f && !touchButtonPress && prevTouchButtonPress)
            {
                pageNumber--;
            }

            if(pageNumber != previousPageNumber)
            {
                Debug.Log("Setting audio");
                //Set audio
                ac.GetComponent<AudioController>().SetAudio(pageNumber, planet);
                //Set text on panel
            }

            //UI control starts

            GameObject PlanetUI = planet.transform.Find(planet.name + "Info").gameObject;

            //Rotate panel to face user
            if(previousPageNumber == 0)
            {
                GameObject hmd = GameObject.Find("Vive HMD");
                float angle = getPanelRotation(hmd.transform.position.x, hmd.transform.position.z);
                PlanetUI.transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            PlanetUI.gameObject.SetActive(true); 
            for (int i = 1; i < 4; i++)
            {
                if(i == pageNumber)
                {
                    PlanetUI.transform.GetChild(i - 1).gameObject.SetActive(true);
                }
                else
                {
                    PlanetUI.transform.GetChild(i - 1).gameObject.SetActive(false);
                }
            } 

            //UI contro ends

            if (exitButton.GetPress())
            {
            	PlanetUI.gameObject.SetActive(false);

                GameObject zc = GameObject.Find("ZoomControl");
                zc.GetComponent<ZoomControl>().ZoomOut();
                pageNumber = 0;
                state = VirtualHandState.Open;
                ac.GetComponent<AudioController>().StopAudio();
            }


            prevTouchButtonPress = touchButtonPress;

        }
    }

    private float getPanelRotation(float x, float z)
    {
        float rotateAngle;
        if(x>=0)
        {
            if(z>=0)
            {
                rotateAngle = -90.0f - Mathf.Atan(z / x); 
            }
            else
            {
                rotateAngle = -1 * Mathf.Atan(x / (z * -1));
            }
        }
        else
        {
            if (z >= 0)
            {
                rotateAngle = 90.0f + Mathf.Atan(z / (x * -1));
            }
            else
            {
                rotateAngle = Mathf.Atan((x * -1) / (z * -1));
            }
        }
        return rotateAngle;
    }
}