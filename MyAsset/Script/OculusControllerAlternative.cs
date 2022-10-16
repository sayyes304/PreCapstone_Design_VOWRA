using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusControllerAlternative : MonoBehaviour
{


    public GameObject centerEyeAnchor;
    public float angle = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Oculus();
        //blablalba
    }

    void Oculus()
    {

        OVRInput.Update();
        OVRInput.FixedUpdate();

        // 이상하게 GetDown은 안됨..
        if (OVRInput.Get(OVRInput.Button.One))
        {
            Debug.Log("A button");
        }

        if (OVRInput.Get(OVRInput.Button.Two))
        {
            Debug.Log("B button");
        }

        if (OVRInput.Get(OVRInput.Button.Three))
        {
            Debug.Log("X button");

        }
        if (OVRInput.Get(OVRInput.Button.Four))
        {
            Debug.Log("Y button");

        }


        // position

        Vector2 direction = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        Vector3 axis = new Vector3(direction.x * Time.deltaTime, 0, direction.y * Time.deltaTime);

        Vector3 rotated = Quaternion.Euler(0, centerEyeAnchor.transform.rotation.eulerAngles.y, 0) * axis;
        transform.position += rotated;



        // rotation

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            transform.RotateAround(centerEyeAnchor.transform.position, Vector3.up, -angle);
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            transform.RotateAround(centerEyeAnchor.transform.position, Vector3.up, angle);
        }

    }

}
