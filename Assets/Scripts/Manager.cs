using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject stickmanPrefab; // stickman game object
    public int populationSize; // num of stickmans per generation
    public int GenerationTime; // time after which generation resets

    private bool isTraning = false;
    private int generationNumber = 0;
    private List<StickmanAI> stickmanList = null;

    private List<NeuralNetwork> nets;
    private int[] layers = new int[] { 1, 10, 10, 1 };

    void Timer()
    {
        isTraning = false;
    }


	void Update ()
    {
        if (isTraning == false)
        {
            if (generationNumber == 0) // If first generation - create neural networkd for each stickman
            {
                InitStickmanNeuralNetworks();
            }
            else
            {
                nets.Sort(); // sort neural networks by fitness

                // Itterate over weaker half of population & give them new NeuralNetworks mutated from stronger half of population
                for (int i = 0; i < populationSize / 2; i++)
                {
                    nets[i] = new NeuralNetwork(nets[i+(populationSize / 2)]);
                    nets[i].Mutate();
                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); // reset neuron matrix values bu making deepcopy
                }

                // Itterate over population & reset their fitness
                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            generationNumber++; // increment generation by 1
            
            // Start training session
            isTraning = true;
            Invoke("Timer", GenerationTime); // set timer
            CreateStickmans(); // create stickmans
        }
    }


    private void CreateStickmans()
    {
        // If any stickmans exist - destroy them
        if (stickmanList != null)
        {
            for (int i = 0; i < stickmanList.Count; i++)
            {
                GameObject.Destroy(stickmanList[i].gameObject); // destroy GameObject
            }

        }

        stickmanList = new List<StickmanAI>(); // define stickmanList

        // Ittrate over population & create stickmans
        for (int i = 0; i < populationSize; i++)
        {
            StickmanAI currentStickman = ((GameObject)Instantiate(stickmanPrefab, new Vector3(0, 0, 0), stickmanPrefab.transform.rotation)).GetComponent<StickmanAI>();
            currentStickman.Init(nets[i]);
            stickmanList.Add(currentStickman);
        }

    }

    void InitStickmanNeuralNetworks()
    {
        // Make sure population is even
        if (populationSize % 2 != 0)
        {
            populationSize = 20; 
        }

        nets = new List<NeuralNetwork>(); // define nets list
        
        // Itterate over population, and create a neuralNetwork for each stickman
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate(); // mutate neural network
            nets.Add(net);
        }
    }
}