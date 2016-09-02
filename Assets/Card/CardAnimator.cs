using UnityEngine;
using System.Collections;

public class CardAnimator : MonoBehaviour
{
	public GameObject StoryCard;
	public GameObject YesCard;
	public GameObject NoCard;

	public GameObject StartPosition;
	public GameObject EndPosition;

	public State state;

	public AnimationCurve animationCurve;
	public float animationDuration = 1.0f;
	private float animationProgress = 0.0f;

	[Range (0f, 1f)] public float animationStabilization = 0.5f;

	public bool animating ()
	{
		return animationProgress < 1.0f;
	}

	private struct Position
	{
		public Vector3 position;
		public Quaternion rotation;

		public Position (Position pos)
		{
			position = pos.position;
			rotation = pos.rotation;
		}

		public Position (GameObject obj)
		{
			position = obj.transform.position;
			rotation = obj.transform.rotation;
		}

		public Position RotateY (float amount)
		{
			Position pos = new Position (this);
			Vector3 rot = rotation.eulerAngles;
			pos.rotation = Quaternion.Euler (new Vector3 (rot.x, rot.y + amount, rot.z));
			return pos;
		}
	}

	private class Animation
	{
		public GameObject card;

		// current animation start/stop positions
		public Position start = new Position ();
		public Position stop = new Position ();

		// all possible animation targets
		public Position[] targets;

		public Animation ()
		{
			targets = new Position[5];
			for (int i = 0; i < targets.Length; i++) {
				targets [i] = new Position ();
			}
		}

		public void MoveTo (Position p)
		{
			card.transform.position = p.position;
			card.transform.rotation = p.rotation;
			Stop ();
		}

		public void SetTargetState (State state)
		{
			start = new Position (card);
			stop = targets [(int)state];

			//stop.rotation = Quaternion.Slerp (stop.rotation, Random.rotation, 0.01f);
		}

		public void Stop ()
		{
			start = new Position (card);
			stop = new Position (card);
		}

		public void Finish ()
		{
			card.transform.position = stop.position;
			card.transform.rotation = stop.rotation;
		}

		public void Update (float p, Quaternion tilt, float stabilization)
		{
			card.transform.position = Vector3.Lerp (start.position, stop.position, p);
			//card.transform.rotation = Quaternion.Slerp (start.rotation, stop.rotation, p);

			Quaternion target = Quaternion.Slerp (start.rotation * tilt, stop.rotation * tilt, p);
			card.transform.rotation = Quaternion.Slerp (card.transform.rotation, target, Mathf.Pow (stabilization, Time.deltaTime));
		}
	}

	private Animation story = new Animation ();
	private Animation yes = new Animation ();
	private Animation no = new Animation ();

	public enum State
	{
		Image,
		Description,
		No,
		Yes
	}

	public void SetTargetState (State newState)
	{
		state = newState;
		animationProgress = 0.0f;

		story.SetTargetState (newState);
		yes.SetTargetState (newState);
		no.SetTargetState (newState);
	}

	Quaternion tilt = Quaternion.Euler (0, 0, 0);

	// normalized position [-1..1], [-1..1]
	public void SetTilt (Vector3 position)
	{
		tilt = Quaternion.Euler (-position.y * 5.0f, -position.x * 5.0f, 0);
	}

	void Start ()
	{
		story.card = StoryCard;
		yes.card = YesCard;
		no.card = NoCard;

		story.targets [(int)State.Image] = new Position (StoryCard);
		story.targets [(int)State.Description] = (new Position (StoryCard)).RotateY (180);
		story.targets [(int)State.No] = new Position (EndPosition).RotateY (180);
		story.targets [(int)State.Yes] = new Position (EndPosition).RotateY (180);

		yes.targets [(int)State.Image] = new Position (EndPosition);
		yes.targets [(int)State.Description] = (new Position (YesCard));
		yes.targets [(int)State.No] = new Position (EndPosition);
		yes.targets [(int)State.Yes] = new Position (StoryCard).RotateY (180);

		no.targets [(int)State.Image] = new Position (EndPosition);
		no.targets [(int)State.Description] = (new Position (NoCard));
		no.targets [(int)State.No] = new Position (StoryCard).RotateY (180);
		no.targets [(int)State.Yes] = new Position (EndPosition);

		story.MoveTo (new Position (StartPosition));
		yes.MoveTo (new Position (EndPosition));
		no.MoveTo (new Position (EndPosition));

		SetTargetState (State.Image);
		animationProgress = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		animationProgress += Time.deltaTime / animationDuration;
		float p = animationCurve.Evaluate (animationProgress);

		story.Update (p, tilt, animationStabilization);
		yes.Update (p, tilt, animationStabilization);
		no.Update (p, tilt, animationStabilization);

		// do a jump from EndPosition to StartPosition
		if (animationProgress >= 1.0f) {
			if (state == State.No || state == State.Yes) {
				story.MoveTo (new Position (StartPosition));
			}
		}
	}
}
