﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;


public class Camera_Beer_pong : MonoBehaviour
{
    public float turnSpeed = 50f;
    public float MinY;
    public float Maxy;
    public float RotationY;
    public float MinX;
    public float MaxX;

    public float RotationX;

    //  public Joystick _Joystick;
    public Vector3 pos = Vector3.zero;
    public Vector3 move;
    public Vector3 moveY;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 dragOriginY;

    public GameObject AngleY;

    // Use this for initialization
    public GameObject container;

    private Camera _mainCam;
    private RectTransform _containerRect;

    void Start()
    {
        _mainCam = Camera.main;
        _containerRect = container.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // RotationY = _Joystick.JoystickInput.y*10;
        RotationX = move.x;
        ///  RotationY = AngleY.GetComponent<Slider>().value-15;
        //  RotationY = moveY.y;

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

        //    RotationY += 0 * Time.deltaTime;
        //RotationY = Mathf.Clamp(RotationY, MinY, Maxy);
        //transform.localEulerAngles = new Vector3(RotationY, RotationX, 0);

        RotationX += 0 * Time.deltaTime;
        RotationX = Mathf.Clamp(RotationX, MinX, MaxX);
        transform.localEulerAngles = new Vector3(RotationY, RotationX, 0);
    }
}