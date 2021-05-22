using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartBone : MonoBehaviour
{
    private const string PATH = "Prefabs/Bone";

	public BodyPartJoint startingJoint;
	public BodyPartJoint endingJoint;

	public Vector3 startingPoint => startingJoint.GetJointPosition();
	public Vector3 endingPoint => endingJoint.GetJointPosition();

	private Rigidbody body;

	/// <summary>
	/// Places the bone between the two joints
	/// </summary>
	public void RefreshBonePlacement() {

		if (startingJoint == null || endingJoint == null) return;

		var width = transform.localScale.z;
		var jPos1 = startingJoint.transform.position;
		var jPos2 = endingJoint.transform.position;

		PlaceBetweenPoints(jPos1, jPos2, width);
	}

	public void RefreshBonePlacement3D() {

		if (startingJoint == null || endingJoint == null) return;

		var width = transform.localScale.z;
		var jPos1 = startingJoint.transform.position;
		var jPos2 = endingJoint.transform.position;

		PlaceBetweenPoints3D(jPos1, jPos2, width);
	}

    /// <summary>
    /// Places the bone between the specified points
    /// </summary>
	
	private void PlaceBetweenPoints(Vector3 start, Vector3 end, float width) {

		start.z = 0;
		end.z = 0;

		PlaceBetweenPoints3D(start, end, width);
	}

	private void PlaceBetweenPoints3D(Vector3 start, Vector3 end, float width) {

		Vector3 offset = end - start;
		Vector3 scale = new Vector3(width, offset.magnitude / 2.0f, width);
		Vector3 position = start + (offset / 2.0f);


		transform.position = position;
		transform.up = offset;
		transform.localScale = scale;
	}

    /// <summary>
    /// Connects the gameobject to the starting end endingJoint
    /// </summary>
	public void ConnectToJoints() {

		if (startingJoint == null || endingJoint == null) return;

		startingJoint.Connect(this);
		endingJoint.Connect(this);
	}

	public void PrepareForEvolution () {
		
		body = GetComponent<Rigidbody>();
		body.isKinematic = false;
	}
}
