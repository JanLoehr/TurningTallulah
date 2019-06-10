using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform ObjectRef;

    public bool KeepHeight;

    private Transform _trans;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        _trans.position = new Vector3(ObjectRef.position.x, KeepHeight ? _trans.position.y : ObjectRef.position.y, _trans.position.z);

        if (KeepHeight)
        {
            _trans.LookAt(ObjectRef);
        }
    }
}
