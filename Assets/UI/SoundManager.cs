using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	public enum ambientSoundInfo {Forest, Swamp, Town, Silence};
	public ambientSoundInfo ambientInfo = ambientSoundInfo.Silence;

	public AnimationCurve interpolationCurve; // Defines interpolation
	public AudioClip defaultSound; // starting music
	public AudioClip defaultSound2; // starting music
	public AudioClip defaultSound3; // starting music
	public float interpolationTime = 3f; // Defines the amount of time needed to go from 0 to 1 volume.

	[Range(0f, 1f)] public float ambientVolume = 0.3f;
	[Range(0f, 1f)] public float mainVolume = 1f;
	[Range(0f, 1f)] public float masterVolume = 1f;

	[Header("Ambient sound effects")]
	public AudioClip ambientWalkingRain;
	public AudioClip ambientWalkingBirds;
	public AudioClip ambientWind;
	public AudioClip ambientFrogs;

	[Header("Card Sounds")]
	public AudioClip[] CardSounds;

	[Header("Music Clips")]
	public AudioClip[] Music;

	private int musicIndex = 0;

	// These are used to interpolate songs between
	private AudioSource mainPlayer;
	private AudioSource secondaryPlayer; 

	private AudioSource ambientMainPlayer;
	private AudioSource ambientSecondaryPlayer;

	private AudioSource cardFlipper;


	// keep track to which track we are interpolating
	private AudioClip primarySound;
	private AudioClip secondarySound;

	private float timeSinceSwitch;

	private bool changedEnvironments;

	// Use this for initialization
	void Start () {

		// Build the gameObject.
		mainPlayer = gameObject.AddComponent<AudioSource> ();
		secondaryPlayer = gameObject.AddComponent<AudioSource> ();

		ambientMainPlayer = gameObject.AddComponent<AudioSource> ();
		ambientSecondaryPlayer = gameObject.AddComponent<AudioSource> ();

		cardFlipper = gameObject.AddComponent<AudioSource> ();

		// Everything but card sounds loops
		mainPlayer.loop = true;
		secondaryPlayer.loop = true;
		ambientMainPlayer.loop = true;
		ambientSecondaryPlayer.loop = true;

		// Initialize the mainPlayer
		mainPlayer.clip = defaultSound;	// In the beginning there should be somekind of default music
		mainPlayer.volume = masterVolume * mainVolume;
		mainPlayer.Play (); // Start playing this song.

		ambientInfo = ambientSoundInfo.Silence;

		InvokeRepeating ("NextSong", 0f, 45f);
	}

	private void NextSong() {
		musicIndex = (musicIndex + 1) % this.Music.Length;
		this.ChangeSong (this.Music [musicIndex], this.secondaryPlayer);
	}

	void OnEnable(){
		CardInteraction.OnCardPushedAside += CardFlipSound;
		SlidingScrollPanel.OnDialogDismissed += CardFlipSound;
	}

	void OnDisable(){
		CardInteraction.OnCardPushedAside -= CardFlipSound;
		SlidingScrollPanel.OnDialogDismissed -= CardFlipSound;
	}
	
	// Update is called once per frame
	void Update () {


		// Place holder for testing
		if (defaultSound2 != null) {
			ChangeSong (defaultSound2, this.secondaryPlayer);
			defaultSound2 = null;
		}

		if (defaultSound3 != null) {
			ChangeAmbient (ambientSoundInfo.Swamp);
			ChangeSong (defaultSound3, this.ambientSecondaryPlayer);

			defaultSound3 = null;
		}

		//


		if (changedEnvironments) { 
			switch (ambientInfo) {
				case ambientSoundInfo.Silence:
					break;
				case ambientSoundInfo.Forest:
					ChangeSong (ambientWalkingBirds, ambientSecondaryPlayer);
					break;
				case ambientSoundInfo.Swamp:
					ChangeSong (ambientFrogs, ambientSecondaryPlayer);
					break;
				case ambientSoundInfo.Town:
					ChangeSong (ambientWind, ambientSecondaryPlayer);
					break;
			}
			changedEnvironments = false;
		}

		InterpolateSongs (this.mainPlayer, this.secondaryPlayer, maximumVolume: mainVolume * masterVolume);
		InterpolateSongs (this.ambientMainPlayer, this.ambientSecondaryPlayer, maximumVolume: 0.3f * ambientVolume * masterVolume);

	} 

	public void ChangeSong (AudioClip newSong, AudioSource secondaryPlayer) {
		secondaryPlayer.clip = newSong;
		secondaryPlayer.volume = 0f;
		secondaryPlayer.Play ();

		timeSinceSwitch = 0f;
	}

	public void ChangeAmbient(ambientSoundInfo ambientInfo) {
		changedEnvironments = true;
		this.ambientInfo = ambientInfo;
	}

	public void CardFlipSound() {
		cardFlipper.clip = CardSounds [Random.Range (0, CardSounds.Length)];
		cardFlipper.pitch = Random.value * 0.2f + 0.9f; // Range[0.95, 1.05]
		cardFlipper.Play();
	}

	private void InterpolateSongs(AudioSource mainPlayer, AudioSource secondaryPlayer, float maximumVolume = 1f) {

		// If the interpolation is not complete.
		if (secondaryPlayer.clip != null) {

			// Keep track of time
			timeSinceSwitch += Time.deltaTime;

			// Interpolate the volume
			float percentageInterpolationDone = timeSinceSwitch / interpolationTime;
			mainPlayer.volume = interpolationCurve.Evaluate (1 - percentageInterpolationDone) * maximumVolume; // make quieter as the interpolation is in action
			secondaryPlayer.volume = interpolationCurve.Evaluate(percentageInterpolationDone) * maximumVolume; // make louder

			// If the interpolation is done
			if (percentageInterpolationDone >= 1f) {

				// mainPlayer = secondaryPlayer
				mainPlayer.clip = secondaryPlayer.clip;
				mainPlayer.volume = secondaryPlayer.volume;
				mainPlayer.time = secondaryPlayer.time;
				mainPlayer.Play ();

				// secondaryPlayer = none
				secondaryPlayer.clip = null;
				secondaryPlayer.volume = 0f;
				secondaryPlayer.time = 0f;
				secondaryPlayer.Stop();

				// restart the variables
				timeSinceSwitch = 0f;
			}
			
		}
	} 
}
