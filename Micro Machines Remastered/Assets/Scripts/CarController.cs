using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    [SerializeField]
    string horizontalAxis;

    [SerializeField]
    string accelerationAxis;

    [SerializeField]
    string brakeAxis;

    [SerializeField]
    string reverseAxis;

    [SerializeField]
    string resetAxis;

    float force = 1000;

    Rigidbody rb;
    public AudioClip[] drifting;
    private AudioSource sjorsAudio;
    public AudioClip shootClip;
    public GameObject driftSound;

    float shrinkSpeed = 1.0f;
    Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);
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
    float launchForce = 700;

    public GameObject[] outOfBounds;

    bool isDrifting = false;

    public RaceManager canFinishBool;
    public RaceManager lapInt;

    public int checkpointCounter = 0;

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
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            car.transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;


            if (car.transform.localScale.x < targetScale.x && car.transform.localScale.y < targetScale.y && car.transform.localScale.z < targetScale.z)
            {
                respawn = true;
                hasCollided = false;
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
            Respawn();
        }
    }

    void Respawn()
    {
        car.transform.localScale = standardTransform;
        transform.eulerAngles = initialRot;
        transform.position = initialPos;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        StartCoroutine(Zeit());
        respawn = false;
    }

    void FixedUpdate ()
    {
        float turn = Input.GetAxis(horizontalAxis);

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

        if (Input.GetButton(accelerationAxis))
        {
            rb.AddForce(transform.forward * speedForce);
        }

        if(Input.GetButton(brakeAxis))
        {
            rb.AddForce(-brakeForce * rb.velocity);
        }

        if (Input.GetButton(reverseAxis))
        {
            rb.AddForce(-transform.forward * speedForce);
        }

        if (Input.GetButton(resetAxis))
        {
            respawn = true;
        }

        if (jumpNow)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.AddForce(transform.up * launchForce, ForceMode.Impulse);
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
            checkpointCounter++;
        }

        if(tagler.tag == "landPart")
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            transform.position = new Vector3(transform.position.x, 0.02f, transform.position.z);
        }

        if (tagler.gameObject.GetComponent<GoSlowLmao>())
        {
            gameObject.GetComponent<Rigidbody>().mass = 1000;
            rb.AddForce(-10000 * rb.velocity);
        }

        if (tagler.GetComponent<LastCheckpoint>())
        {
            canFinishBool.canFinish = true;
        }

        /*if (tagler.GetComponent<LastCheckpoint2>())
        {
            canFinishBool.canFinish2 = true;
        }*/

        if (tagler.GetComponent<FinishLine>())
        {
            if(canFinishBool.canFinish == true)
            {
                lapInt.lapCount++;
                canFinishBool.canFinish = false;
            }

            /*if(canFinishBool.canFinish2 == true)
            {
                lapInt.lapCount2++;
                canFinishBool.canFinish2 = false;
            }*/
        }
    }

    void OnTriggerExit(Collider taggert)
    {
        if(taggert.tag == "jumpPart")
        {
            jumpNow = false;
        }

        if (taggert.gameObject.GetComponent<GoSlowLmao>())
        {
            gameObject.GetComponent<Rigidbody>().mass = 1;
        }
    }

    private void OnCollisionEnter(Collision colbo)
    {
        if(colbo.gameObject.tag == "Obstacle")
        {
            Vector3 dir = colbo.contacts[0].point - transform.position;
            dir = -dir.normalized;
            rb.AddForce(dir * force);
        }
    }

    IEnumerator Zeit()
    {
        yield return new WaitForSeconds(1);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        StopCoroutine(Zeit());
    }

}
