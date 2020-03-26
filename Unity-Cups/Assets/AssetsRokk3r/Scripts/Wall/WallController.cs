using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

  public bool _isWallKick = true;

  public static WallController share;

  public bool IsWallKick {
    get { return _isWallKick; }
    set {
      _isWallKick = value;
    }
  }

  // Use this for initialization

  void Awake(){
    share = this;
  }
  void Start () {
    //StartCoroutine(StartBefore());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  IEnumerator StartBefore() {
    yield return new WaitForEndOfFrame();
    //SetWallState(true);
  }

  public void SetWallState(bool state) {
      _isWallKick = state;
      Wall.share.IsWall = _isWallKick;
      if(_isWallKick) {
        Wall.share.setWall(Shoot.share._ball.transform.position);
      }
  }
}
