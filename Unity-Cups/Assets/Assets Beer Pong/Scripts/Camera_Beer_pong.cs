using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Serialization;


public class Camera_Beer_pong : MonoBehaviour
{
    public float RotationY;
    public float MinX;
    public float MaxX;

    public float RotationX;
    public float dragSpeed = 2;

    public GameObject Indicator;
    public GameObject BallObject;
    public GameObject Pointer;
    
    // Use this for initialization
    public GameObject Container;

    private Vector3 dragOrigin;
    private Vector3 dragOriginY;
    private Vector3 pos = Vector3.zero;
    private Vector3 move;
    private Camera _mainCam;
    private RectTransform _containerRect;

    void Start()
    {
        _mainCam = Camera.main;
        _containerRect = Container.GetComponent<RectTransform>();
    }
    
    void Update()
    {
        RotationX = move.x;
        if (Input.GetMouseButtonDown(0)) {
            if (RectTransformUtility.RectangleContainsScreenPoint(_containerRect, Input.mousePosition, _mainCam)) {
                dragOrigin = Input.mousePosition;
                return;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            move = new Vector3(0, 0, 0);
        }

        if (!Input.GetMouseButton(0)) {
            return;
        }
        
        if (RectTransformUtility.RectangleContainsScreenPoint(_containerRect, Input.mousePosition, _mainCam)) {
            pos = _mainCam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            move = new Vector3(pos.x * dragSpeed, 0, 0);
        }
        
        RotationX = Mathf.Clamp(RotationX, MinX, MaxX);
        
        Pointer.transform.localEulerAngles = new Vector3(RotationY, RotationX, 0);
        BallObject.transform.localEulerAngles = new Vector3(RotationY, RotationX, 0); 
        Indicator.transform.localEulerAngles = new Vector3(RotationY, RotationX * 3, 0);
    }
}