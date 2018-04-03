using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carEngineSound : MonoBehaviour {

    public float topSpeed = 100; // km per hour
    private float currentSpeed = 0;
    private float potch = 0;

    void Update()
    {
        currentSpeed = transform.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        potch = currentSpeed / topSpeed;

        transform.GetComponent<AudioSource>().pitch = potch;
    }
}
