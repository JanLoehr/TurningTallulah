using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{
    [Header("Parameters")]
    public Vector2 RotationBounds;

    public float RotationSpeedFactor = 10;

    [Header("ObjectRefs")]
    public Transform CameraTarget;

    private Transform _trans;
    private float _rotationDirection = -10;

    private float _rotation;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _trans.RotateAround(CameraTarget.position, Vector3.up, _rotationDirection * Time.deltaTime);
        _rotation += _rotationDirection * Time.deltaTime;

        if (_rotation < RotationBounds.x)
        {
            _rotationDirection = RotationSpeedFactor;
        }
        else if (_rotation > RotationBounds.y)
        {
            _rotationDirection = -RotationSpeedFactor;
        }
    }
}
