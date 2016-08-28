using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof(EventTrigger))]
public class SlidingScrollPanel : MonoBehaviour {

	public delegate void DialogAction ();
	public static event DialogAction OnDialogDismissed;

	public float defaultY;
	public float stabilizeLerp = 0.2f;
	public float dismissThreshold = -10f;
	public float dragRatio = 2f;

	private ScrollRect scrollRect; 

	private bool dragging = false;

	// Use this for initialization
	void Start () {
		transform.parent.position = new Vector3 (transform.parent.position.x, defaultY);

		scrollRect = GetComponent<ScrollRect> ();
		EventTrigger trigger = GetComponent<EventTrigger> ();

		EventTrigger.Entry pointerDragBegin = new EventTrigger.Entry ();
		pointerDragBegin.eventID = EventTriggerType.Drag;
		pointerDragBegin.callback.AddListener ((data) => OnPointerDragBegin((PointerEventData) data));
		trigger.triggers.Add (pointerDragBegin);

		EventTrigger.Entry pointerDrag = new EventTrigger.Entry ();
		pointerDrag.eventID = EventTriggerType.Drag;
		pointerDrag.callback.AddListener ((data) => OnPointerDrag((PointerEventData) data));
		trigger.triggers.Add (pointerDrag);

		EventTrigger.Entry pointerRelease = new EventTrigger.Entry ();
		pointerRelease.eventID = EventTriggerType.EndDrag;
		pointerRelease.callback.AddListener ((data) => OnPointerRelease((PointerEventData) data));
		trigger.triggers.Add (pointerRelease);
	}

	void Update(){
		if (!dragging) {
			Stabilize ();
		}
	}

	private void Stabilize() {
		transform.parent.position = Vector3.Lerp (transform.parent.position, new Vector3(transform.parent.position.x, defaultY), stabilizeLerp);
	}

	public void OnPointerDragBegin(PointerEventData ev){
		dragging = true;
	}

	public void OnPointerDrag(PointerEventData ev){
		if (scrollRect.verticalNormalizedPosition == 1f || transform.parent.position.y <= defaultY) {
			transform.parent.position += new Vector3 (0, ev.delta.y * dragRatio);
			if (transform.parent.position.y < dismissThreshold - Screen.height / 2) {
				if (OnDialogDismissed != null) {
					OnDialogDismissed ();
				}
				transform.parent.gameObject.SetActive (false);
			}
		}
	}

	public void OnPointerRelease(PointerEventData ev){
		dragging = false;
	}
}
