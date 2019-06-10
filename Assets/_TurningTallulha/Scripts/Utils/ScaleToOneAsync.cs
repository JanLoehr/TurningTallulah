using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToOneAsync : MonoBehaviour
{
    private Transform _trans;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
    }

    public void StartScale(float duration)
    {
        StartCoroutine(ScaleAsync(duration));
    }

    private IEnumerator ScaleAsync(float duration)
    {
        float lerp = 0;

        while (lerp < 1)
        {
            lerp += Time.deltaTime * (1 / duration);

            _trans.localScale = Vector3.one * lerp;

            yield return null;
        }
    }
}
