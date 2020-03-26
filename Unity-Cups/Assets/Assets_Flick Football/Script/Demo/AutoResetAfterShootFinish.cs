using UnityEngine;
using System.Collections;
using System;

public class AutoResetAfterShootFinish : MonoBehaviour
{

    public float resetAfter = 1.5f;
    public bool RandomNewPos { get; set; }
	public bool goPlay = true;

	public static Action<bool> EventGoPlayAgain = delegate { };
	// Use this for initialization
	void Start ()
	{
	    RandomNewPos = true;
	    GoalDetermine.EventFinishShoot += OnShootFinished;
	    Shoot.EventDidPrepareNewTurn += OnNewTurn;
		Shoot.EventOnTriggerEnter += OnTriggernEnter;
		Shoot.EventGoPlayAgain += OnEventRepetPlay;
	}

    void OnDestroy()
    {
        GoalDetermine.EventFinishShoot -= OnShootFinished;
        Shoot.EventDidPrepareNewTurn -= OnNewTurn;
		Shoot.EventOnTriggerEnter -= OnTriggernEnter;
		Shoot.EventGoPlayAgain -= OnEventRepetPlay;
    }

    void OnNewTurn()
    {
        RunAfter.removeTasks(gameObject);
    }

    void OnShootFinished(bool isGoal, Area area)
    {
		goPlay = false;
		StopAllCoroutines ();
		Shoot.hitSomething = false;
        RunAfter.runAfter(gameObject, () =>
        {
            DemoShoot.share.Reset(RandomNewPos);
        }, resetAfter);
    }

	void OnTriggernEnter(Collider other)
	{
		goPlay = false;
		StopAllCoroutines ();
		Shoot.hitSomething = false;
		RunAfter.runAfter(gameObject, () =>
			{
				DemoShoot.share.Reset(RandomNewPos);
			}, resetAfter);
		
	}

	void OnEventRepetPlay(bool other)
	{
		goPlay = true;
		StartCoroutine (RepetPlay ());
	}

	IEnumerator RepetPlay(){
		yield return new WaitForSeconds (2f);

		if(goPlay && !Shoot._isShooting){
			//Debug.Log ("Go PLay");
			EventGoPlayAgain (true);
			RunAfter.runAfter(gameObject, () =>
				{
					DemoShoot.share.Reset(RandomNewPos);
				}, resetAfter);
		}
	
	}
}
