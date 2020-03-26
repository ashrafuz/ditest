using UnityEngine;
using System.Collections;

public class Pong_Rebound : MonoBehaviour {

    private GameObject Ball;
    // Use this for initialization
    void Start () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Beer_pong")
        {
            Ball = other.gameObject;
            Ball.tag = "Doble_Pong";
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
