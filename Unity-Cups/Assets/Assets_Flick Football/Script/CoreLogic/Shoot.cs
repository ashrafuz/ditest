using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using Random = UnityEngine.Random;
using LitJson;

public class Shoot : MonoBehaviour {

	public static Shoot share;

	public static Action EventShoot = delegate {};
	public static Action<float> EventChangeSpeedZ = delegate {};
	public static Action<float> EventChangeBallZ = delegate { };
	public static Action<float> EventChangeBallX = delegate { };
	public static Action<float> EventChangeBallLimit = delegate { };
	public static Action<Collision> EventOnCollisionEnter = delegate { };
	public static Action<Collider> EventOnTriggerEnter = delegate { };
	public static int onceCrowdOh = 0;
	public static Action EventDidPrepareNewTurn = delegate { };
	public float _ballControlLimit;

	public static Action<bool> EventGoPlayAgain = delegate { };

	public Transform _goalKeeper;
	public Transform _ballTarget;
	protected Vector3 beginPos;
	protected bool _isShoot = false;

	public float minDistance = 100;		// 40f


	public Rigidbody _ball;
	public float factorUp = 0.012f;				// 10f
	public float factorDown = 0.003f;			// 1f
	public float factorLeftRight = 0.025f;		// 2f
	public float factorLeftRightMultiply = 0.8f;		// 2f
	public float _zVelocity = 24f;

	public AnimationCurve _curve;
	protected Camera _mainCam;

	protected float factorUpConstant =  0.017f * 960f; 	// 0.015f * 960f;
	protected float factorDownConstant = 0.006f * 960f; // 0.005f * 960f;
	protected float factorLeftRightConstant =  0.0235f * 640f; // 0.03f * 640f; // 0.03f * 640f;

	public Transform _ballShadow;


	public float _speedMin = 18f;	// 20f;
	public float _speedMax = 30f;	// 36f;

	public float _distanceMinZ = 20.5f;
	public float _distanceMaxZ = 40f;

	public float _distanceMinX = -30f;
	public float _distanceMaxX = 30f;

	public static bool _isShooting = false;   
	public bool _canControlBall = false;
	public static bool hitSomething = false;

	public Transform _cachedTrans;

	public bool _enableTouch = false;
	public float screenWidth;
	public float screenHeight;

	Vector3 _prePos, _curPos;
	public float angle;
	protected ScreenOrientation orientation;

	protected Transform _ballParent;

	protected RaycastHit _hit;
	public bool _isInTutorial = false;
	public Vector3 ballVelocity;

	private float _ballPostitionZ = -22f;
	private float _ballPostitionX = 0f;

	public float BallPositionZ
	{
		get { return _ballPostitionZ; }
		set { _ballPostitionZ = value; }
	}

	public float BallPositionX
	{
		get { return _ballPostitionX; }
		set { _ballPostitionX = value; }
	}
	public TrailRenderer _effect;


	[Header("Online tournament")]
	public string JsonString;
	private JsonData Data;
	public int actualPos = 1;
	public int actualTarget = 1;
	string tournamentID = "1";
	public float[] posBalonX;
	public float[] posBalonZ;
	public float[] posTotalBalon;

	protected virtual void Awake() {
		share = this;
		_cachedTrans = transform;
		_isShooting = true;
		_ballParent = _ball.transform.parent;

		//PENALTIESDUELIT
		_distanceMinX = -15f;
		_distanceMaxX = 15f;
		_distanceMaxZ = 30f;
		_distanceMinZ = 20f;

	}

	// Use this for initialization
	protected virtual void Start () {

		tournamentID = PlayerPrefs.GetString("tournamentID");
		#if UNITY_EDITOR
		tournamentID = "1";
		#endif
		if (tournamentID != "1") {
			StartCoroutine (DownloadInfo ());
		} else {
			StartGame ();
		}
	}



	void StartGame(){
		//        Application.targetFrameRate = 30;
		TargetController.SharedInstance().RespawnTarget();
		TimerControl.SharedInstance ().StartGame();
		_mainCam = CameraManager.share._cameraMainComponent;

		#if UNITY_WP8 || UNITY_ANDROID || UNITY_IOS
		Time.maximumDeltaTime = 0.2f;
		Time.fixedDeltaTime = 0.008f;
		#else
		Time.maximumDeltaTime = 0.1f;
		Time.fixedDeltaTime = 0.005f;
		#endif

		orientation = Screen.orientation;
		calculateFactors();

		//_ballControlLimit = 6f;
		EventChangeBallLimit(_ballControlLimit);

		reset();
		CameraManager.share.reset();
		GoalKeeper.share.reset();

		GoalDetermine.EventFinishShoot += goalEvent;

		if (TimerControl.SharedInstance ().LoadingScreen.activeSelf) {
			TimerControl.SharedInstance ().LoadingScreen.SetActive (false);
		}
	} 

	void OnDestroy() {
		GoalDetermine.EventFinishShoot -= goalEvent;
	}

	public virtual void goalEvent(bool isGoal, Area area) {
		_canControlBall = false;
		_isShooting = false;
	}

	public void calculateFactors() {
		screenHeight = Screen.height;
		screenWidth = Screen.width;

		minDistance = (100 * screenHeight) / 960f;
		factorUp = factorUpConstant / screenHeight;
		factorDown = factorDownConstant / screenHeight;
		factorLeftRight = factorLeftRightConstant / screenWidth;

		/*
		Debug.Log("Orientation : " + orientation + "\t Screen height = " + screenHeight 
            + "\t Screen width = " + screenWidth + "\t factorUp = " + factorUp + "\t factorDown = " + factorDown 
            + "\t factorLeftRight = " + factorLeftRight + "\t minDistance = " + minDistance);
*/
	}

	protected void LateUpdate()
	{
		if(screenHeight != Screen.height) {
			orientation = Screen.orientation;
			calculateFactors();
			CameraManager.share.reset();
		}
	}
	void FixedUpdate() {
		ballVelocity = _ball.velocity;

		Vector3 pos = _ball.transform.position;
		pos.y = 0.015f;
		_ballShadow.position = pos;
	}

	protected virtual void Update() {

		if(_isShooting) {		// neu banh chua vao luoi hoac trung thu mon, khung thanh thi banh duoc phep bay voi van toc dang co
			if( _enableTouch && !_isInTutorial ) {
				#if UNITY_STANDALONE || UNITY_EDITOR
				if(Input.GetMouseButtonDown(0)) {// touch phase began
					mouseBegin(Input.mousePosition);
				}
				else if( Input.GetMouseButton(0) ) {			
					mouseMove(Input.mousePosition);
				}
				else if(Input.GetMouseButtonUp(0)) {// touch ended
					mouseEnd();
				}
				#endif

				#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
				if(Input.touchCount == 1) {
					Touch touchEvent = Input.touches[0];
					if(touchEvent.phase == TouchPhase.Began) {
						mouseBegin(touchEvent.position);
					}
					if(touchEvent.phase == TouchPhase.Moved) {
						mouseMove(touchEvent.position);
					}
					if(touchEvent.phase == TouchPhase.Ended || touchEvent.phase == TouchPhase.Canceled) {
						mouseEnd();
					}
				}
				#endif
			}
			if(_isShoot) {
				Vector3 speed = _ballParent.InverseTransformDirection(_ball.velocity);
				speed.z = _zVelocity;
				_ball.velocity = _ballParent.TransformDirection(speed);
			}
		}
	}

	public void mouseBegin(Vector3 pos) {
		_prePos = _curPos = pos;
		beginPos = _curPos;
	}

	public void mouseEnd() {
		if(_isShoot == true) {		// neu da sut' roi` thi ko cho dieu khien banh nua, tranh' truong` hop nguoi choi tao ra nhung cu sut ko the~ do~ noi~
			//_canControlBall = false;
		}
	}

	public void mouseMove(Vector3 pos) {
		if(_curPos != pos) {		// touch phase moved
			_prePos = _curPos;
			_curPos = pos;


			Vector3 distance = _curPos - beginPos;

			if(_isShoot == false) {				// CHUA SUT
				if(distance.y > 0 && distance.magnitude >= minDistance) {		
					if(Physics.Raycast( _mainCam.ScreenPointToRay(_curPos), out _hit, 100f) && _hit.transform != _cachedTrans){
						_isShoot = true;

						Vector3 point1 = _hit.point;		// contact point
						point1.y = 0;
						point1 = _ball.transform.InverseTransformPoint(point1);		// dua point1 ve he truc toa do cua ball, coi ball la goc toa do cho de~
						point1 -= Vector3.zero;			// vector tao boi point va goc' toa do

						Vector3 diff = point1;
						diff.Normalize();				// normalized rat' quan trong khi tinh' goc

						float angle = 90 - Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg;		// doi ra degree va lay 90 tru` vi nguoc
						//								Debug.Log("angle = " + angle);

						float x = _zVelocity * Mathf.Tan(angle * Mathf.Deg2Rad);				

						//							float x = distance.x * factorLeftRight;
						_ball.velocity = _ballParent.TransformDirection(new Vector3(x, distance.y * factorUp, _zVelocity));
						_ball.angularVelocity = new Vector3(0, x, 0f);

						if(EventShoot != null) {
							EventShoot();
						}
					}
				}
			}
			else {				// da~ sut' roi`, tuy theo do lech cua touch frame hien tai va truoc do' ma se lam cho banh xoay' trai', phai~, len va xuong' tuong ung'
				if(_canControlBall == true) {	// neu nhac ngon tay len khoi man hinh roi thi ko cho dieu khien banh nua

					if (_cachedTrans.position.z < -_ballControlLimit)
					{
						// neu banh xa hon khung thanh 6m thi moi' cho dieu khien banh xoay, di vo trong khoang cach 6m thi ko cho nua~ de~ lam cho game can bang`

						distance = _curPos - _prePos;

						Vector3 speed = _ballParent.InverseTransformDirection(_ball.velocity);
						speed.y += distance.y*((distance.y > 0) ? factorUp : factorDown);
						speed.x += distance.x * factorLeftRight * factorLeftRightMultiply;
						_ball.velocity = _ballParent.TransformDirection(speed);

						speed = _ball.angularVelocity;
						speed.y += distance.x*factorLeftRight;
						_ball.angularVelocity = speed;
					}
					else
					{
						//						Debug.Log ("aca");
						_canControlBall = false;
					}
				}
			}
		}
	}

	protected void OnCollisionEnter(Collision other)
	{
		string tag = other.gameObject.tag;
		if(tag.Equals("Player") || tag.Equals("Obstacle") || tag.Equals("Net") || tag.Equals("Wall")) {	// banh trung thu mon hoac khung thanh hoac da vao luoi roi thi ko cho banh bay voi van toc nua, luc nay de~ cho physics engine tinh' toan' quy~ dao bay
			_isShooting = false;

			if (tag.Equals ("Net")) {
				_ball.velocity /= 3f;
			} else {
//				Debug.Log(tag);
				if (!hitSomething) {
					hitSomething = true;
					EventGoPlayAgain (true);
				}
			}
		}

		EventOnCollisionEnter(other);
	}

	protected void OnTriggerEnter(Collider other){

		if(other.gameObject.name=="Wall1" && onceCrowdOh == 0){
			//Debug.Log ("Triggerred: "+other.gameObject.name);

			EventOnTriggerEnter(other);
		}
	}

	private void enableEffect() {
		//		_effect.enabled = true;
		_effect.time = 1;
	}

	public virtual void reset() {
		reset (- Random.Range(_distanceMinX, _distanceMaxX), - Random.Range(_distanceMinZ, _distanceMaxZ));

	}

	public virtual void reset(float x, float z)
	{
		//Debug.Log(string.Format("<color=#c3ff55>Reset Ball Pos, x = {0}, z = {1}</color>", x, z));


		if(tournamentID != "1"){
			posTotalBalon = getNextBall ();
			x = posTotalBalon [0];
			z = -(posTotalBalon [1]);
		}

		float oldx = x;
		float oldz = z;

		if (x >= 1000f || x <= -1000f){
			x = x/10000f;
		}

		if (z >= 1000f || z <= -1000f){
			z = z/10000f;
		}


		/*
		 * 
		 * 
		_distanceMinX = -15f;
		_distanceMaxX = 15f;
		_distanceMaxZ = 30f;
		_distanceMinZ = 20f;
		 * 
		 * 
		 **
		 */

		//x = -20f;
		//z = -40f hasta -20;

		MainMenu.NativeLog("Ball reset: x: " + x.ToString() + ", z: " + z.ToString() + " oldx: " + oldx.ToString() + ", oldz: " + oldz.ToString());

		_effect.time = 0;
		//		_effect.enabled = false;
		Invoke("enableEffect", 0.1f);

		BallPositionX = x;
		EventChangeBallX(x);
		BallPositionZ = z;
		EventChangeBallZ(z);


		_canControlBall = true;
		_isShoot = false;
		_isShooting = true;

		// reset ball
		_ball.velocity = Vector3.zero;
		_ball.angularVelocity = Vector3.zero;
		_ball.transform.localEulerAngles = Vector3.zero;


		Vector3 pos = new Vector3(BallPositionX, 0f, BallPositionZ);
		Vector3 diff = -pos;
		diff.Normalize();
		float angleRadian = Mathf.Atan2(diff.z, diff.x);		// tinh' goc' lech
		float angle = 90 - angleRadian * Mathf.Rad2Deg;

		_ball.transform.parent.localEulerAngles = new Vector3(0, angle, 0);		// set parent cua ball xoay 1 do theo truc y = goc lech

		_ball.transform.position = new Vector3(BallPositionX, 0.16f, BallPositionZ);

		pos = _ballTarget.position;
		pos.x = 0;
		_ballTarget.position = pos;

		float val = (Mathf.Abs(_ball.transform.localPosition.z) - _distanceMinZ) / (_distanceMaxZ - _distanceMinZ);
		_zVelocity =  Mathf.Lerp(_speedMin, _speedMax, val);

		EventChangeSpeedZ(_zVelocity);

		WallController.share.SetWallState(true);

		EventDidPrepareNewTurn();
	}

	public void enableTouch()
	{
		_enableTouch = true;
	}

	public void disableTouch()
	{
		StartCoroutine(_disableTouch());
	}

	private IEnumerator _disableTouch()
	{
		yield return new WaitForEndOfFrame();
		_enableTouch = false;
	}

	public IEnumerator DownloadInfo()
	{
		// Pull down the JSON from our web-service
		string gameID = PlayerPrefs.GetString("gameID");
		string tournamentID = PlayerPrefs.GetString("tournamentID");
		string updateTime = PlayerPrefs.GetString("updateTime");
		string isDebug = PlayerPrefs.GetString("isDebug");
		string token = PlayerPrefs.GetString("token");

		int seed = 1;
		int.TryParse(tournamentID,out seed);
		Random.InitState(seed);

		// Build connection string
		string connectionString = "";

		if (isDebug == "Debug")
		{
			// Debug environment
			//connectionString = "http://dev.api.duelit.com/game/";
			// Debug.Log("Used debug URL");
			connectionString = PlayerPrefs.GetString("url");
		}
		else {
			// Release environment
			//connectionString = "http://api.duelit.com/game/";
			connectionString = PlayerPrefs.GetString("url");
		}




		//connectionString = "https://api-dev-lite-dot-duelit-1288.appspot.com/api/v1/games/randomness";

		//onnectionString = "http://engines.pressstart.co/duelit/kick.php";

		WWWForm form = new WWWForm();

		form.AddField("tournament_id",tournamentID);
		form.AddField("token",token);

		WWW w = new WWW(connectionString,form);
		//Debug.Log(connectionString);


		while (!w.isDone)
		{
			//debugTetris.text = "WAITING.";
			yield return new WaitForSeconds(0.3f);
		}

		yield return w;

		if (w.error != null)
		{
			// Show error or reload.
			//StartGame ();
			//Try again or send something to the upper layer
			Debug.Log("error");
			MainMenu.Exit();
			MainMenu.LoadingError (w.error);
		}
		else
		{
			JsonString = w.text;
			MainMenu.NativeLog("Football downloaded data: " + JsonString.ToString());
			//Debug.Log (JsonString);

			Data = JsonMapper.ToObject(JsonString.ToString());

			posBalonX = new float[Data["level"]["posX"].Count];
			posBalonZ = new float[Data["level"]["posZ"].Count];
			TargetController.SharedInstance().posTargetX = new float[Data["level"]["targetX"].Count];
			TargetController.SharedInstance().posTargetY = new float[Data["level"]["targetY"].Count];

			float x;
			float z;

			for (int i = 0; i < Data["level"]["posX"].Count; i++)
			{
				if(Single.TryParse (Data ["level"] ["posX"][i].ToString (), NumberStyles.Float, new CultureInfo("en-US"), out x))
					posBalonX[i] = x;


				if(Single.TryParse (Data ["level"] ["posZ"][i].ToString (), NumberStyles.Float, new CultureInfo("en-US"), out z))
					posBalonZ[i] = z;


				if(Single.TryParse (Data ["level"] ["targetX"][i].ToString (), NumberStyles.Float, new CultureInfo("en-US"), out z))
					TargetController.SharedInstance().posTargetX[i] = z;


				if(Single.TryParse (Data ["level"] ["targetY"][i].ToString (), NumberStyles.Float, new CultureInfo("en-US"), out z))
					TargetController.SharedInstance().posTargetY[i] = z;


			}

			w.Dispose ();

			StartGame ();
		}
	}

	public float[] getNextBall(){
		float posX = 0;
		float posZ = 0;
		float[] posXZ = new float[2];

		if (tournamentID != "1") {
			if(actualPos==(posBalonX.Length-1)){
				actualPos = 1;
			}
			posX = posBalonX[actualPos-1];
			posZ = posBalonZ[actualPos-1];
			posXZ [0] = posX;
			posXZ [1] = posZ;
			actualPos++;
		} 

		return posXZ;
	}  

}
