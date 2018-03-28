using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    float shrinkSpeed = 1.0f;
    Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);
    float destroyScale = 0.15f;
    bool hasCollided = false;
    GameObject car;
    Vector3 standardTransform;

    float speedForce = 60.0f;
    float brakeForce = 5.0f;
    float torqueForce = 60.0f;
    float driftFactorSticky = 0.9f;
    float driftFactorSlippy = 1f;
    float maxStickyVelocity = 2.5f;
    float minSlippyVelocity = 1.5f;

    public GameObject[] outOfBounds;

    void Start ()
    {
        car = this.gameObject;
        Debug.Log(car.transform.localScale);
        standardTransform = car.transform.localScale;
	}

    void Update()
    {
        if (hasCollided)
        {
            car.transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;

            if (car.transform.localScale.x < targetScale.x && car.transform.localScale.y < targetScale.y && car.transform.localScale.z < targetScale.z)
            {
                hasCollided = false;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            car.transform.localScale = standardTransform;
        }
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

        if(Input.GetButton("Brakes"))
        {
            rb.AddForce(-brakeForce * rb.velocity);
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

    private void OnTriggerEnter(Collider other)
    {
        hasCollided = true;
    }
}
