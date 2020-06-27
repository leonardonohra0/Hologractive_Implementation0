using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System.Linq;

public class Voice_Commands : MonoBehaviour {

    public Transform inside_pos;
    public Transform top_pos, bot_pos, left_pos, right_pos;
    public GameObject porsche, fridge, main_cam;

    public Camera top_cam, bot_cam, right_cam, left_cam;
    public Canvas holo_display;
    Dictionary<string, System.Action> commands = new Dictionary<string, System.Action>();
    KeywordRecognizer recognizer = null;

    public bool is_inside;
    public int index;

    // Use this for initialization
    void Start() {
        is_inside = false;
        index = 0;

        top_cam.transform.position = top_pos.position;
        top_cam.transform.rotation = top_pos.rotation;
        bot_cam.transform.position = bot_pos.position;
        bot_cam.transform.rotation = bot_pos.rotation;
        left_cam.transform.position = left_pos.position;
        left_cam.transform.rotation = left_pos.rotation;
        right_cam.transform.position = right_pos.position;
        right_cam.transform.rotation = right_pos.rotation;

        commands.Add("go in", () =>
        {
            this.OnEnterCommand();
        });

        commands.Add("get out", () =>
        {
            this.OnExitCommand();
        });

        commands.Add("go to next", () =>
        {
            this.OnNextCommand();
        });

        commands.Add("back", () =>
        {
            this.OnPreviousCommand();
        });

        commands.Add("Basic", () =>
        {
            this.OnReset();
        });

        commands.Add("start", () =>
        {
            this.OnMotionSwitch();
            Debug.Log("Switch to motion");
        });

        recognizer = new KeywordRecognizer(commands.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnCommandRecognised;
        recognizer.Start();
        Debug.Log("Enter and exit commands have been added");
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnCommandRecognised(PhraseRecognizedEventArgs args)
    {
        System.Action key_Action;
        if (commands.TryGetValue(args.text, out key_Action))
            key_Action.Invoke();
    }

    void OnEnterCommand()
    {
        top_cam.transform.position = inside_pos.position;
        top_cam.transform.rotation = inside_pos.rotation;

        bot_cam.transform.position = inside_pos.position;
       // bot_cam.transform.rotation = inside_pos.rotation;
        left_cam.transform.position = inside_pos.position;
        //left_cam.transform.rotation = inside_pos.rotation;
        right_cam.transform.position = inside_pos.position;
        //right_cam.transform.rotation = inside_pos.rotation;

        bot_cam.transform.rotation = Quaternion.Euler(180,
            inside_pos.transform.rotation.y, inside_pos.transform.rotation.z);
        left_cam.transform.rotation = Quaternion.Euler(180,
            inside_pos.transform.rotation.y, 90);
        right_cam.transform.rotation = Quaternion.Euler(180,
            inside_pos.transform.rotation.y, -90);

        /*top_cam.GetComponent<Light>().intensity = 1;
        left_cam.GetComponent<Light>().intensity = 1;
        bot_cam.GetComponent<Light>().intensity = 1;
        right_cam.GetComponent<Light>().intensity = 1;
        */
        Debug.Log("Just said enter");
    }

    void OnExitCommand()
    {
        holo_display.enabled = true;
        is_inside = false;
        
        /*
        top_cam.GetComponent<Light>().intensity = 8;
        left_cam.GetComponent<Light>().intensity = 8;
        bot_cam.GetComponent<Light>().intensity = 8;
        right_cam.GetComponent<Light>().intensity = 8;
        */
        Debug.Log("Just said exit");
    }

    void OnNextCommand()
    {
        porsche.SetActive(false);
        fridge.SetActive(true);
        Debug.Log("just said next");
    }

    void OnPreviousCommand()
    {
        porsche.SetActive(true);
        fridge.SetActive(false);
        Debug.Log("just said previous");
    }

    void OnReset()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    void OnMotionSwitch()
    {
        main_cam.gameObject.GetComponent<BodySourceManager>().enabled = true;
        this.gameObject.GetComponent<Holo_Motion_Control>().enabled = true;
        this.gameObject.GetComponent<Voice_Commands>().enabled = false;
    }
}
