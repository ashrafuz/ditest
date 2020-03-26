using UnityEngine;
using System.Collections;

public class Shadow_Pong : MonoBehaviour {
    public GameObject target;

    public Vector3 Offset;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Beer_pong");

        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Offset = transform.position - target.transform.position;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
        }
        else
        {
            transform.position = target.transform.position + Offset;
        }   
    }
    void Update()
    {
        if (target == null)
        {
          Destroy(gameObject);
        }
        if (target ==GameObject.FindGameObjectWithTag("Doble_Pong"))
        {
            Invoke("Destroy", 1);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
