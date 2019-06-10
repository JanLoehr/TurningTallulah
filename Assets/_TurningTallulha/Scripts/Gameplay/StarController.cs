using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    [Header("Parameters")]
    public float TurnSpeedModifier = 10;

    [Header("Audio")]
    public AudioSource AudioSource;
    public AudioClip BlingClip;

    private Transform _trans;
    private Transform _starTransform;

    private bool _isCollected;

    // Start is called before the first frame update
    void Start()
    {
        _trans = transform;
        _starTransform = _trans.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isCollected)
        {
            _starTransform.Rotate(0, TurnSpeedModifier * Time.deltaTime, 0, Space.World);
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.SendMessageUpwards("StarCollected");
            GetComponent<Collider>().enabled = false;

            _isCollected = true;
            StartCoroutine(StarCollectedAnimation());

            AudioSource.PlayOneShot(BlingClip);
        }
    }

    private IEnumerator StarCollectedAnimation()
    {
        float startTime = Time.time;

        while (startTime + 3 > Time.time)
        {
            _trans.localPosition += new Vector3(1, 1, 0);
            _starTransform.Rotate(0, TurnSpeedModifier * Time.deltaTime * 3, 0, Space.World);

            yield return null;
        }

        Destroy(gameObject);
    }
}
