using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAt : MonoBehaviour
{
    public Transform Target;

    private Transform _trans;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _trans.LookAt(Target, Vector3.up);
    }
}
