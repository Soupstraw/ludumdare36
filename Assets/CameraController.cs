using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Camera mainCamera;
	public GameObject target;

	// Use this for initialization
	void Start ()
	{
		mainCamera = gameObject.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var taspect = target.transform.localScale.x / target.transform.localScale.y;
		var caspect = mainCamera.aspect;

		if (taspect < caspect) {
			var fov = mainCamera.fieldOfView * Mathf.Deg2Rad;
			var dz = -target.transform.localScale.y * 0.5f / Mathf.Tan (fov * 0.5f);
			var z = target.transform.position.z;
			transform.position = new Vector3 (0, 0, dz + z);
		} else {
			var fov = mainCamera.fieldOfView * Mathf.Deg2Rad * caspect;
			var dz = -target.transform.localScale.x * 0.5f / Mathf.Tan (fov * 0.5f);
			var z = target.transform.position.z;
			transform.position = new Vector3 (0, 0, dz + z);
		}
	}
}
