using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStats : MonoBehaviour
{
    // Text fields to update
    public GameObject GenerationNumberText;
    public GameObject Top10ListText;

    // Camera
    public GameObject camera;
    public Vector3 offset;

    public void UpdateGenerationNumberText(int generationNumber)
    {
        GenerationNumberText.GetComponent<TMPro.TextMeshProUGUI>().text = "Generation: " + generationNumber.ToString();
    }

    public void UpdateTop10ListText(List<NeuralNetwork> nets)
    {
        Top10ListText.GetComponent<TMPro.TextMeshProUGUI>().text = ""; // empty list

        nets.Sort((a, b) => b.CompareTo(a));

        int j;

        if (nets.Count < 10) j = nets.Count;
        else j = 10;
        
        for (int i = 0; i < j; i++)
        {
            Top10ListText.GetComponent<TMPro.TextMeshProUGUI>().text += nets[i].GetID().ToString("00000") + " | fitness:" + Math.Round(nets[i].GetFitness(), 4) + "\n";
        }

    }

    void LateUpdate()
    {
        transform.position = new Vector3(camera.transform.position.x + offset.x, camera.transform.position.y + offset.y, offset.z); // follow target; stay in camera view
    }
}
