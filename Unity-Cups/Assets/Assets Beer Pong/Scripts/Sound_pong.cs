using UnityEngine;
using System.Collections;

public class Sound_pong : MonoBehaviour
{
    private GameObject Ball;
    public float VOL_Pong;
    public float Vel;
    public GameObject Sound;
    // Use this for initialization
    void OnCollisionEnter(Collision collision)
    {
      

        if (collision.relativeVelocity.magnitude > 2)
        {
            Sound.GetComponent<AudioSource>().Play();
            VOL_Pong -= 1;
            Vel += 0.1f;
            Sound.GetComponent<AudioSource>().volume -= VOL_Pong;
            if (Sound.GetComponent<AudioSource>().pitch < 2.8f)
                Sound.GetComponent<AudioSource>().pitch += Vel;

        }
    }

  
void Start ()
    {
       // Sound=GameObject.Find("Audio_pong");

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
