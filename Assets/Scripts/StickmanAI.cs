using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAI : MonoBehaviour {

    public GameObject head, torso, lowerTorso, leftLeg, rightLeg, leftKnee, rightKnee; // body parts

    private bool initilized = false; // initialization state
    private Vector3 initialPosition; // initial position

    public float speed = 5f; // speed
    private Color color; // color

    private NeuralNetwork net; // neural network

    private GameObject manager; // manager game object
    private GameObject camera; // camera game object

    private List<NeuralNetwork> nets; // empty list of nets
    private List<GameObject> bodyParts = new List<GameObject>(); // list of all body parts
    private List<GameObject> activeBodyParts = new List<GameObject>(); // list of active body parts

    void Start()
    {
        // Set inital position
        initialPosition = head.GetComponent<Transform>().position;

        // Find gameobjects by tags
        manager = GameObject.FindWithTag("Manager");
        camera = GameObject.FindWithTag("MainCamera");

        // Generate random color for stickman
        color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        // Add active body parts to list
        activeBodyParts.Add(torso);
        activeBodyParts.Add(lowerTorso);
        activeBodyParts.Add(leftLeg);
        activeBodyParts.Add(rightLeg);
        activeBodyParts.Add(leftKnee);
        activeBodyParts.Add(rightKnee);

        // Add all body parts to list
        bodyParts.AddRange(activeBodyParts);
        bodyParts.Add(head);

        // Color body parts
        foreach (GameObject i in bodyParts)
        {
            i.GetComponent<Renderer>().material.color = color;
        }
    }

    void FixedUpdate()
    {
        // Do if initialized
        if (initilized == true)
        {
            /* Inputs:
		    *    - distance from ground(i.o. minJointY)
            *    - height (excluding head; i.o. maxJointY)
		    *    - x velocity
		    *    - y velocity
		    *    - angular velocity
		    *    - creature rotation
		    */

            // Brain inputs
            var minJointY = activeBodyParts[0].GetComponent<HingeJoint2D>().transform.position.y;
            var maxJointY = activeBodyParts[0].GetComponent<HingeJoint2D>().transform.position.y;
            var velocityX = 0f;
            var velocityY = 0f;
            var angularVelocityZ = 0f;
            var pointsTouchingGround = 0f;
            var rotationZ = 0f;

            // Itterate over active body parts
            foreach (GameObject i in activeBodyParts)
            {
                var rigidbody = i.GetComponent<Rigidbody2D>();
                var joint = i.GetComponent<HingeJoint2D>();
                var collider = i.GetComponent<Collider2D>();
                var jointPos = joint.transform.position;

                // Determine lowest and highest joints
                if (jointPos.y > maxJointY)
                    maxJointY = jointPos.y;
                else if (jointPos.y < minJointY)
                    minJointY = jointPos.y;

                // Accumulate the velocity
                velocityX += rigidbody.velocity.x;
                velocityY += rigidbody.velocity.y;

                // Accumulate the angular velocity
                angularVelocityZ += rigidbody.angularVelocity;

                // Accumulate the rotation angle
                rotationZ += (rigidbody.transform.rotation.eulerAngles.z - 180f) * 0.002778f;
            }

            // Find average velocity, angular velocity, and rotation
            velocityX /= activeBodyParts.Count;
            velocityY /= activeBodyParts.Count;
            angularVelocityZ /= activeBodyParts.Count;
            rotationZ /= activeBodyParts.Count;

            // Handle inputs
            float[] inputs = new float[6]{ minJointY, maxJointY, velocityX, velocityY, angularVelocityZ, rotationZ };

            // Handle outputs
            float[] outputs = net.FeedForward(inputs);

            // Add torque to torso
            torso.GetComponent<Rigidbody2D>().AddTorque(activeBodyParts.IndexOf(torso) * 2000 * Time.fixedDeltaTime);

            // Itterate over active body parts and modify their moto speed
            foreach (GameObject i in activeBodyParts) {
                HingeJoint2D joint = i.GetComponent<HingeJoint2D>();
                JointMotor2D motor = joint.motor;
                motor.motorSpeed = outputs[activeBodyParts.IndexOf(i)] * speed;
                joint.motor = motor;
            }

            // Handle fitness
            net.SetFitness(head.transform.position.x - initialPosition.x);
        }
	}

    // If stickman fell, start next generation
    public void Fell()
    {
        // TODO
    }

    // Initialize
    public void Init(NeuralNetwork net)
    {
        this.net = net; // set stickman's neural system
        initilized = true; // set initialized to true
    }
}
