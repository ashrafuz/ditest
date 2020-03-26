using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetController : MonoBehaviour {

	public TargetPoint targetPoints;
	public Transform _edgeLeftTop;
	public Transform _edgeRightTop;
	public Transform _edgeLeftBottom;
	public Transform _edgeRightBottom;
	public Text _totalPointsText;

	[Header("Online tournament")]
	public int actualPos = 1;
	string tournamentID = "1";
	public float[] posTargetX;
	public float[] posTargetY;
	public float[] posTotalTarget;

	private static TargetController _mInstance;
	private int _totalPoints = 0;

	public static TargetController SharedInstance() {
		return _mInstance;
	}

	// Use this for initialization
	void Awake () {
		_mInstance = this;
		if(targetPoints) {
			targetPoints.OnTargetMakePoints = OnBallCollideWithTarget;
		}
	}

	// Update is called once per frame
	void Start () {
		tournamentID = PlayerPrefs.GetString("tournamentID");
		#if UNITY_EDITOR
		tournamentID = "1";
		#endif
		//RespawnTarget();
	}

	void OnDestroy() {
		targetPoints.OnTargetMakePoints -= OnBallCollideWithTarget;
		_mInstance = null;
	}

	/// <summary>
	/// This method is active when the target obtain points
	/// </summary>
	/// <param name="points">Points.</param>
	public void OnBallCollideWithTarget(int points) {
		_totalPoints += points;
		_totalPointsText.text = _totalPoints.ToString();

		TimerControl.SharedInstance ().SetPoints(_totalPoints);
	}

	public void OnPointsChanged(int newPoints){
		_totalPoints = newPoints;
		_totalPointsText.text = _totalPoints.ToString();
	}

	public int GetScore() {
		return _totalPoints;
	}

	/// <summary>
	/// Respawns the target.
	/// Calculate the position of the target and make visible
	/// </summary>
	public void RespawnTarget() {
		if (targetPoints != null) {
			targetPoints._isTargetActive = true;


			/**
			 * si es mayor a 0, use leftTop position
			 * si es menor que 0, use righTop position
			 * 
			 * */


			float posX = (Random.Range(-10, 10) > 0) ? _edgeLeftTop.position.x : _edgeRightTop.position.x;
			float posY = CalculateRandomValue(_edgeLeftTop.position.y, _edgeRightBottom.position.y);


			tournamentID = PlayerPrefs.GetString("tournamentID");
			#if UNITY_EDITOR
			tournamentID = "1";
			#endif

			if (tournamentID != "1") {
				posTotalTarget = getNextTarget ();
				posX = posTotalTarget [0];
				posY = posTotalTarget [1];
				targetPoints.transform.position = new Vector3(posX, posY, _edgeLeftTop.position.z);
			} else {
				targetPoints.transform.position = new Vector3(posX, posY, _edgeLeftTop.position.z);
			}


		}
	}

	/// <summary>
	/// Calculates the random value.
	/// Define a random value among two variables
	/// </summary>
	/// <returns>The random value.</returns>
	/// <param name="posMin">Position minimum.</param>
	/// <param name="posMax">Position max.</param>
	public float CalculateRandomValue(float posMin, float posMax) {
		return Random.Range(posMin, posMax);
	}

	public bool collidesWithTarget(Vector3 ballPosition) {
		if (targetPoints) {
			return targetPoints.collidesWithTarget (ballPosition);
		}
		return false;
	}

	public float[] getNextTarget(){
		float posX = 0;
		float posY = 0;
		float[] posXY = new float[2];

		if (tournamentID != "1") {
			if(actualPos==(posTargetY.Length-1)){
				actualPos = 1;
			}
			posX = posTargetX[actualPos-1];
			posY = posTargetY[actualPos-1];
			posXY [0] = posX;
			posXY [1] = posY;
			actualPos++;
		} 

		return posXY;
	} 
}
