using System;
using UnityEngine;
using System.Collections;

public class Test_velocity : MonoBehaviour {


    public float power = 1000.0f;

    private Vector3 startPos ;
    private Camera _mainCam;
    private Rigidbody _myRigidbody;

    private void Start()
    {
        _mainCam = Camera.main;
        _myRigidbody = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        startPos = Input.mousePosition;
        startPos.z = transform.position.z - _mainCam.transform.position.z;
        startPos = _mainCam.ScreenToWorldPoint(startPos);
    }

    void OnMouseUp()
    {
        var endPos = Input.mousePosition;
        endPos.z = transform.position.z -_mainCam.transform.position.z;
        endPos = _mainCam.ScreenToWorldPoint(endPos);

        var force = endPos - startPos;
        force.z = force.magnitude;
        force.Normalize();

        _myRigidbody.AddForce(force * power);
      //  ReturnBall();
    }

    void ReturnBall()
    {
       // yield WaitForSeconds(4.0);
       // transform.position = Vector3.zero;
///this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


}
