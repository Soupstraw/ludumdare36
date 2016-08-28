using UnityEngine;
using System.Collections;

public class ChoicePanel : MonoBehaviour {

	public float target = 0;
	public float lerpFactor = 0.2f;
	public float stoppingPoint = 0.3f;
	public float sensitivity = 10.0f;

	void Start(){
		
	}

	// Update is called once per frame
	void Update () {
		target = Mathf.Clamp (target, -stoppingPoint * Screen.width, stoppingPoint * Screen.width);
		transform.position = Vector3.Lerp (transform.position, new Vector3((target * sensitivity + Screen.width/2), transform.position.y), lerpFactor);
	}
}
