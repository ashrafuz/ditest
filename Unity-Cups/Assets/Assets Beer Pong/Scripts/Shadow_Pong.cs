using UnityEngine;
using System.Collections;

public class Shadow_Pong : MonoBehaviour
{
    private GameObject _target;
    private Vector3 _offset;

    public void SetTarget(Transform transform)
    {
        _target = transform.gameObject;
        _offset = transform.position - _target.transform.position;
    }

    void LateUpdate()
    {
        if (_target != null) {
            transform.position = _target.transform.position + _offset;
        }
    }
}