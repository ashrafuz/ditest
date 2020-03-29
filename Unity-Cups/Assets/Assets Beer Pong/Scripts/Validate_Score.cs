using UnityEngine;
using System.Collections;
using  System;

public class Validate_Score : MonoBehaviour
{
    public static event Action<GameObject, int> OnScore;
    public float Time_max_efect;
    public int Total_Score;
    public Transform Point_Iniciate;
    public GameObject Particles;
    public GameObject Points;
    public GameObject Bonus;

    private Camera _mainCam;
    private AudioSource _audioSource;

    // Use this for initialization
    void Start()
    {
        _mainCam = Camera.main;
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collided with " + other.gameObject);
        if (other.tag.Equals(GameConstants.SINGLE_PONG_TAG)) {
            Score_Pong(other.gameObject, false);
        }
        else if (other.tag.Equals(GameConstants.DOUBLE_PONG_TAG)) {
            Score_Pong(other.gameObject, true);
        }
    }

    public void Score_Pong(GameObject go, bool isDouble)
    {
        OnScore?.Invoke(go, isDouble ? Total_Score * 2 : Total_Score);
        Instantiate(Particles, Point_Iniciate.position, _mainCam.transform.rotation);
        if (isDouble) {
            Instantiate(Points, Point_Iniciate.position, _mainCam.transform.rotation);
        }
        else {
            Instantiate(Bonus, Point_Iniciate.position, _mainCam.transform.rotation);
        }

        _audioSource?.Play();
    }
}