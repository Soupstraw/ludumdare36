using UnityEngine;
using System.Collections;

public class ChoicePanel : MonoBehaviour {

	public float target = 0;
	public float lerpFactor = 0.2f;

	void Start(){
		
	}

	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, new Vector3(target + Screen.width/2, transform.position.y), lerpFactor);
	}
}
