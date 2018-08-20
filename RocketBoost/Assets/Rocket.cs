using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    static Rigidbody rigidBody;
    static AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();
    }

    private static void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }
        if (Input.GetKey(KeyCode.A)){
            rigidBody.transform.Rotate(Vector3.forward);
        }
        else   if (Input.GetKey(KeyCode.D))
        {
            rigidBody.transform.Rotate(-Vector3.forward);
        }
	}
}
