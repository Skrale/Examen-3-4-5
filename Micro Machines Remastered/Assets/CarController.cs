using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    float shrinkSpeed = 1.0f;
    Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 targetScaleBig = new Vector3(1.5f, 1.5f, 1.5f);
    float destroyScale = 0.15f;
    bool hasCollided = false;
    bool jumpNow = false;
    bool shrinkNow = false;

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

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void Start ()
    {
        car = this.gameObject;
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

        if (jumpNow)
        {
            car.transform.localScale += Vector3.one * Time.deltaTime * shrinkSpeed;

            if (car.transform.localScale.x > targetScaleBig.x && car.transform.localScale.y > targetScaleBig.y && car.transform.localScale.z > targetScaleBig.z)
            {
                jumpNow = false;
            }
        }

        if (shrinkNow)
        {
            car.transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;

            if (car.transform.localScale.x <= standardTransform.x && car.transform.localScale.y <= standardTransform.y && car.transform.localScale.z <= standardTransform.z)
            {
                car.transform.localScale = standardTransform;
                shrinkNow = false;
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

    void OnTriggerEnter(Collider tagler)
    {
        if(tagler.tag == "outOfBounds")
        {
            hasCollided = true;
        }

        if(tagler.tag == "jumpPart")
        {
            jumpNow = true;
        }
    }

    void OnTriggerExit(Collider taggert)
    {
        if(taggert.tag == "jumpPart")
        {
            shrinkNow = true;
        }
    }

}
