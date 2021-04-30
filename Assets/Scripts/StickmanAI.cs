using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAI : MonoBehaviour {
    private bool initilized = false; // initialization state

    private NeuralNetwork net; // neural network

    void Start()
    {
    }

    void FixedUpdate ()
    {
        // Do script if initialized
        if (initilized == true)
        {
        }
	}

    // Initialize
    public void Init(NeuralNetwork net)
    {
        this.net = net; // set stickman's neural system
        initilized = true; // set initialized to true
    }
}
