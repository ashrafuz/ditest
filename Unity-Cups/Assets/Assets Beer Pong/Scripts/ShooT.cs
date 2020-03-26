using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShooT : MonoBehaviour {

    public GameObject Manager_Pong;
    public GameObject Pointer;
    public GameObject Bar_force;
    public GameObject force;
    // Use this for initialization
    void Start () {
	
	}

    void OnMouseUp()
    {
        force.GetComponent<Slider>().value = 0;
        Pointer.gameObject.SetActive(false);
        Bar_force.gameObject.SetActive(false);
        Manager_Pong.GetComponent<Shooting_Poong>().shoot();
    }
    void OnMouseDown()
    {
       
        Pointer.gameObject.SetActive(true);
        Bar_force.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
