using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode_Manager : MonoBehaviour {

    public bool can_motion;
    public bool can_parts;
    public bool can_switch;

    Holo_Motion_Control motion;
    Parts_Manager parts;
	// Use this for initialization
	void Start () {
        
        can_motion = false;
        can_parts = true;
        can_switch = true;

        motion = this.gameObject.GetComponent<Holo_Motion_Control>();
        parts = this.gameObject.GetComponent<Parts_Manager>();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Switch_to_Motion()
    {
        if (can_motion == true && can_switch == true)
        {
            this.gameObject.GetComponent<Holo_Motion_Control>().can_interact = true;
            this.gameObject.GetComponent<Parts_Manager>().can_separate = false;
           // this.gameObject.GetComponent<Holo_Motion_Control>().Reset_Children();
            can_motion = false;
            can_switch = false;
            Invoke("Reset_Motion", 1.0f);
        }
    }

    public void Switch_to_Parts()
    {
        if(can_parts == true && can_switch == true)
        {
            this.gameObject.GetComponent<Holo_Motion_Control>().can_interact = false;
            this.gameObject.GetComponent<Parts_Manager>().can_separate = true;
            this.gameObject.GetComponent<Parts_Manager>().SetObject();
            can_parts = false;
            can_switch = false;
            Invoke("Reset_Parts", 1.0f);
        }
    }

    void Reset_Motion()
    {
        can_motion = true;
        Invoke("Reset_Switch", 1.0f);
    }

    void Reset_Parts()
    {
        can_parts = true;
        Invoke("Reset_Switch", 1.0f);
    }

    void Reset_Switch()
    {
        can_switch = true;
    }

}
