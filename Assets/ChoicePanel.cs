using UnityEngine;
using System.Collections;

public class ChoicePanel : MonoBehaviour {

	public float target = 0;
	public float lerpFactor = 0.2f;
	public float stoppingPoint = 0.3f;

	void Start(){
		
	}

	// Update is called once per frame
	void Update () {
		if (target > 0) {
			target = Mathf.Min (target, Screen.width * stoppingPoint);
		} else {
			target = Mathf.Max (target, - Screen.width * stoppingPoint);
		}
		transform.position = Vector3.Lerp (transform.position, new Vector3(target + Screen.width/2, transform.position.y), lerpFactor);
	}
}
