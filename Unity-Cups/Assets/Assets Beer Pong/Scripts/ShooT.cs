using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShooT : MonoBehaviour
{
    public static event Action<float> OnShoot;
    
    public Slider ForceSlider;
    public GameObject Pointer;
    public GameObject Bar_force;

    void OnMouseUp()
    {
        //Pointer.gameObject.SetActive(false);
        Bar_force.gameObject.SetActive(false);
        OnShoot?.Invoke(ForceSlider.value);
        ForceSlider.value = 0;
    }

    void OnMouseDown()
    {
        //Pointer.gameObject.SetActive(true);
        Bar_force.gameObject.SetActive(true);
    }
}