using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class Parts_Manager : MonoBehaviour
{

    public GameObject manager;
    public JointType hand_left;
    public JointType hand_right;
    private BodySourceManager body_manager;
    private Body[] bodies;
    public HandState state;

    public GameObject current_object;
    public GameObject current_instance;

    public GameObject[] object_parts;
    public GameObject holo;

    public Transform init_pos;
    public Transform inside_spot;

    public int index;
    public string left;
    public string right;

    public bool can_switch_to_next;
    public bool can_switch_to_previous;
    public bool can_reset;
    public bool can_enable_all;

    public bool can_separate;
    public bool can_enable;

    public Text vector_coordinates;

    // Use this for initialization
    void Start()
    {

        //this.gameObject.GetComponent<Holo_Motion_Control>().enabled = false;

        body_manager = manager.GetComponent<BodySourceManager>();
        left = "Lasso";
        right = "Lasso";

        holo = this.gameObject;
        index = 0;
        
        can_switch_to_next = true;
        can_switch_to_previous = true;
        can_reset = true;
        can_enable_all = false;

        can_separate = false;
        can_enable = false;

        SetObject();

        Invoke("Reset_Enable_All", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

        if (body_manager == null)
            return;
        bodies = body_manager.GetData();
        if (bodies == null)
            return;
        foreach (var body in bodies)
        {
            if (body == null)
                continue;
            if (body.IsTracked && can_separate == true)
            {

                var pos_left = body.Joints[hand_left].Position;
                var pos_right = body.Joints[hand_right].Position;

                Vector3 vec_left = new Vector3(pos_left.X, pos_left.Y, pos_left.Z);
                Vector2 vec_right = new Vector3(pos_right.X, pos_right.Y, pos_right.Z);
                //vector_coordinates.text = "Left: " + vec_left + "\n" + "Right: " + vec_right;
                switch (body.HandRightState)
                {
                    case HandState.Open:
                        right = "Open"; break;
                    case HandState.Closed:
                        right = "Close"; break;
                    case HandState.Lasso:
                        right = "Lasso"; break;
                }

                switch (body.HandLeftState)
                {
                    case HandState.Open:
                        left = "Open"; break;
                    case HandState.Closed:
                        left = "Close"; break;
                    case HandState.Lasso:
                        left = "Lasso"; break;
                }

                if (left == "Lasso" && right == "Lasso")
                {
                    if (vec_right.x >= 0.4f /*&& vec_left.x >= -0.20f*/ && can_switch_to_next == true)
                    {
                        OnNextPart();
                        can_switch_to_previous = true;
                        can_switch_to_next = false;
                        Invoke("Reset_Switch_Next", 0.75f);
                    }

                    if (/*vec_left.x <= -0.45f &&*/ vec_right.x <= 0.0f && can_switch_to_previous == true)
                    {
                        OnPreviousPart();
                        can_switch_to_previous = false;
                        can_switch_to_next = true;
                        Invoke("Reset_Switch_Previous", 0.75f);
                    }

                    if (vec_left.x <= -0.45f && vec_right.x >= 0.45f && can_reset == true)
                    {
                        OnReset();
                        can_switch_to_next = false;
                        can_switch_to_previous = false;
                        can_reset = false;
                        Invoke("Reset_Reset", 0.75f);
                    }
                    can_enable_all = true;
                }

                else if (right == "Lasso")
                {
                    if (left == "Open")
                    {
                        object_parts[index].transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                    }
                    else if (left == "Close")
                    {
                        object_parts[index].transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                    }
                }

                else if (left == "Lasso")
                {
                    if (right == "Open")
                    {
                        object_parts[index].transform.Rotate(Vector3.up * 50 * Time.deltaTime * (-1));
                    }
                    if (right == "Close")
                    {
                        object_parts[index].transform.Rotate(Vector3.up * 50 * Time.deltaTime);
                    }
                }

                else if (left == "Open" && right == "Open")
                {
                    object_parts[index].transform.Rotate(Vector3.left * 50 * Time.deltaTime);
                }

                else if (left == "Close" && right == "Close")
                {
                    object_parts[index].transform.Rotate(Vector3.left * 50 * Time.deltaTime * (-1));
                }

                else if (right == "Open" && left == "Close" && can_enable_all == true)
                {
                    EnableAll();
                }
            }
        }
    }

    public void SetObject()
    {
        current_object = holo.gameObject.GetComponent<Holo_Motion_Control>().current_object;
        current_instance = Instantiate(current_object, current_object.transform);

        current_instance.transform.parent = this.gameObject.transform;
        current_object.SetActive(false);
        object_parts = new GameObject[current_instance.transform.childCount];

        for (int i = 0; i < current_instance.gameObject.transform.childCount; i++)
        {
            object_parts[i] = current_instance.transform.GetChild(i).gameObject;
            object_parts[i].SetActive(false);
        }

        object_parts[index].SetActive(true);
        can_separate = true;
        can_enable_all = false;
        Invoke("Reset_Enable_All", 1.0f);
    }

    void OnNextPart()
    {
        if (index != current_instance.transform.childCount - 1)
        {
            object_parts[index].transform.rotation = Quaternion.Euler(0, 0, 0);
            object_parts[index].transform.localScale = new Vector3(1, 1, 1);
            object_parts[index].SetActive(false);

            index += 1;
            object_parts[index].SetActive(true);
        }
        else
        {
            object_parts[index].transform.rotation = Quaternion.Euler(0, 0, 0);
            object_parts[index].transform.localScale = new Vector3(1, 1, 1);
            object_parts[index].SetActive(false);

            index = 0;
            object_parts[index].SetActive(true);
        }
    }

    void OnPreviousPart()
    {
        if (index > 0)
        {
            object_parts[index].transform.rotation = Quaternion.Euler(0, 0, 0);
            object_parts[index].transform.localScale = new Vector3(1, 1, 1);
            object_parts[index].SetActive(false);

            index -= 1;
            object_parts[index].SetActive(true);

        }
        else
        {
            object_parts[index].transform.rotation = Quaternion.Euler(0, 0, 0);
            object_parts[index].transform.localScale = new Vector3(1, 1, 1);
            object_parts[index].SetActive(false);

            index = current_object.transform.childCount - 1;
            object_parts[index].SetActive(true);
        }
    }

    public void EnableAll()
    {

        index = 0;
        object_parts[index].SetActive(true);
        this.gameObject.GetComponent<Holo_Motion_Control>().can_interact = true;

        current_object.SetActive(true);
        Destroy(current_instance);

        can_separate = false;
        can_enable_all = false;

        current_object.SetActive(true);
        Debug.Log("Holo Motion enabled");
        
    }

    void OnReset()
    {
        object_parts[index].transform.rotation = Quaternion.Euler(0, 0, 0);
        object_parts[index].transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("Just reset to initial values");
    }

    void Reset_Switch_Next()
    {
        can_switch_to_next = true;
    }

    void Reset_Switch_Previous()
    {
        can_switch_to_previous = true;
    }

    void Reset_Reset()
    {
        can_reset = true;
        can_switch_to_next = true;
        can_switch_to_previous = true;
    }

}