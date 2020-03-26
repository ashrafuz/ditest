using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// GoalsCounter
public class GoalsCounter : MonoBehaviour
{

	public Text textOutput;

	private int goalsCounter = 0;
	private static GoalsCounter sharedInstance;

	public static GoalsCounter SharedInstance ()
	{
		return sharedInstance;
	}

	// Use this for initialization
	void Awake ()
	{
		sharedInstance = this;
	}

	void OnDestroy(){
		sharedInstance = null;
	}

	/// <summary>
	/// Increment the value of goalcounter in one unit and show in the UItext
	/// And update the level of difficulty of the goalkeeper
	/// </summary>
	public void IncreaseGoalValue ()
	{
		goalsCounter++;
		if (textOutput != null) {
			textOutput.text = "Goals: " + goalsCounter;
		}
		GoalkeeperController.SharedInstance ().OnChangeDifficulty (goalsCounter);
	}

}
