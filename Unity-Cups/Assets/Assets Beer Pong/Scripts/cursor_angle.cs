using UnityEngine;
using System.Collections;

public class cursor_angle : MonoBehaviour
{
    public float RotationY;
    public float MinX;
    public float MaxX;
    public float RotationX;

    public Vector3 pos = Vector3.zero;
    public Vector3 move;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    private Vector3 dragOriginY;

    // Use this for initialization
    public GameObject container;

    private Camera _mainCam;
    private RectTransform _containerRect;

    void Start()
    {
        _mainCam = Camera.main;
        //_containerRect = container.GetComponent<RectTransform>();
    }

    // Update is called once per frame
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
        transform.localEulerAngles = new Vector3(RotationY, RotationX * 3, 0);
    }
}