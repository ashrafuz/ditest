using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShooT : MonoBehaviour
{
    public static event Action<float> OnShoot;
    
    public Slider ForceSlider;
    public GameObject Bar_force;

    void OnMouseUp()
    {
        Bar_force.gameObject.SetActive(false);
        OnShoot?.Invoke(ForceSlider.value);
        ForceSlider.value = 0;
    }

    void OnMouseDown()
    {
        Bar_force.gameObject.SetActive(true);
    }
}