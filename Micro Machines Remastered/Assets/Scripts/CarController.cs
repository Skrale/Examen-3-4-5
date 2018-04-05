using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    Rigidbody rb;
    public AudioClip[] drifting;
    private AudioSource sjorsAudio;
    public AudioClip shootClip;
    public GameObject driftSound;

    float shrinkSpeed = 1.0f;
    Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 targetScaleBig = new Vector3(1.5f, 1.5f, 1.5f);
    float destroyScale = 0.15f;
    bool hasCollided = false;
    bool jumpNow = false;
    bool shrinkNow = false;

    Vector3 initialPos;
    Vector3 initialRot;
    bool respawn = false;

    public ParticleSystem tireSmoke1;
    public ParticleSystem tireSmoke2;

    GameObject car;
    Vector3 standardTransform;

    float speedForce = 60.0f;
    float brakeForce = 5.0f;
    float torqueForce = 60.0f;
    float driftFactorSticky = 0.9f;
    float driftFactorSlippy = 1f;
    float maxStickyVelocity = 2.5f;
    public float stopParticle = 1.5f;

    public GameObject[] outOfBounds;

    bool isDrifting = false;

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        sjorsAudio = driftSound.GetComponent<AudioSource>();
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
                respawn = true;
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

        if (respawn)
        {
            car.transform.localScale = standardTransform;
            transform.eulerAngles = initialRot;
            transform.position = initialPos;
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            StartCoroutine(Zeit());
            respawn = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            car.transform.localScale = standardTransform;
        }
    }

    void FixedUpdate ()
    {
        float turn = Input.GetAxis("Horizontal");

        float driftFactor = driftFactorSticky;

        if(RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }

        if (RightVelocity().magnitude > stopParticle)
        {
            tireSmoke1.Play();
            tireSmoke2.Play();
            if (!isDrifting)
            {
                sjorsAudio.PlayOneShot(shootClip);
                isDrifting = true;
            }
        }

        if (RightVelocity().magnitude < stopParticle)
        {
            tireSmoke1.Stop();
            tireSmoke2.Stop();

            sjorsAudio.Stop();
            isDrifting = false;
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

        if(tagler.tag == "Checkpoint")
        {
            initialPos = tagler.transform.position;
            initialRot = tagler.transform.eulerAngles;
        }
    }

    void OnTriggerExit(Collider taggert)
    {
        if(taggert.tag == "jumpPart")
        {
            shrinkNow = true;
        }
    }

    IEnumerator Zeit()
    {
        yield return new WaitForSeconds(1);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        StopCoroutine(Zeit());
    }

}
