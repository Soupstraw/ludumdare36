using UnityEngine;
using System.Collections;

public class FlickerLight : MonoBehaviour
{

	public Light flickeringLight;

	public float flickerInterval = 0.2f;
	private float flickerTimer = 0.0f;

	private float intensity;
	private Color color;

	// Use this for initialization
	void Start ()
	{
	
	}

	void Update ()
	{
		flickerTimer -= Time.deltaTime;
		if (flickerTimer <= 0.0f) {
			flickerTimer = flickerInterval;

			intensity = 1 + (Random.value - 0.5f) * 0.1f;
			color = Random.ColorHSV (
				0.0f, 0.05f,
				0.0f, 0.05f,
				0.95f, 1.0f
			);
		}

		flickeringLight.intensity = (flickeringLight.intensity + intensity) / 2.0f;
		flickeringLight.color = (flickeringLight.color + color) / 2.0f;
	}
}
