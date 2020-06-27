using UnityEngine;
using System.Collections;

public class Holo_Conroller : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		speed = 200.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.LeftArrow)) {
			this.gameObject.transform.Rotate (Vector3.up * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			this.gameObject.transform.Rotate (Vector3.up * speed * Time.deltaTime * (-1));
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			this.gameObject.transform.Rotate (Vector3.left * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			this.gameObject.transform.Rotate (Vector3.left * speed * Time.deltaTime * (-1));
		}
		if (Input.GetKey (KeyCode.W)) {
			this.transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
		}
		if (Input.GetKey (KeyCode.S)) {
			this.transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
		}
	}
}
