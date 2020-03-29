using UnityEngine;
using System.Collections;

public class Delete_Mising_ball : MonoBehaviour {
    public BallSpawner Spawner;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameConstants.SINGLE_PONG_TAG) ||  other.tag.Equals(GameConstants.DOUBLE_PONG_TAG))
        {
            Spawner?.HideBall(other.gameObject);
        }
    }

}
