using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Path;
using Holoville.HOTween.Plugins;
using Holoville.HOTween.Plugins.Core;

/// <summary>
/// represents the circle with 3 colored inner circles;
/// the target where the goal is most valued
/// </summary>
public class TargetPoint : MonoBehaviour
{

	public delegate void OnTargetPointsCollider (int points);

	public GameObject _prefabGoalSuccess;
	public OnTargetPointsCollider OnTargetMakePoints;
	public bool _isTargetActive = true;

	private Transform _ball;
	// ball size 0.28
	private Vector3 _ballPos;
	private Vector3 _myPos;

	private const int GOAL_POINTS_CIRCLE_1 = 4;
	private const int GOAL_POINTS_CIRCLE_2 = 3;
	private const int GOAL_POINTS_CIRCLE_3 = 2;

	// Use this for initialization
	void Start ()
	{
		_ball = Shoot.share._ball.transform;
		_prefabGoalSuccess.SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
		_ballPos = _ball.position;
		_myPos = transform.position;
		float distance = Vector2.Distance (_ballPos, _myPos);
		//evaluate postion of the ball and the target
		if (collidesWithTarget (distance, _ballPos, _myPos, false) && _isTargetActive && _ballPos.z > _myPos.z && (Mathf.Abs (_myPos.z - _ballPos.z) < 1)) {
			if (distance < 0.28f) {
				//Debug.Log (" >>>>>  +4 ");
				OnTargetMakePoints (GOAL_POINTS_CIRCLE_1);
			} else if (distance < 0.56f) {
				//Debug.Log (" >>>>>  +3 ");
				OnTargetMakePoints (GOAL_POINTS_CIRCLE_2);
			} else {
				//Debug.Log (" >>>>>  +2 ");
				OnTargetMakePoints (GOAL_POINTS_CIRCLE_3);
			}
			ActivateCollisionPoint ();
			_isTargetActive = false;
		}
	}

	public bool collidesWithTarget (Vector3 ballPosition)
	{
		return collidesWithTarget (ballPosition, _myPos, true);
	}

	public bool collidesWithTarget (Vector3 ballPosition, Vector3 myPos, bool debug)
	{
		float distance = Vector2.Distance (ballPosition, myPos);
		return collidesWithTarget (distance, ballPosition, myPos, debug);
	}

	private bool collidesWithTarget (float distance, Vector3 ballPosition, Vector3 myPos, bool debug)
	{
		if (debug) {
			/*
			Debug.Log (" >>  collidesWithTarget1: " + " distance=" + distance + " ballPosition=" + ballPosition + " myPos=" + myPos
			+ " >>>>>  collidesWithTarget2: " + " distance < 0.84f=" + (distance < 0.84f)
			+ " ballPosition.z(" + ballPosition.z + ") > myPos.z(" + myPos.z + ")=" + (ballPosition.z > myPos.z) + " Mathf.Abs=" + (Mathf.Abs (myPos.z - ballPosition.z) < 1));
			*/
		}

		if (distance < 0.75f) {
			return true;
		} else if (distance < 0.84f) {
			return (_ballPos.z > _myPos.z && (Mathf.Abs (_myPos.z - _ballPos.z) < 1));
		}


		return false; 
	}

	/// <summary>
	/// Activates the collision point.
	/// show goal success
	/// </summary>
	private void ActivateCollisionPoint ()
	{
		_prefabGoalSuccess.SetActive (true);
		_prefabGoalSuccess.transform.position = _ballPos;
		_prefabGoalSuccess.transform.localScale = new Vector3 (0.6f, 0.6f, 1);
		HOTween.To (_prefabGoalSuccess.transform, 1f, new TweenParms ()
		           .Prop ("localScale", new Vector3 (0.9f, 0.9f, 1), false)
		           .Loops (1, LoopType.Restart)
		           .Ease (EaseType.Linear)
		           .OnComplete (animationFinished)
		);

		/*
		Material mat = _prefabGoalSuccess.GetComponent<Renderer> ().sharedMaterial;
		Color col = mat.GetColor ("_TintColor");
		col.a = 0.5f;
		mat.SetColor ("_TintColor", col);
		*/

		iTween.ValueTo (gameObject, iTween.Hash ("time", 0.5f
		                                       , "from", 0.5f
		                                       , "to", 1f
		                                       , "easetype", iTween.EaseType.easeInCubic
		                                       , "onupdate", "onUpdateColor"
		                                       , "oncomplete", "complete1"
		));
	}

	/// <summary>
	/// Ons the color of the update.
	/// change color of success goal
	/// </summary>
	/// <param name="val">Value.</param>
	private void onUpdateColor (float val)
	{
		/*
		Material mat = _prefabGoalSuccess.GetComponent<Renderer> ().sharedMaterial;
		Color col = mat.GetColor ("_TintColor");
		col.a = val;
		mat.SetColor ("_TintColor", col);
		*/
	}

	private void complete1 ()
	{
		/*
		iTween.ValueTo (gameObject, iTween.Hash ("time", 0.5f
		                                       , "from", 1f
		                                       , "to", 0f
		                                       , "easetype", iTween.EaseType.easeOutCubic
		                                       , "onupdate", "onUpdateColor"
		));
		*/
	}

	private void animationFinished ()
	{
		_prefabGoalSuccess.SetActive (false);
	}
}
