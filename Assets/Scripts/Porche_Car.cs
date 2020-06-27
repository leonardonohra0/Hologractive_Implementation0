using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porche_Car : MonoBehaviour {

    public GameObject left_door, right_door;
    public Transform left_rotator, right_rotator;

	// Use this for initialization
	void Start () {
        left_door.gameObject.transform.rotation = left_rotator.rotation;
        right_door.gameObject.transform.rotation = right_rotator.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
