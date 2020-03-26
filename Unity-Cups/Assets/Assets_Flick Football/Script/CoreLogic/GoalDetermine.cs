using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Path;
using Holoville.HOTween.Plugins;
using Holoville.HOTween.Plugins.Core;

public enum Area
{
	Top,
	Left,
	Right,
	Normal,
	CornerLeft,
	CornerRight,
	None
}
public delegate void GoalEvent (bool isGoal, Area area);

public class GoalDetermine : MonoBehaviour
{

	public static GoalDetermine share;

	public static GoalEvent EventFinishShoot;
	//public GameObject _prefabGoalSuccess;

	private bool _isGoal;
	public bool _goalCheck;

	private Transform _ball;

	[SerializeField] 
	private Transform _pointLeft;
	[SerializeField]
	private Transform _pointRight;
	[SerializeField]
	private Transform _pointUp;
	[SerializeField]
	private Transform _pointDown;
	[SerializeField]
	private Transform _pointBack;

	public Renderer _areaTop;
	public Renderer _areaCornerLeft;
	public Renderer _areaCornerRight;
	public Renderer _areaLeft;
	public Renderer _areaRight;

	public Renderer _effectTop;
	public Renderer _effectLeft;
	public Renderer _effectCornerLeft;
	public Renderer _effectCornerRight;
	public Renderer _effectRight;

	public Transform _poleLeft;
	public Transform _poleRight;

	public Material _matCenter;
	public Texture2D _centerNormal;
	public Texture2D _centerCritical;

	private Area _poleArea;
	private bool _touchEventActive = false;

	void Awake ()
	{
		share = this;
		//_prefabGoalSuccess.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
		_ball = Shoot.share._ball.transform;
		Shoot.EventShoot += eventShoot;
	}


	void OnDestroy ()
	{
		Shoot.EventShoot -= eventShoot;
		share = null;
	}

	private Vector3 _contactPointWithPole;

	public void hitPole (Area poleArea, Vector3 contactPoint)
	{
		if (_goalCheck) {
			_contactPointWithPole = contactPoint;
			_poleArea = poleArea;
		}
	}

	private void eventShoot ()
	{
		

		_count1 = _count2 = _count3 = 0;
		_goalCheck = true;
		_prevBallPos = _currentBallPos = _ball.position;
		_touchEventActive = false;
		_poleArea = Area.None;
	}

	public void reset ()
	{
		_goalCheck = false;
	}

	public void flashingHighScoreAreas ()
	{
//		_effectTop.enabled = true;
//		_effectLeft.enabled = true;
//		_effectRight.enabled = true;
//		_effectCornerLeft.enabled = true;
//		_effectCornerRight.enabled = true;

		Material mat = _effectTop.sharedMaterial;
		Color col = mat.GetColor ("_TintColor");
		col.a = 50f / 255f;
		mat.SetColor ("_TintColor", col);

		mat = _effectCornerLeft.sharedMaterial;
		col = mat.GetColor ("_TintColor");
		col.a = 50f / 255f;
		mat.SetColor ("_TintColor", col);

		iTween.ValueTo (gameObject, iTween.Hash ("time", 1.5f
		                                       , "delay", 0.5f
		                                       , "from", 50f / 255f
		                                       , "to", 0f
		                                       , "easetype", iTween.EaseType.linear
		                                       , "onupdate", "onUpdateFlashing"
		                                       , "oncomplete", "completeFlashing"
		));
	}

	private void onUpdateFlashing (float val)
	{
		Material mat = _effectTop.sharedMaterial;
		Color col = mat.GetColor ("_TintColor");
		col.a = val;
		mat.SetColor ("_TintColor", col);

		mat = _effectCornerLeft.sharedMaterial;
		col = mat.GetColor ("_TintColor");
		col.a = 50f / 255f;
		mat.SetColor ("_TintColor", col);
	}

	private void completeFlashing ()
	{
		_effectTop.enabled = false;
		_effectLeft.enabled = false;
		_effectRight.enabled = false;
		_effectCornerLeft.enabled = false;
		_effectCornerRight.enabled = false;
	}

	private Vector3 _prevBallPos;
	private Vector3 _currentBallPos;

	private float _count1;
	private float _count2;
	private float _count3;

	float _distance;

	// Update is called once per frame
	void Update ()
	{

		if (_goalCheck) {
			CallTouchEvent ();

			_prevBallPos = _currentBallPos;
			_currentBallPos = _ball.position;

			if (_currentBallPos.z >= _pointDown.position.z) {       // ko check goal nua
				if (_currentBallPos.z <= _pointBack.position.z && _currentBallPos.x < _pointRight.position.x && _currentBallPos.x > _pointLeft.position.x && _currentBallPos.y < _pointUp.position.y && _currentBallPos.y > 0) {
					_count1 = 0;

					_goalCheck = false;

					Area area = Area.Normal;

					if (_poleArea != Area.None) {		// neu truoc do trung xa ngang hay cot doc xong roi banh vo luoi 

						area = Area.CornerLeft;
						setGoalSuccesIcon (area, _contactPointWithPole);

					} else {		// khong trung xa ngang cot doc gi het
						if (_areaTop.bounds.Contains (_currentBallPos))
							area = Area.Top;
						else if (_areaLeft.bounds.Contains (_currentBallPos)) {
							area = Area.Left;
						} else if (_areaRight.bounds.Contains (_currentBallPos)) {
							area = Area.Right;
						} else if (_areaCornerLeft.bounds.Contains (_currentBallPos)) {
							area = Area.CornerLeft;
						} else if (_areaCornerRight.bounds.Contains (_currentBallPos)) {
							area = Area.CornerRight;
						}

						setGoalSuccesIcon (area, _ball.transform.position);
					}

					//Debug.Log (" >>>>>  CHECKING TARGET");
					//if it does collide with target, TargetPoint already notifies TimerControl
					if (!TargetController.SharedInstance ().collidesWithTarget (_currentBallPos)) {
//						Debug.Log (" >>>>>  NO TARGET: " + " ballPos=" + _currentBallPos);
						TimerControl.SharedInstance ().AddPoints (1);
					}
					if (EventFinishShoot != null)
						EventFinishShoot (true, area);
				} else {
					_touchEventActive = true;
					_count1 += Time.deltaTime;
					if (_count1 > 0.8f) {
						_goalCheck = false;
						if (EventFinishShoot != null) {
							_touchEventActive = false;
							EventFinishShoot (false, Area.None);  
						}
					}
				}
			} else {      // keep checking this goal
				if (_currentBallPos.z > _prevBallPos.z) {
					_count2 = 0;

					_distance = 10f;
					if (Shoot.share._ball.velocity.sqrMagnitude < 0.3f)
						_distance = 0f;
					else if (Shoot.share._ball.velocity.sqrMagnitude < 2f)
						_distance = 4f;

					if (Mathf.Abs (_currentBallPos.x) > _distance && (Mathf.Abs (_currentBallPos.x) > Mathf.Abs (_prevBallPos.x) || Shoot.share._ball.velocity.sqrMagnitude < 0.3f)) {
						_count3 += Time.deltaTime;
						if (_count3 > 0.8f) {
							_goalCheck = false;
							if (EventFinishShoot != null)
								EventFinishShoot (false, Area.None);
						}
					} else {
						_count3 = 0;
					}
				} else {
					_count2 += Time.deltaTime;
					if (_count2 > 0.8f) {
						_goalCheck = false;
						if (EventFinishShoot != null)
							EventFinishShoot (false, Area.None);
					}
				}
			}
		}
	}

	private void CallTouchEvent ()
	{
		if (_touchEventActive) {
			if (Input.touchCount >= 1 || Input.GetMouseButtonDown (0)) {
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					_touchEventActive = false;
					if (EventFinishShoot != null)
						EventFinishShoot (false, Area.None);
				}
			}
		}
	}

	private void setGoalSuccesIcon (Area area, Vector3 position)
	{

		if (area == Area.None || area == Area.Normal) {
			//_matCenter.mainTexture = _centerNormal;

		} else { 
			//_matCenter.mainTexture = _centerCritical;
		}
		GoalsCounter.SharedInstance ().IncreaseGoalValue ();
		flashingHighScoreAreas ();
	}


}
