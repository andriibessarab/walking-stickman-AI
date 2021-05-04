using UnityEngine;

public class FollowTarget : MonoBehaviour
{   
    private Transform target;

    private float smoothSpeed = 0.125f;

    public Vector3 offset;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void LateUpdate()
    {
        if (target)
        {
            transform.position = new Vector3(target.position.x + offset.x, offset.y, offset.z); // follow target
        }
    }

}
