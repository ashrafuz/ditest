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
    private List<GameObject> _shadowPool;
    
    void Start()
    {
        _ballPool = new List<GameObject>();
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
        
        _shadowPool[newBallIndex].transform.position = ShadowInitiate.position;
        _shadowPool[newBallIndex].transform.rotation = ShadowInitiate.rotation;

        return  _ballPool[newBallIndex];
    }

    public void HideBall(GameObject idleBall)
    {
        for (int i = 0; i < _ballPool.Count; i++) {
            if (idleBall.Equals(_ballPool[i])) {
                _ballPool[i].SetActive(false);
                _shadowPool[i].SetActive(false);
            }
        }
    }

    private int CreateNewBall()
    {
        GameObject nb = Instantiate(BallPrefab, PointInitiate.position, PointInitiate.rotation); 
        nb.transform.SetParent(this.transform);
        
        GameObject ns = Instantiate(ShadowPrefab, ShadowInitiate.position, ShadowInitiate.rotation);
        ns.transform.SetParent(this.transform);

        _ballPool.Add(nb);
        _shadowPool.Add(ns);
        return _ballPool.Count - 1 ; //returning last index
    }


}
