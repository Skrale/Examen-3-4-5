using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    float speedForce = 100.0f;
    float torque = 15.0f;
    
    void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        float turn = Input.GetAxis("Horizontal");

        if (Input.GetButton("Accelerate"))
        {
            rb.AddForce(transform.forward * speedForce);
        }

        rb.AddTorque(transform.up * torque * turn);


	}

    Vector3 ForwardVelocity()
    {
        return transform.forward * Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
    }

    Vector3 RightVelocity()
    {
        return transform.forward * Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
    }
}
