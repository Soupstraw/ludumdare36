using UnityEngine;
using System.Collections;

public class CardSounds : MonoBehaviour
{
	private AudioSource source;

	public AudioClip[] Sounds;

	// Use this for initialization
	void Start ()
	{
		source = gameObject.AddComponent<AudioSource> ();
	}

	private void PlayRandom ()
	{
		source.clip = Sounds [Random.Range (0, Sounds.Length)];
		source.pitch = Random.Range (0.9f, 1.1f);
		source.Play ();
	}

	public void NewStoryCard ()
	{
		PlayRandom ();
	}

	public void FlipStoryCard ()
	{
		PlayRandom ();
	}

	public void SelectOption ()
	{
		PlayRandom ();
	}
}
