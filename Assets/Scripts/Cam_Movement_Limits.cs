using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Movement_Limits : MonoBehaviour {

    public GameObject front_limit, back_limit, right_limit, left_limit;

    public bool reached_front_limit, reached_back_limit, reached_right_limit, reached_left_limit;
	// Use this for initialization
	void Start () {
        reached_front_limit = false;
        reached_back_limit = false;
        reached_left_limit = false;
        reached_right_limit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider enter)
    {
        if (enter.gameObject == front_limit)
            reached_front_limit = true;
        else if (enter.gameObject == back_limit)
            reached_back_limit = true;
        else if (enter.gameObject == left_limit)
            reached_left_limit = true;
        else if (enter.gameObject == right_limit)
            reached_right_limit = true;
    }
}
