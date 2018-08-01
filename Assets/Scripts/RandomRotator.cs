using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {
    Rigidbody asteroidRigidbody;
    public float tumble;

	// Use this for initialization
	void Start () {
        asteroidRigidbody = GetComponent<Rigidbody>();
        asteroidRigidbody.angularVelocity = Random.insideUnitSphere * tumble;
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
