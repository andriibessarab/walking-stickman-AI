using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAI : MonoBehaviour {
    private bool initilized = false; // initialization state
    private NeuralNetwork net; // neural network
    private float speed; // speed
    private GameObject manager; // manager game object
    private GameObject camera; // camera game object
    private List<NeuralNetwork> nets;

    // Body parts
    public Transform head, torso, lowerTorso, leftLeg, rightLeg, leftKnee, rightKnee; // body parts
    Rigidbody2D torsoRB, lowerTorsoRB, leftLegRB, rightLegRB, leftKneeRB, rightKneeRB; // body parts' rigid bodys
    HingeJoint2D torsoJoint,lowerTorsoJoint, leftLegJoint, rightLegJoint, leftKneeJoint, rightKneeJoint; // body parts' joints

    void Start()
    {
        // Get game objects by tags
        manager = GameObject.FindWithTag("Manager");
        camera = GameObject.FindWithTag("MainCamera");

        speed = 60f; // speed

        Color color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        // Color body parts
        head.GetComponent<Renderer>().material.color = color;
        torso.GetComponent<Renderer>().material.color = color;
        lowerTorso.GetComponent<Renderer>().material.color = color;
        leftLeg.GetComponent<Renderer>().material.color = color;
        rightLeg.GetComponent<Renderer>().material.color = color;
        leftKnee.GetComponent<Renderer>().material.color = color;
        rightKnee.GetComponent<Renderer>().material.color = color;

        // Rigidbody2D
        torsoRB = torso.GetComponent<Rigidbody2D>();
        lowerTorsoRB = lowerTorso.GetComponent<Rigidbody2D>();
        leftLegRB = leftLeg.GetComponent<Rigidbody2D>();
        rightLegRB = rightLeg.GetComponent<Rigidbody2D>();
        leftKneeRB = leftKnee.GetComponent<Rigidbody2D>();
        rightKneeRB = rightKnee.GetComponent<Rigidbody2D>();

        // HingeJoint2D
        torsoJoint = torso.GetComponent<HingeJoint2D>();
        lowerTorsoJoint = lowerTorso.GetComponent<HingeJoint2D>();
        leftLegJoint = leftLeg.GetComponent<HingeJoint2D>();
        rightLegJoint = rightLeg.GetComponent<HingeJoint2D>();
        leftKneeJoint = leftKnee.GetComponent<HingeJoint2D>();
        rightKneeJoint = rightKnee.GetComponent<HingeJoint2D>();
    }

    void FixedUpdate()
    {
        // Do script if initialized
        if (initilized == true)
        {
            // Angles
            float torsoAngle = torso.transform.rotation.eulerAngles.z;
            float lowerTorsoAngle = lowerTorso.transform.rotation.eulerAngles.z;
            float leftLegAngle = leftLeg.transform.rotation.eulerAngles.z;
            float rightLegAngle = rightLeg.transform.rotation.eulerAngles.z;
            float leftKneeAngle = leftKnee.transform.rotation.eulerAngles.z;
            float rightKneeAngle = rightKnee.transform.rotation.eulerAngles.z;

            // AngularVelocity
            float torsoAV = torsoRB.angularVelocity;
            float leftLegAV  = leftLegRB.angularVelocity;
            float rightLegAV  = rightLegRB.angularVelocity;

            // Height
            float height = torso.position.y + 1.7f;
            if (height < -1) height = -1;
            if (height > 1) height = 1;

            // Handle inputs
            float[] inputs = new float[10]{ torsoAngle, lowerTorsoAngle, leftLegAngle, rightLegAngle, leftKneeAngle, rightKneeAngle, torsoAV, leftLegAV, rightLegAV, height };

            // Handle outputs
            float[] outputs = net.FeedForward(inputs);

            // Speed
            float leftLegSpeed = outputs[0];
            float leftKneeSpeed = outputs[1];
            float rightLegSpeed = outputs[2];
            float rightKneeSpeed = outputs[3];
            float torsoSpeed = outputs[4];

            // Add torque to torso
            torsoRB.AddTorque(torsoSpeed * 2000 * Time.fixedDeltaTime);

            // Motors
            JointMotor2D leftLegMotor = leftLegJoint.motor;
            leftLegMotor.motorSpeed = leftLegSpeed * speed;
            leftLegJoint.motor = leftLegMotor;

            JointMotor2D leftKneeMotor = leftKneeJoint.motor;
            leftKneeMotor.motorSpeed = leftKneeSpeed * speed;
            leftKneeJoint.motor = leftKneeMotor;

            JointMotor2D rightLegMotor = rightLegJoint.motor;
            rightLegMotor.motorSpeed = rightLegSpeed * speed;
            rightLegJoint.motor = rightLegMotor;

            JointMotor2D rightKneeMotor = rightKneeJoint.motor;
            rightKneeMotor.motorSpeed = rightKneeSpeed * speed;
            rightKneeJoint.motor = rightKneeMotor;

            JointMotor2D torsoMotor = torsoJoint.motor;
            torsoMotor.motorSpeed = torsoSpeed * speed;
            torsoJoint.motor = torsoMotor;

            // Handle fitness
            net.SetFitness(head.transform.position.x);

            nets = manager.GetComponent<Manager>().GetNeuralNetworks();
            nets.Sort((a, b) => b.CompareTo(a)); // sort nets by fitness

            int stickmanRank = nets.IndexOf(net); // get stickman's rank

            // Show only if has the best fitness
            if (stickmanRank == 0)
            {
                camera.GetComponent<FollowTarget>().SetTarget(this.GetComponent<Transform>()); // set camera to follow this stickman

                // Show bopdy parts
                head.GetComponent<Renderer>().enabled = true;
                torso.GetComponent<Renderer>().enabled = true;
                lowerTorso.GetComponent<Renderer>().enabled = true;
                leftLeg.GetComponent<Renderer>().enabled = true;
                rightLeg.GetComponent<Renderer>().enabled = true;
                leftKnee.GetComponent<Renderer>().enabled = true;
                rightKnee.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                // Hide body parts
                head.GetComponent<Renderer>().enabled = false;
                torso.GetComponent<Renderer>().enabled = false;
                lowerTorso.GetComponent<Renderer>().enabled = false;
                leftLeg.GetComponent<Renderer>().enabled = false;
                rightLeg.GetComponent<Renderer>().enabled = false;
                leftKnee.GetComponent<Renderer>().enabled = false;
                rightKnee.GetComponent<Renderer>().enabled = false;
            }
        }
	}

    // If stickman fell, start next generation
    public void Fell()
    {
        nets.Sort((a, b) => b.CompareTo(a)); // sort nets by fitness

        int stickmanRank = nets.IndexOf(net); // get stickman's rank

        // If stickman is top 1 and he fell - go to next generation
        if (stickmanRank == 0)
        {
            manager.GetComponent<Manager>().Timer();
        }
    }

    // Initialize
    public void Init(NeuralNetwork net)
    {
        this.net = net; // set stickman's neural system
        initilized = true; // set initialized to true
    }
}
