using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    public int Shakes = 4;

    public float MaxOffset = 1;

    public float JitterDuration = 0.1f;

    private Transform _trans;

    private Vector3 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
        _startPos = _trans.localPosition;
    }

    public void CameraShake()
    {
        StartCoroutine(CameraShakeAsync());
    }

    private IEnumerator CameraShakeAsync()
    {
        for (int i = 0; i < Shakes; i++)
        {
            Vector3 target = _startPos + new Vector3(Random.Range(-MaxOffset, MaxOffset), Random.Range(-MaxOffset, MaxOffset));

            float lerp = 0;
            while (lerp < 1)
            {
                lerp += Time.deltaTime * 1 / JitterDuration;

                _trans.localPosition = Vector3.Lerp(_trans.localPosition, target, lerp);

                yield return null;
            }
        }

        _trans.localPosition = _startPos;
    }
}
