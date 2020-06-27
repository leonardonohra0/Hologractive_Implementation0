using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class Holo_Motion_Control : MonoBehaviour {

    public GameObject manager;
    public Camera top_cam, bot_cam, left_cam, right_cam, main_cam;

    public Transform inside_pos;
    public Transform top_pos, bot_pos, left_pos, right_pos;

    public GameObject current_object;
    public GameObject holo_objects;
    public GameObject[] holo_object;

    public JointType hand_left;
    public JointType hand_right;
    private BodySourceManager body_manager;
    private Body[] bodies;
    public HandState state;

    public Text vector_coordinates;
    public Canvas holo_display;

    public string left;
    public string right;

    public float field_of_view;
    public int index;

    public bool is_inside;
    public bool can_switch_views;
    public bool can_switch_to_next;
    public bool can_switch_to_previous;
    public bool can_reset;

    public bool can_interact;
    public bool can_switch_interact;
    public bool can_switch_mode;
    public bool in_exhibition_mode;

    // Use this for initialization
    void Start()
    {
        body_manager = manager.GetComponent<BodySourceManager>();
        left = "Lasso";
        right = "Lasso";
        field_of_view = 60;
        index = 0;

        holo_object = new GameObject[holo_objects.transform.childCount];

        for(int i = 0; i < holo_objects.transform.childCount; i++)
        {
            holo_object[i] = holo_objects.transform.GetChild(i).gameObject;
            holo_object[i].SetActive(false);
        }

        is_inside = false;
        can_switch_views = true;
        can_switch_to_next = true;
        can_switch_to_previous = true;
        can_reset = true;

        can_switch_mode = true;
        in_exhibition_mode = true;

        can_interact = true;
        can_switch_interact = true;

        top_cam.transform.position = top_pos.position;
        top_cam.transform.rotation = top_pos.rotation;
        bot_cam.transform.position = bot_pos.position;
        bot_cam.transform.rotation = bot_pos.rotation;
        left_cam.transform.position = left_pos.position;
        left_cam.transform.rotation = left_pos.rotation;
        right_cam.transform.position = right_pos.position;
        right_cam.transform.rotation = right_pos.rotation;

        current_object = holo_object[index];
        current_object.SetActive(true);
        //scale_Text.text = gameObject.transform.localScale.ToString();
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
            if (body.IsTracked && can_interact == true)
            {
                var pos_left = body.Joints[hand_left].Position;
                var pos_right = body.Joints[hand_right].Position;

                Vector3 vec_left = new Vector3(pos_left.X, pos_left.Y, pos_left.Z);
                Vector2 vec_right = new Vector3(pos_right.X, pos_right.Y, pos_right.Z);
                switch (body.HandRightState)
                {

                    case HandState.Open:
                        if (left == "Lasso")
                        {
                            if (holo_display.enabled == true)
                            {
                                gameObject.transform.Rotate(Vector3.up * 50 * Time.deltaTime * (-1));
                            }
                            else
                            {
                                top_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime * (-1));
                                bot_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime * (-1));
                                left_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime * (-1)); 
                                right_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime * (-1));
                            }
                        }
                        right = "Open";
                        break;
                    case HandState.Closed:
                        if (left == "Lasso")
                        {
                            if (holo_display.enabled == true)
                            {
                                gameObject.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
                            }
                            else
                            {
                                top_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                                bot_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                                left_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                                right_cam.transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                            }
                        }
                        right = "Closed";
                        break;
                    case HandState.Lasso:
                        right = "Lasso"; break;
                    default: break;
                }
                switch (body.HandLeftState)
                {
                    case HandState.Open:
                        if (right == "Lasso")
                        {
                            if (holo_display.enabled == true)
                            {
                                gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                            }
                            else
                            {
                                //Debug.Log(inside_cam.transform.position.z);
                                field_of_view += 1;
                                top_cam.fieldOfView = field_of_view;
                                bot_cam.fieldOfView = field_of_view;
                                left_cam.fieldOfView = field_of_view;
                                right_cam.fieldOfView = field_of_view;
                            }
                        }
                        left = "Open";
                        break;
                    case HandState.Closed:
                        if (right == "Lasso")
                        {
                            if (holo_display.enabled == true)
                            {
                                gameObject.transform.localScale -= new Vector3(0.025f, 0.025f, 0.025f);
                            }
                            else
                            {
                                field_of_view -= 1;
                                top_cam.fieldOfView = field_of_view;
                                bot_cam.fieldOfView = field_of_view;
                                left_cam.fieldOfView = field_of_view;
                                right_cam.fieldOfView = field_of_view;
                            }
                        }
                        left = "Closed";
                        break;
                    case HandState.Lasso:
                        Debug.Log("is in Lasso");
                        left = "Lasso"; break;
                    default: break;
                }
               // Debug.Log("Left hand: " + vec_left + " Right hand: " + vec_right);

                if (right == "Open" && left == "Open")
                {
                    if (holo_display.enabled == true)
                    {
                        gameObject.transform.Rotate(Vector3.left * 50 * Time.deltaTime);
                    }
                    else
                    {
                        top_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
                        bot_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
                        left_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
                        right_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
                    }
                    Debug.Log("Left is:" + left + "Right is:" + right);
                }
                if (right == "Closed" && left == "Closed")
                {
                    if (holo_display.enabled == true)
                    {
                        gameObject.transform.Rotate(Vector3.left * 50 * Time.deltaTime * (-1));
                    }
                    else
                    {
                        top_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime * (-1));
                        bot_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime * (-1));
                        left_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime * (-1));
                        right_cam.transform.Rotate(Vector3.up * 50 * Time.deltaTime * (-1));
                    }
                    Debug.Log("Left is:" + left + "Right is:" + right);
                }
                else if(right == "Closed" && left == "Open")
                {
                    if (can_switch_views == true)
                    {
                        is_inside = !is_inside;
                        OnEnter(is_inside);
                    }
                    can_switch_views = false;
                    Debug.Log("Left is:" + left + "Right is:" + right);
                }

                else if (right == "Open" && left == "Closed" && can_switch_interact == true)
                {
                    Debug.Log("Left is:" + left + "Right is:" + right);
                    //this.gameObject.GetComponent<Parts_Manager>().enabled = true;
                    // this.enabled = false;
                    //this.gameObject.GetComponent<Mode_Manager>().On_Parts_Switch();

                    //this.gameObject.GetComponent<Mode_Manager>().Switch_to_Parts();
                    //can_interact = false;
                    //Debug.Log("Parts manager enabled");
                    this.gameObject.GetComponent<Parts_Manager>().SetObject();
                    can_interact = false;
                    can_switch_interact = false;
                    //On_Mode_Switch();
                }

                else if(right == "Lasso" && left == "Lasso")
                {
                    if (vec_right.x >= 0.45f && vec_left.x >= -0.20f && can_switch_to_next == true)
                    {
                        OnNext();
                        can_switch_to_previous = true;
                        can_switch_to_next = false;
                        can_switch_interact = true;
                        Invoke("Reset_Switch_Next", 0.75f);
                    }

                    if(/*vec_left.x <= -0.45f &&*/ vec_right.x <= 0.1f && can_switch_to_previous == true)
                    {
                        Debug.Log("Previous switch");
                        OnPrevious();
                        can_switch_to_previous = false;
                        can_switch_to_next = true;
                        Invoke("Reset_Switch_Previous", 0.75f);
                    }

                    if((vec_left.x <= -0.45f && vec_right.x >= 0.45f && can_reset == true)
                        || (Input.GetKeyUp(KeyCode.Return) && can_reset == true))
                    {
                        Debug.Log("ABC");
                        can_switch_to_next = false;
                        can_switch_to_previous = false;
                        OnReset();
                        can_reset = false;
                        Invoke("Reset_Reset", 0.75f);
                    }
                    Debug.Log("Left is:" + left + "Right is:" + right);
                    can_switch_views = true;
                    can_switch_mode = true;
                    Debug.Log("Can switch");
                }
            }
        }
    }

    void OnEnter(bool inside)
    {
        if(inside == true)
        {
            top_cam.transform.position = inside_pos.position;
            top_cam.transform.rotation = inside_pos.rotation;

            bot_cam.transform.position = inside_pos.position;
            bot_cam.transform.rotation = Quaternion.Euler(180,
                inside_pos.transform.rotation.y, inside_pos.transform.rotation.z);

            left_cam.transform.position = inside_pos.position;
            left_cam.transform.rotation = Quaternion.Euler(180,
                inside_pos.transform.rotation.y, 90);

            right_cam.transform.position = inside_pos.position;
            right_cam.transform.rotation = Quaternion.Euler(180,
                inside_pos.transform.rotation.y, -90);
        }

        else
        {
            top_cam.transform.position = top_pos.position;
            top_cam.transform.rotation = top_pos.rotation;

            bot_cam.transform.position = bot_pos.position;
            bot_cam.transform.rotation = bot_pos.rotation;

            left_cam.transform.position = left_pos.position;
            left_cam.transform.rotation = left_pos.rotation;

            right_cam.transform.position = right_pos.position;
            right_cam.transform.rotation = right_pos.rotation;
        }
    }

    void OnNext()
    {
      
        if (index != holo_objects.transform.childCount - 1)
        {
            holo_object[index].SetActive(false);

            index += 1;
            holo_object[index].SetActive(true);
        }
        else
        {
            holo_object[index].SetActive(false);

            index = 0;
            holo_object[index].SetActive(true);
        }
        current_object = holo_object[index];
        Debug.Log("Just switched to next");
    }

    void OnPrevious()
    {
        if(index > 0)
        {
            holo_object[index].SetActive(false);

            index -= 1;
            holo_object[index].SetActive(true);
        }
        else
        {
            holo_object[index].SetActive(false);

            index = holo_objects.transform.childCount - 1;
            holo_object[index].SetActive(true);
        }
        current_object = holo_object[index];
        Debug.Log("Just switched to previous");
    }

    void OnReset()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 180);
        this.transform.localScale = new Vector3(1, 1, 1);
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

    void On_Mode_Switch()
    {
        if(can_switch_mode == true)
        {
            if(in_exhibition_mode == true)
            {
                top_cam.transform.position = bot_cam.transform.position;
                top_cam.transform.rotation = bot_cam.transform.rotation;

                left_cam.transform.position = bot_cam.transform.position;
                left_cam.transform.rotation = bot_cam.transform.rotation;

                right_cam.transform.position = bot_cam.transform.position;
                right_cam.transform.rotation = bot_cam.transform.rotation;

                can_switch_mode = false;
                in_exhibition_mode = false;
                Invoke("Mode_Switch_Reset", 1.0f);
            }
            else
            {
                top_cam.transform.position = top_pos.position;
                top_cam.transform.rotation = top_pos.rotation;

                bot_cam.transform.position = bot_pos.position;
                bot_cam.transform.rotation = bot_pos.rotation;

                left_cam.transform.position = left_pos.position;
                left_cam.transform.rotation = left_pos.rotation;

                right_cam.transform.position = right_pos.position;
                right_cam.transform.rotation = right_pos.rotation;

                in_exhibition_mode = true;
                can_switch_mode = false;
            }
        }
    }

}
