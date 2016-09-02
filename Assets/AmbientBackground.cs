using UnityEngine;
using System.Collections;

public class AmbientBackground : MonoBehaviour
{
	public GameObject background;

	public AnimationCurve crossfadeCurve;
	public float crossfadeDuration = 3f;
	[Range (0f, 1f)] private float crossfade = 0.0f;

	void Start ()
	{
		SwitchTo ("Generic");
	}

	void Update ()
	{
	
	}

	public void ChangeEnvironment (Game.Card.Environment environment)
	{
		SwitchTo (environment.ToString ());
	}

	void SwitchTo (string name)
	{
		string path = "Environment/" + name;
		Texture2D tex = Resources.Load<Texture2D> (path);

		Renderer renderer = background.GetComponent<Renderer> ();
		if (tex != null) {
			renderer.material.mainTexture = tex;
		}
	}
}
