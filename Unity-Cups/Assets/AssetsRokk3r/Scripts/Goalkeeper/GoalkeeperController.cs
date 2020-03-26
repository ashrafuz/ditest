using UnityEngine;
using System.Collections;

public class GoalkeeperController : MonoBehaviour {

  	private static GoalkeeperController shared;
  	
  	float difficultVal = 0f;
  	float predictVal = 0f;
  	float deltaDistance = 0f;
  	
  	public static GoalkeeperController SharedInstance() {
  	  	return shared;
  	}


	// Use this for initialization
	void Awake () {
    	shared = this;
	}

  	/// <summary>
	/// change the skills of the goalkeeper and increase the difficulty level
  	/// </summary>
  	/// <param name="value"></param>
  	public void OnChangeDifficulty(int value) {
    	difficultVal = (difficultVal < GoalKeeperLevel.share.getMaxLevel()) ? difficultVal + 0.5f: difficultVal;
    	predictVal = (predictVal < 20f)? predictVal + 0.5f:predictVal;
    	deltaDistance = (deltaDistance < 4f)? deltaDistance + 0.1f: deltaDistance;
    	GoalKeeper.share._predictFactor = (int)predictVal;
    	GoalKeeperHorizontalFly.share._deltaDistance = deltaDistance;
    	if(GoalKeeperLevel.share != null) {
      		GoalKeeperLevel.share.setLevel((int)difficultVal);
    	}
  	}

}
