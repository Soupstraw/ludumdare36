using System;

public class BackgroundMusic
{
	public AudioClip[] tracks;
	public int activeTrack = 0;

	public AnimationCurve interpolationCurve;
	public float interpolationTime = 3f;

	[Range (0f, 1f)] public float volume = 1f;

	private AudioSource main;
	private AudioSource secondary;

	public BackgroundMusic ()
	{
		
	}
}

