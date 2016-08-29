using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof(EventTrigger))]
public class FadingPanel : MonoBehaviour {

	public delegate void DialogAction ();
	public static event DialogAction OnDialogDismissed;

	public Text target;
	public float sensitivity = 0.1f;
	public float stabilizeLerpFactor = 0.5f;

	private bool dragging = false;
	private bool visible = false;

	void Start () {
		ChangeAlpha (0f);

		EventTrigger trigger = GetComponent<EventTrigger> ();

		EventTrigger.Entry pointerDrag = new EventTrigger.Entry ();
		pointerDrag.eventID = EventTriggerType.Drag;
		pointerDrag.callback.AddListener ((data) => OnPointerDrag((PointerEventData) data));
		trigger.triggers.Add (pointerDrag);

		EventTrigger.Entry pointerRelease = new EventTrigger.Entry ();
		pointerRelease.eventID = EventTriggerType.EndDrag;
		pointerRelease.callback.AddListener ((data) => OnPointerRelease((PointerEventData) data));
		trigger.triggers.Add (pointerRelease);
	}

	void OnEnable(){
		CardInteraction.OnCardPushedAside += FadeIn;
	}

	void OnDisable(){
		CardInteraction.OnCardPushedAside -= FadeIn;
	}

	public void FadeIn(){
		visible = true;
	}

	void Update(){
		if (!dragging && visible) {
			ChangeAlpha (Mathf.Lerp(target.color.a, 1, stabilizeLerpFactor));
		}

		if (Input.GetButtonDown ("Jump") && Debug.isDebugBuild) {
			ChangeAlpha (0);
			visible = false;
			if(OnDialogDismissed != null){
				OnDialogDismissed ();
			}
		}
	}

	public void OnPointerDrag(PointerEventData ev){
		if (visible) {
			dragging = true;
			ChangeAlpha (Mathf.Clamp01 (1 - Vector2.Distance (ev.pressPosition, ev.position) / Screen.width * sensitivity));
		}
	}

	public void OnPointerRelease(PointerEventData ev){
		if (visible) {
			if (Vector2.Distance (ev.pressPosition, ev.position) >= Screen.width / sensitivity) {
				visible = false;
				ChangeAlpha (0);
				if (OnDialogDismissed != null) {
					OnDialogDismissed ();
				}
			}
			dragging = false;
		}
	}

	private void ChangeAlpha(float alpha){
		Color c = target.color;
		target.color = new Color(c.r, c.g, c.b, alpha);
	}
}
