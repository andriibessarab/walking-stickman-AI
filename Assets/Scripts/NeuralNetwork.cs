using System;
using System.Collections.Generic;

/// <summary>
/// Neural Network C#(Unsupervised Learning)
/// </summary>

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] layers; // layers
    private float[][] neurons; // neuron matrix
    private float[][][] weights; // weight matrix
    private float fitness; // fitness of the network
    private int ID; // network's ID

    /// <summary>
    /// Initialize neural network with random weights
    /// </summary>
    /// <param name="layers"> Layers to neural networks </param>
    public NeuralNetwork(int[] layers)
    {

        // Initialize layers
        this.layers = new int[layers.Length];   
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        // Generate matrix
        InitNeurons();
        InitWeights();

    }

    /// <summary>
    /// Deep copy constructor
    /// </summary>
    /// <param name="copyNetwork"> Network to deep copy </param>
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];   
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);

    }

    private void CopyWeights(float[][][] copyWeights)
    {
        for(int i = 0; i < weights.Length; i++)
        {
            for(int j = 0; j < weights[i].Length; j++)
            {
                for(int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    /// <summary>
    /// Create neurons matrix
    /// </summary>
    private void InitNeurons()
    {
        // Initialize neuronsList
        List<float[]> neuronsList = new List<float[]>();

        for(int i = 0; i < layers.Length; i++) // run through all neurons
        {
            neuronsList.Add(new float[layers[i]]); // add layer to neuronsList
        }

        neurons = neuronsList.ToArray();
    }

    /// <summary>
    /// Create weights matrix
    /// </summary>
    private void InitWeights()
    {
        // Initialize weightsList
        List<float[][]> weightsList = new List<float[][]>(); // weightsList(will be converted into weights)

        // Iterate over all neurons that have weight connection
        for(int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>(); // layerWeightList for current layer(will be converted into 2D array)

            int neuronsInPreviousLayer = layers[i - 1];

            // Itterate over all neurons in current layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; // neuronWeights

                // Itterate over all neurons in previous layer and set weights randomly between -0.5f and 0.5f
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    // Give random weights to neuron weights
                    neuronWeights[k] =  UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights); // add neuron weights of current level to layerWeightsList
            }

            weightsList.Add(layerWeightsList.ToArray()); // add current layer's weights converted into 2D array into weightsList
        }

        weights = weightsList.ToArray(); // convert weightsList into 3D array
    }

    /// <summary>
    /// Feed forward neural network with a given input array
    /// </summary>
    /// <param name="inputs"> Inputs to neural networks </param>
    /// <returns></returns>
    public float[] FeedForward(float[] inputs)
    {
        // Add inputs to the neural matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        // Itterate over all neurons and compute feedforward values
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; // sum of all weights connections of this neuron weight their values in previous layer
                }

                neurons[i][j] = (float)Math.Tanh(value); // hyperbolic tangent activation
            }
        }

        return neurons[neurons.Length - 1]; // return output layer
    }

    /// <summary>
    /// Mutate neural network weights
    /// </summary>
    public void Mutate()
    {
        for(int i = 0; i < weights.Length; i++)
        {
            for(int j = 0; j < weights[i].Length; j++)
            {
                for(int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //Mutate current weight's value
                    float randomNumber = UnityEngine.Random.Range(0f, 100f);

                    if (randomNumber <= 1f) // Case 1 - Flip sign of weight
                    {
                        weight *= -1f;
                    }
                    else if (randomNumber <= 2f) // Case 2 - Pick random weight between -1 and 1
                    {
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 3f) // Case 3 - Randomly increase by 0% to 100%
                    {
                        weight *= UnityEngine.Random.Range(0f, 1f) +1f;
                    }
                    else if (randomNumber <= 4f) // Case 4 - Random;y decrease by 0% to 100%
                    {
                        weight *= UnityEngine.Random.Range(0f, 1f);
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void SetID(int newID)
    {
        ID = newID;
    }
    
    public int GetID()
    {
        return ID;
    }

    public void SetFitness(float fit)
    {
        fitness = fit;
    }
    
    public float GetFitness()
    {
        return fitness;
    }

    /// <summary>
    /// Compare two neural networks and sort based on fitness
    /// </summary>
    /// <param name="other"> Network to be compared to </param>
    /// <returns></returns>
    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }
}