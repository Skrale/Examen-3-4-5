using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    float speedForce = 60.0f;
    float torqueForce = 60.0f;
    float driftFactorSticky = 0.9f;
    float driftFactorSlippy = 1f;
    float maxStickyVelocity = 2.5f;
    float minSlippyVelocity = 1.5f;

    void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        float turn = Input.GetAxis("Horizontal");

        float driftFactor = driftFactorSticky;

        if(RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }

        if (Input.GetButton("Accelerate"))
        {
            rb.AddForce(transform.forward * speedForce);
        }

        rb.angularVelocity = transform.up * torqueForce * turn;

        rb.velocity = ForwardVelocity() + RightVelocity() * driftFactorSlippy;
	}

    Vector3 ForwardVelocity()
    {
        return transform.forward * Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
    }

    Vector3 RightVelocity()
    {
        return transform.right * Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.right);
    }
}
