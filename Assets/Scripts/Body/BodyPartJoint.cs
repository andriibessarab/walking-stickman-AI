using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartJoint : MonoBehaviour
{
	private Rigidbody2D body; // rigidbody

	private bool isCollidingWithGround; // state of colliding with ground

	private Dictionary<BodyPartBone, UnityEngine.Joint2D> joints = new Dictionary<BodyPartBone, UnityEngine.Joint2D>();

    /// <summary>
    /// Move joint
    /// </summary>
	public void MoveTo(Vector3 newPosition) {

		transform.position = newPosition;

		foreach (var connectedBone in joints.Keys) {
			connectedBone.RefreshBonePlacement();
		}
	}

	/// <summary>
    /// Connect joint with bones
    /// </summary>
	public void Connect(BodyPartBone bone, HingeJoint2d joint) {
		joints.Add(bone, joint);
	}

    /// <summary>
    /// Prepare for evolution
    /// </summary>
	public void PrepareForEvolution() {
		body = GetComponent<Rigidbody2D>();
		body.isKinematic = false;
	}

    /// <summary>
    /// Detect entering collision
    /// </summary>
	void OnTriggerEnter(Collider collider) {
		isCollidingWithGround = true;
	}

    /// <summary>
    /// Detect exiting collision
    /// </summary>
	void OnTriggerExit(Collider collider) {
		isCollidingWithGround = false;	
	}

    /// <summary>
    /// Return body
    /// </summary>
    public Rigidbody2D GetJointBody() {
        return body;
    }

	/// <summary>
    /// Return joint position
    /// </summary>
	public Vector3 GetJointPosition() {
		return transform.position;
	}

	public bool IsCollidingWithGround() {
		return isCollidingWithGround;
	}
}
