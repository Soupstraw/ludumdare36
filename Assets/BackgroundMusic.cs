using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
	public AudioClip[] tracks;
	private int activeTrack = -1;

	public AnimationCurve crossfadeCurve;
	public float crossfadeDuration = 3f;
	[Range (0f, 1f)] private float crossfade = 0.0f;

	[Range (0f, 1f)] public float volume = 1f;

	private AudioSource primary;
	private AudioSource secondary;

	void Start ()
	{
		primary = gameObject.AddComponent<AudioSource> ();
		primary.loop = true;

		secondary = gameObject.AddComponent<AudioSource> ();
		secondary.loop = true;

		Next ();
	}

	void Next ()
	{
		activeTrack = (activeTrack + 1) % tracks.Length;
		ChangeTo (tracks [activeTrack]);
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
			if (primary.clip.length - primary.time <= crossfadeDuration) {
				Next ();
			}
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

