using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBodyCollisionDetection : MonoBehaviour
{   
    void OnCollisionEnter2D(Collision2D collision)
    {
        // On collision with the ground report that stickman fell
        if (collision.gameObject.tag == "Ground")
        {
        transform.parent.GetComponent<StickmanAI>().Fell();
        }
    }
}
