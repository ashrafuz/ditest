using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject BallPrefab;
    [SerializeField] private GameObject ShadowPrefab;
    
    [SerializeField] private Transform PointInitiate;
    [SerializeField] private Transform ShadowInitiate;

    private List<GameObject> _ballPool;
    private GameObject _savedShadow; //assuming we need only one shadow per shoot
    private Shadow_Pong _shadowPong;

    void Start()
    {
        _ballPool = new List<GameObject>();

        _savedShadow = Instantiate(ShadowPrefab, ShadowInitiate.position, ShadowInitiate.rotation);
        _savedShadow.transform.SetParent(this.transform);
        _shadowPong = _savedShadow.GetComponent<Shadow_Pong>();
    }

    public GameObject GetABall()
    {
        int newBallIndex = -1;
        for (int i = 0; i < _ballPool.Count; i++) {
            if (!_ballPool[i].activeInHierarchy) {
                newBallIndex = i;
                break;
            }
        }
        
        newBallIndex = newBallIndex != -1 ? newBallIndex : CreateNewBall();
        
        //reset poistion
        _ballPool[newBallIndex].transform.position = PointInitiate.position;
        _ballPool[newBallIndex].transform.rotation = PointInitiate.rotation;
        
        _savedShadow.transform.position = ShadowInitiate.position;
        _savedShadow.transform.rotation = ShadowInitiate.rotation;
        _shadowPong.SetTarget(_ballPool[newBallIndex].transform);
        
        _ballPool[newBallIndex].SetActive(true);

        return  _ballPool[newBallIndex];
    }

    public void HideBall(GameObject idleBall)
    {
        for (int i = 0; i < _ballPool.Count; i++) {
            if (idleBall.Equals(_ballPool[i])) {
                _ballPool[i].SetActive(false);
            }
        }
    }

    private int CreateNewBall()
    {
        GameObject nb = Instantiate(BallPrefab, PointInitiate.position, PointInitiate.rotation); 
        nb.transform.SetParent(this.transform);
        _ballPool.Add(nb);
        return _ballPool.Count - 1 ; //returning last index
    }


}
