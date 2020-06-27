using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class Body_Track : MonoBehaviour {

    public Body[] _bodies;
    public Canvas canvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
    {
        var reference = e.FrameReference.AcquireFrame();

        // Color
        // Display the color stream...

        // Body
        using (var frame = reference.BodyFrameReference.AcquireFrame())
        {
            if (frame != null)
            {
                _bodies = new Body[frame.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(_bodies);

                foreach (var body in _bodies)
                {
                    if (body != null)
                    {
                        if (body.IsTracked)
                        {
                            // Find the joints
                            Windows.Kinect.Joint handRight = body.Joints[JointType.HandRight];
                            Windows.Kinect.Joint thumbRight = body.Joints[JointType.ThumbRight];

                            Windows.Kinect.Joint handLeft = body.Joints[JointType.HandLeft];
                            Windows.Kinect.Joint thumbLeft = body.Joints[JointType.ThumbLeft];
                        }
                    }
                }
            }
        }
    }
}
