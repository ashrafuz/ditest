using UnityEngine;
using System.Collections;

public class Test_velocity : MonoBehaviour {


    public float power = 1000.0f;

    private Vector3 startPos ;
  
 void OnMouseDown()
    {
        startPos = Input.mousePosition;
        startPos.z = transform.position.z - Camera.main.transform.position.z;
        startPos = Camera.main.ScreenToWorldPoint(startPos);
    }

    void OnMouseUp()
    {
        var endPos = Input.mousePosition;
        endPos.z = transform.position.z - Camera.main.transform.position.z;
        endPos = Camera.main.ScreenToWorldPoint(endPos);

        var force = endPos - startPos;
        force.z = force.magnitude;
        force.Normalize();

        this.GetComponent<Rigidbody>().AddForce(force * power);
      //  ReturnBall();
    }

    void ReturnBall()
    {
       // yield WaitForSeconds(4.0);
       // transform.position = Vector3.zero;
///this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


}
