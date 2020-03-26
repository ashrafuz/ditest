using UnityEngine;
using System.Collections;

public class Validate_Score : MonoBehaviour {

    private GameObject Ball;
    public float Time_max_efect;
    public int Total_Score;
    public Transform Point_Iniciate;
    public GameObject Particles;
    public GameObject Points;
    public GameObject Bonus;

    // Use this for initialization
    void Start () {
	
	}
	   void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Beer_pong")
        {

          Ball = other.gameObject;
          Ball.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(Ball.gameObject);
            Invoke("Score_Pong", 0);
        }
        if (other.tag == "Doble_Pong")
        {
            Ball = other.gameObject;
            Destroy(Ball.gameObject);
            Invoke("Score_Pong_Doble", 0);
        }
    }

    public void Score_Pong()
    {
       GameObject Particles_Score = (GameObject)Instantiate(Particles, Point_Iniciate.position, Camera.main.transform.rotation);
        GameObject Points_Score = (GameObject)Instantiate(Points, Point_Iniciate.position, Camera.main.transform.rotation);
        Shooting_Poong.score += Total_Score;
        Destroy(Ball.gameObject);

		GetComponent<AudioSource>().Play();
        
        //Debug.Log("Cesta!");

    }
    public void Score_Pong_Doble()
    {
        GameObject Particles_Score = (GameObject)Instantiate(Particles, Point_Iniciate.position, Camera.main.transform.rotation);
        GameObject Points_Score = (GameObject)Instantiate(Bonus, Point_Iniciate.position, Camera.main.transform.rotation);
        Destroy(Ball.gameObject);
        Shooting_Poong.score += Total_Score*2;
//        Debug.Log("Cesta del bonus!");

		GetComponent<AudioSource>().Play();

    }
    // Update is called once per frame
    void Update()
    {
		

    }
}
