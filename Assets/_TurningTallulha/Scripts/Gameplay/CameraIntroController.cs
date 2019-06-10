using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntroController : MonoBehaviour
{
    public AnimationCurve PositionLerp;

    [Header("Object Refs")]
    public TallulhaControl Tallulah;

    private float _animationDuration;
    private float _currenAnimationStep;
    private FollowObject _followObject;

    private Transform _trans;

    // Start is called before the first frame update
    void Start()
    {
        _animationDuration = PositionLerp.keys[PositionLerp.keys.Length - 1].time;

        _followObject = GetComponent<FollowObject>();

        _trans = transform;

        enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_currenAnimationStep < _animationDuration)
        {
            _currenAnimationStep += Time.deltaTime;

            Vector3 newPos = Vector3.Lerp(Tallulah.IntroCamPosRef.position, Tallulah.GameCamPosRef.position, PositionLerp.Evaluate(_currenAnimationStep));
            _trans.position = new Vector3(newPos.x, _trans.position.y, newPos.z);

            _trans.LookAt(Tallulah.transform);
        }
        else
        {
            enabled = false;
            _followObject.enabled = true;
        }
    }

    public void StartIntroAnimation()
    {
        _followObject.enabled = false;
        enabled = true;
        _currenAnimationStep = 0;
    }
}
