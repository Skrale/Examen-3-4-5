﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.position = new Vector3(target.position.x, 40, target.position.z);
	}
}
