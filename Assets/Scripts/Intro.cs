using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

    public GameObject holo_world;
    public float speed = 10.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        holo_world.transform.Rotate(Vector3.up * speed * Time.deltaTime);
        if (Input.GetKeyUp(KeyCode.Return))
            Initiate();
	}

    void Initiate()
    {
        holo_world.SetActive(false);
        this.gameObject.GetComponent<Holo_Motion_Control>().enabled = true;
        //this.gameObject.GetComponent<Parts_Manager>().enabled = true;
        this.enabled = false;
    }
}
