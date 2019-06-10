using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        other.SendMessageUpwards("FinishHit");
    }
}
