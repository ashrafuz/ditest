using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointBehavior : MonoBehaviour
{
    private float startTime;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        Object.Destroy(this.gameObject, 3.0f);
        sr = gameObject.GetComponent("SpriteRenderer") as SpriteRenderer;
    }

    // Update is called once per frame
    void Update()
    {
        Transper(0.5f, 0.5f);
    }

    void Transper(float wait, float trans){
        if(Time.time < startTime + wait){
            // Do nothing
        }else if(Time.time > (startTime + wait + trans)){
            sr.color = new Color(1f,1f,1f,0f);
        }else{
            sr.color = new Color(1f,1f,1f,1f-((Time.time - startTime - wait)/trans));
        }

    }
}
