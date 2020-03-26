using UnityEngine;
using System.Collections;

public class Delete_Mising_ball : MonoBehaviour {
    public int balls;
	// Use this for initialization
	void Start () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Beer_pong"|| other.tag == "Doble_Pong")
        {
            Destroy(other.gameObject);
        }

    }
    // Update is called once per frame
    void Update () {
        balls = GameObject.FindGameObjectsWithTag("Doble_Pong").Length;
       if(balls==3)
        {
            Destroy(GameObject.FindGameObjectWithTag("Doble_Pong"));
        }
    }
}
