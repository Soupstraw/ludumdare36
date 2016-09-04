using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
	// -1 = no, 0 = center, 1 = yes
	public delegate void TriggerAction (int choice);

	public event TriggerAction OnTrigger;

	public struct XForm
	{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 localScale;

		public XForm (GameObject obj)
		{
			position = obj.transform.position;
			rotation = obj.transform.rotation;
			localScale = obj.transform.localScale;
		}

		public XForm (XForm form)
		{
			position = form.position;
			rotation = form.rotation;
			localScale = form.localScale;
		}

		public XForm TransformXZRY (float dx, float dz, float ry)
		{
			XForm xform = new XForm (this);

			xform.position.x += dx;
			xform.position.z += dz;

			Vector3 rot = rotation.eulerAngles;
			xform.rotation = Quaternion.Euler (new Vector3 (rot.x, rot.y + ry, rot.z));

			return xform;
		}

		public void MoveObject (GameObject obj, float transitionSpeed)
		{
			obj.transform.position = Vector3.Lerp (obj.transform.position, position, transitionSpeed);
			obj.transform.rotation = Quaternion.Slerp (obj.transform.rotation, rotation, transitionSpeed);
			obj.transform.localScale = Vector3.Lerp (obj.transform.localScale, localScale, transitionSpeed);
		}

		public static XForm Lerp (XForm a, XForm b, float p)
		{
			XForm xform = new XForm ();
			xform.position = Vector3.Lerp (a.position, b.position, p);
			xform.rotation = Quaternion.Slerp (a.rotation, b.rotation, p);
			xform.localScale = Vector3.Lerp (a.localScale, b.localScale, p);
			return xform;
		}
	}

	public GameObject Card;
	public GameObject Yes;
	public GameObject No;

	public GameObject CardStart;
	public GameObject CardFinish;

	public AnimationCurve animationCurve = AnimationCurve.EaseInOut (0.0f, 0.0f, 1.0f, 1.0f);
	public float animationDuration = 0.8f;
	private float animationProgress = 0.0f;

	private XForm cardAnimationStart;
	private XForm cardAnimationFinish;
	private XForm cardCenter;

	private XForm yesZero;
	private XForm noZero;

	public enum State
	{
		// Possible state transitions
		// Image -> Description
		// Description -> Image2 or Option
		// Image2 -> Description or Option2
		// Option, Option2 -> Dismiss
		// Dismiss -> Image
		Image,
		Description,
		Option,
		Image2,
		Option2,
		Dismiss
	}

	public State state;

	void Start ()
	{
		cardCenter = new XForm (Card);
		cardAnimationStart = new XForm (Card);
		cardAnimationFinish = new XForm (Card);

		yesZero = new XForm (Yes);
		noZero = new XForm (No);

		SetState (State.Image);
	}

	public bool Animating ()
	{
		return animationProgress <= 1.0f;
	}

	// normalized input
	private bool mouseDragging = false;

	private Vector3 draggingCenter = new Vector3 ();

	[Range (0.0f, 1.0f)] public float MouseThreshold = 0.4f;

	[Range (0.0f, 1.0f)] public float TiltOffset = 0.65f;
	[Range (0.0f, 90.0f)] public float TiltRotation = 65.0f;

	public void SetState (State newState)
	{
		state = newState;
		animationProgress = 0.0f;

		switch (state) {
		case State.Image:
			cardAnimationStart = new XForm (CardStart);
			cardAnimationFinish = new XForm (cardCenter);
			break;

		case State.Description:
			cardAnimationStart = new XForm (Card);
			cardAnimationFinish = new XForm (cardCenter).TransformXZRY (0f, 0.0f, 180f);
			break;

		case State.Option:
			cardAnimationStart = new XForm (Card);
			cardAnimationFinish = new XForm (cardCenter).TransformXZRY (0f, 0.0f, 360f);
			break;

		case State.Image2:
			cardAnimationStart = new XForm (Card);
			cardAnimationFinish = new XForm (cardCenter);
			break;

		case State.Option2:
			cardAnimationStart = new XForm (Card);
			cardAnimationFinish = new XForm (cardCenter).TransformXZRY (0f, 0.0f, 180f);
			break;
		
		case State.Dismiss:
			cardAnimationStart = new XForm (Card);
			cardAnimationFinish = new XForm (CardFinish);
			break;
		}

		// jump to start position
		cardAnimationStart.MoveObject (Card, 1.0f);
	}

	void Update ()
	{
		animationProgress += Time.deltaTime / animationDuration;

		float transitionSpeed = 10.0f * Time.deltaTime;
	
		// normalized mouse/finger position on the screen
		Vector3 pos = new Vector3 ();

		{ // Calculate the normalized position in the Card center area
			Vector3 npos = Input.mousePosition;

			// normalized position in the area
			npos.x = 2.0f * (npos.x - Screen.width / 2.0f) / (0.4f * Screen.height);
			npos.y = 2.0f * (npos.y - Screen.height / 2.0f) / Screen.height;

			npos.x = Mathf.Clamp (npos.x, -1.0f, 1.0f);
			npos.y = Mathf.Clamp (npos.y, -1.0f, 1.0f);

			// move the current mouse center
			if (!mouseDragging) {
				draggingCenter = npos;
			}

			// try to move starting center towards screen center
			draggingCenter.x = HomeToCenter (draggingCenter.x, npos.x, transitionSpeed);
			draggingCenter.y = HomeToCenter (draggingCenter.y, npos.y, transitionSpeed);

			// calculate position relative to dragging center
			pos.x = ValueRelativeTo (draggingCenter.x, npos.x);
			pos.y = ValueRelativeTo (draggingCenter.y, npos.y);
		}

		{ // Update mouse and handle trigger
			bool pressed = Input.GetButtonDown ("Fire1");
			bool released = Input.GetButtonUp ("Fire1");
			if (pressed) {
				mouseDragging = true;
			}
			if (released) {
				mouseDragging = false;

				if (released) {
					if (pos.x < -MouseThreshold) {
						Trigger (1);
					} else if (pos.x > MouseThreshold) {
						Trigger (-1);
					} else {
						Trigger (0);
					}
				}
			}
		}

		{ // Update Card position
			if (Animating ()) {
				float p = animationCurve.Evaluate (animationProgress);
				XForm target = XForm.Lerp (cardAnimationStart, cardAnimationFinish, p);
				target.MoveObject (Card, transitionSpeed);
			} else {
				XForm center = cardAnimationFinish;
				XForm tilted = new XForm (center);

				float tilt = Mathf.Abs (pos.x);
				if (mouseDragging) {
					switch (state) {
					case State.Image:
						tilted = pos.x < 0 ? 
							center.TransformXZRY (0.0f, 0.0f, TiltRotation) : 
							center.TransformXZRY (0.0f, 0.0f, -TiltRotation);
						break;

					case State.Description:
					case State.Image2:
						tilted = pos.x < 0 ? 
							center.TransformXZRY (-TiltOffset, -0.4f, -TiltRotation) : 
							center.TransformXZRY (TiltOffset, -0.4f, TiltRotation);

						if (tilt > MouseThreshold) {
							tilt = 1.0f;
						}
						break;

					case State.Option:
					case State.Option2:
						tilted.position = Vector3.Lerp (center.position, CardFinish.transform.position, 0.5f);
						tilt = Mathf.Abs (pos.y) + Mathf.Abs (pos.x);
						break;
					}
				}

				XForm target = XForm.Lerp (center, tilted, tilt);
				target.MoveObject (Card, transitionSpeed);
			}
		}

		{ // Update Yes/No
			// update alpha
			UnityEngine.UI.Text yesText = Yes.GetComponentInChildren<UnityEngine.UI.Text> ();
			UnityEngine.UI.Text noText = No.GetComponentInChildren<UnityEngine.UI.Text> ();

			XForm yesStart = new XForm (yesZero);
			XForm noStart = new XForm (noZero);

			XForm yesFinish = new XForm (yesZero);
			XForm noFinish = new XForm (noZero);

			float yesAlpha = 0.0f;
			float noAlpha = 0.0f;

			if (state == State.Description || state == State.Image2) {
				if (pos.x < 0.0f) {
					yesFinish = new XForm (cardCenter);
					yesFinish.position.x = -0.1f;
				} else {
					noFinish = new XForm (cardCenter);
					noFinish.position.x = 0.1f;
				}

				if (pos.x < -MouseThreshold) {
					yesAlpha = 1.0f;
				} else if (pos.x > MouseThreshold) {
					noAlpha = 1.0f;
				}
			}

			LerpAlphaTowards (yesText, yesAlpha, transitionSpeed);
			LerpAlphaTowards (noText, noAlpha, transitionSpeed);

			float p = Mathf.Abs (pos.x);
			if (p > MouseThreshold) {
				p = 1.0f;	
			}
			XForm yesTarget = XForm.Lerp (yesStart, yesFinish, p);
			XForm noTarget = XForm.Lerp (noStart, noFinish, p);

			yesTarget.MoveObject (Yes, transitionSpeed);
			noTarget.MoveObject (No, transitionSpeed);
		}
	}

	private void Trigger (int choice)
	{
		if (Animating ()) {
			return;
		}
		if (OnTrigger != null) {
			OnTrigger (choice);
		}
	}

	// moves center towards <0,0> when p < center
	private float HomeToCenter (float center, float p, float transitionSpeed)
	{
		if (Mathf.Abs (center) > Mathf.Abs (p)) {
			return Mathf.Lerp (center, p, transitionSpeed);
		}
		return center;
	}

	// center in [-1,1]
	// p      in [-1,1]
	// return in [-1,1], such that
	//   p < center == return < 0
	private float ValueRelativeTo (float center, float p)
	{
		float delta = p - center;
		float scale = delta < 0.0f ? center + 1.0f : 1.0f - center;
		if (Mathf.Abs (scale) < 0.01f) {
			return Mathf.Clamp (delta, -1.0f, 1.0f);
		}
		return Mathf.Clamp (delta / scale, -1.0f, 1.0f);
	}

	private void LerpAlphaTowards (UnityEngine.UI.Text text, float target, float transitionSpeed)
	{
		Color value = text.color;
		value.a = Mathf.Lerp (value.a, target, transitionSpeed);
		text.color = value;
	}
}
