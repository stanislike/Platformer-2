using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float offsetZ = -10f;
    private Vector3 velocity = Vector3.zero;

    public BoxCollider2D boundsCollider;
    
    void FixedUpdate()
    {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, offsetZ));

        

        if (!boundsCollider.bounds.Contains(targetPosition))
        {
            targetPosition = boundsCollider.bounds.ClosestPoint(targetPosition);
            targetPosition.z = offsetZ;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
