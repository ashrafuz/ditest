using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class Validate_Score : MonoBehaviour
{
    public float Time_max_efect;
    public int Total_Score;
    public Transform Point_Iniciate;
    public GameObject Particles;
    public GameObject Points;
    public GameObject Bonus;

    private Camera _mainCam;

    // Use this for initialization
    void Start()
    {
        _mainCam = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Beer_pong")) {
            other.GetComponent<Rigidbody>().isKinematic = true;
            Score_Pong(false);
        }
        else if (other.tag.Equals("Doble_Pong")) {
            Score_Pong(true);
        }
    }

    public void Score_Pong(bool isDouble)
    {
        GameObject Particles_Score =
            (GameObject) Instantiate(Particles, Point_Iniciate.position, _mainCam.transform.rotation);
        if (isDouble) {
            Instantiate(Points, Point_Iniciate.position, _mainCam.transform.rotation);
            Shooting_Poong.score += Total_Score;
        }
        else {
            GameObject Points_Score =
                (GameObject) Instantiate(Bonus, Point_Iniciate.position, _mainCam.transform.rotation);
            Shooting_Poong.score += Total_Score * 2;
        }
        GetComponent<AudioSource>().Play();
    }
}