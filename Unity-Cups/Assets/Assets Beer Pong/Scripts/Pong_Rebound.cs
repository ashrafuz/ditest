using UnityEngine;
using System.Collections;

public class Pong_Rebound : MonoBehaviour
{
    // NOT SURE WHAT ITS DOING, but i am sure there is a better way to handle it than this
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SINGLE_PONG_TAG)) {
            other.gameObject.tag = GameConstants.DOUBLE_PONG_TAG;
        }
    }
}