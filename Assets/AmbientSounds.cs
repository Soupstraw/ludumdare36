using UnityEngine;
using System.Collections;

public class AmbientSounds : MonoBehaviour
{
	public AnimationCurve crossfadeCurve;
	public float crossfadeDuration = 3f;
	[Range (0f, 1f)] private float crossfade = 0.0f;

	[Range (0f, 1f)] public float volume = 0.3f;

	private AudioSource primary;
	private AudioSource secondary;

	void Start ()
	{
		primary = gameObject.AddComponent<AudioSource> ();
		primary.loop = true;

		secondary = gameObject.AddComponent<AudioSource> ();
		secondary.loop = true;
	}

	public void ChangeEnvironment (Game.Card.Environment environment)
	{
		string path = "Ambient/" + environment.ToString ();
		AudioClip clip = Resources.Load<AudioClip> (path);
		if (clip != null) {
			ChangeTo (clip);
		}
	}

	void ChangeTo (AudioClip song)
	{
		secondary.clip = song;
		secondary.volume = 0.0f;
		secondary.time = 0.0f;
		secondary.Play ();

		crossfade = 0.0f;
	}

	void Update ()
	{
		if (secondary.clip == null) {
			return;
		}
		crossfade += Time.deltaTime / crossfadeDuration;

		float p = crossfadeCurve.Evaluate (crossfade);
		p = Mathf.Clamp01 (p);

		primary.volume = (1.0f - p) * volume;
		secondary.volume = (p) * volume;

		if (p >= 1.0f) {
			AudioSource tmp = secondary;
			secondary = primary;
			primary = tmp;

			secondary.Stop ();
			secondary.time = 0.0f;
			secondary.volume = 0.0f;
			secondary.clip = null;
		}
	}
}

