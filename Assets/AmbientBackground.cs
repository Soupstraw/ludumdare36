using UnityEngine;
using System.Collections;

public class AmbientBackground : MonoBehaviour
{
	public GameObject background;

	public AnimationCurve crossfadeCurve;
	public float crossfadeDuration = 3f;
	[Range (0f, 1f)] private float crossfade = 0.0f;

	private Texture2D fadeTo;

	void Start ()
	{
		SwitchTo ("Generic");
	}

	void Update ()
	{
		if (fadeTo == null) {
			return;
		}
		crossfade += Time.deltaTime / crossfadeDuration;

		float p = crossfadeCurve.Evaluate (crossfade);
		p = Mathf.Clamp01 (p);
		Renderer renderer = background.GetComponent<Renderer> ();
		renderer.material.SetFloat ("_Crossfade", p);

		if (crossfade >= 1.0f) {
			renderer.material.SetTexture ("_MainTex", fadeTo);
			fadeTo = null;
		}
	}

	public void ChangeEnvironment (Game.Card.Environment environment)
	{
		SwitchTo (environment.ToString ());
	}

	void SwitchTo (string name)
	{
		string path = "Environment/" + name;
		Texture2D tex = Resources.Load<Texture2D> (path);
		if (tex != null) {
			ChangeTo (tex);
		}
	}

	void ChangeTo (Texture2D tex)
	{
		fadeTo = tex;

		Renderer renderer = background.GetComponent<Renderer> ();
		renderer.material.SetTexture ("_FadeTex", fadeTo);
		crossfade = 0.0f;
		renderer.material.SetFloat ("_Crossfade", crossfade);
	}
}
