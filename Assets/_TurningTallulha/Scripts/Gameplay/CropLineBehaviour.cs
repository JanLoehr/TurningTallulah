using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CropLineBehaviour : MonoBehaviour
{
    public float GrowDuration = 0.5f;

    private List<ScaleToOneAsync> _crops;

    private bool _triggered;

    // Start is called before the first frame update
    void Start()
    {
        _crops = transform.GetComponentsInChildren<ScaleToOneAsync>().ToList();

        for (int i = 0; i < _crops.Count; i++)
        {
            _crops[i].transform.localScale = Vector3.zero;

            _crops[i].transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (!_triggered)
        {
            _triggered = true;

            StartCoroutine(GrowCropsAsync());

            other.SendMessageUpwards("CroplineHit");
        }
    }

    private IEnumerator GrowCropsAsync()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        while (_crops.Count > 0)
        {
            int r = Random.Range(0, _crops.Count);

            _crops[r].StartScale(1);
            _crops.RemoveAt(r);

            yield return wait;
        }
    }
}
