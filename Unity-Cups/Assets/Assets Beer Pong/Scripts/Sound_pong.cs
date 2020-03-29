using System;
using UnityEngine;
using System.Collections;

public class Sound_pong : MonoBehaviour
{
    private GameObject Ball;
    public float VOL_Pong;
    public float Vel;
    public GameObject Sound;

    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2) {
            audio.Play();
            VOL_Pong -= 1;
            Vel += 0.1f;
            audio.volume -= VOL_Pong;
            if (audio.pitch < 2.8f) {
                audio.pitch += Vel;
            }
        }
    }
}