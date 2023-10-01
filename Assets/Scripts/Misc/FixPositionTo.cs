using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPositionTo : MonoBehaviour
{
    [Tooltip("The gameobject of this component will be set to the this targets position.")]
    public Transform target;
    public Vector3 offset = Vector3.zero;

    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
        {
            transform.position = target.position;
            transform.position += offset;
        }
        else
        {
            Debug.LogWarning("This FixPositionTo component does not have a target");
        }
    }
}
