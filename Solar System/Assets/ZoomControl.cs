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

public class ZoomControl : MonoBehaviour
{
    // Inspector parameters
    [Tooltip("The controller joystick used to control scale.")]
    public CommonAxis scaleJoystrick;

    [Tooltip("The button required to be pressed to scaling objects.")]
    public CommonButton scaleButton;

    [Tooltip("The speed amplifier for thrown objects. One unit is physically realistic.")]
    public float speed = 1.0f;

    [Tooltip("The scaling amplifier for scaling object.")]
    public float scalingRate = 1.0f;

    // Private interaction variables
    FixedJoint grasp;
    Vector3 curScale;
    Collider scaleTarget;
    GameObject[] planets;
    GameObject sun;
    private bool buttonPress;
    private bool prevButtonPress;


    // Called at the end of the program initialization
    void Start()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        sun = GameObject.Find("Sun");
        // Ensure hand interactive is properly configured
    }

    // FixedUpdate is not called every graphical frame but rather every physics frame
    public void ZoomToPlanet(GameObject planetX)
    {
        //buttonPress = scaleButton.GetPress();
        //if (!buttonPress && prevButtonPress)
        //{
            GameObject planet = GameObject.Find("Venus");

            GameObject SolarSystem = GameObject.Find("SolarSystem");

            foreach(GameObject p in planets)
            {
                p.GetComponent<Planet>().DisableMotionControl();

                //Debug.Log(p.transform.position);
                //p.transform.position = p.transform.position + distanceDiff;
            }
            SolarSystem.transform.localScale = SolarSystem.transform.localScale * 10;

            Vector3 distanceDiff = new Vector3(0,1.2f,0) - planetX.transform.position; 
            SolarSystem.transform.Translate(distanceDiff);
            sun.SetActive(false);

            //sun.transform.position = sun.transform.position + distanceDiff;
            //Vector3 scalingRateV3 = new Vector3(scalingRate, scalingRate, scalingRate);
            //Vector3 finalScale = curScale + scaleJoystrick.GetAxis().y * scalingRateV3;
            //if (finalScale.x <= 0 || finalScale.y <= 0 || finalScale.z <= 0)
            //{
            //    finalScale = new Vector3(0.01F, 0.01F, 0.01F);
            //}
            //GameObject.Find("SolarSystem").transform.localScale = finalScale;
        //}
        //prevButtonPress = buttonPress;
    }

    public void ZoomOut()
    {
        GameObject SolarSystem = GameObject.Find("SolarSystem");
        sun.SetActive(true);
        SolarSystem.transform.localScale = SolarSystem.transform.localScale * 0.1f;
        Vector3 distanceDiff = new Vector3(0, 1.2f, 0) - sun.transform.position;
        SolarSystem.transform.Translate(distanceDiff);

        foreach (GameObject p in planets)
        {
            p.GetComponent<Planet>().EnableMotionControl();
        }
    }

}