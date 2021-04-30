using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    public Transform target;    

    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed);
    }

}
