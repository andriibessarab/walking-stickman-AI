using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject stickmanPrefab; // stickman game object
    public GameObject statsCanvas; // game object with update stats funcs

    public int populationSize; // num of stickmans per generation
    public int GenerationTime; // time after which generation resets

    private bool isTraning = false;
    private int generationNumber = 0;
    private int nextID = 1;
    private List<StickmanAI> stickmanList = null;

    private List<NeuralNetwork> nets;
    private int[] layers = new int[] { 6, 10, 10, 6 };

    public void Timer()
    {
        isTraning = false;
    }

	void Update()
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
                    nets[i].SetID(nextID); // update net's ID

                    nextID++; // increment nextID
                }
            }

            generationNumber++; // increment generation by 1
            statsCanvas.GetComponent<UpdateStats>().UpdateGenerationNumberText(generationNumber); // update gen num stat
            
            // Start training session
            isTraning = true;
            Invoke("Timer", GenerationTime); // set timer
            CreateStickmans(); // create stickmans
        }
        else
        {
            statsCanvas.GetComponent<UpdateStats>().UpdateTop10ListText(nets); // update top10  stat
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
            StickmanAI currentStickman = ((GameObject)Instantiate(stickmanPrefab, new Vector3(Random.Range(-10f, 10f), 0, 0), stickmanPrefab.transform.rotation)).GetComponent<StickmanAI>();
            currentStickman.Init(nets[i]);
            stickmanList.Add(currentStickman);
        }
    }

    void InitStickmanNeuralNetworks()
    {
        // Make sure population is even
        if (populationSize % 2 != 0)
        {
            populationSize += 1; 
        }

        nets = new List<NeuralNetwork>(); // define nets list
        
        // Itterate over population, and create a neuralNetwork for each stickman
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate(); // mutate neural network
            net.SetID(nextID); // set net's ID
            nets.Add(net); // add this net to list

            nextID++; // increment nextID
        }
    }

    public List<NeuralNetwork> GetNeuralNetworks()
    {
        return nets;
    }
}
