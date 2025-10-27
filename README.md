# 🚶‍♂️ AI-Powered Stickman Locomotion Simulator

![](misc/walking_stickman_ai_preview.gif)

[ **RUN SIMULATION** ](https://play.unity.com/en/games/6a5da469-8c89-40d0-9c48-9a523bc01ccd/stickman-ai-learns-to-walk)

---

## **Project Overview**

This project is a Unity-based physics simulation designed to teach a bipedal stickman how to walk. It uses a
custom-built neural network and a genetic algoirthm to evolve successful locomotion strategies through unsupervised
learning. I made this project to explore basic artificial Intelligence and machine learning principles.

---

## **Technical Highlights**

| Category                | Component                 | Focus                                                                                                                               |
|:------------------------|:--------------------------|:------------------------------------------------------------------------------------------------------------------------------------|
| **Engine & Language**   | **Unity (C#)**            | Built the entire simulation environment, physics model, and core AI logic in C#.                                                    |
| **ML Model**            | **Custom Neural Network** | Implemented a fully custom, feed-forward network from scratch to directly control joint rotation angles.                            |
| **AI Technique**        | **Genetic Algorithm**     | Drove the learning process through selection, crossover, and mutation of the network's weights, optimizing for distance traveled.   |
| **Activation Function** | *_Tanh_*         | Used for non-linear neuron activation, crucial for complex movement control.                                                        |

### **The Learning Process**

The AI agents were trained through an evolutionary process:

1. **Fitness Scoring:** Each stickman was scored based on the distance traveled.
2. **Evolution:** Networks with high fitness were selected to pass their traits to the next generation.
3. **Mutation:** A controlled random variation was applied to the weights to ensure the AI continued to explore and
   improve its walking technique across over 1000 generations.

---

## **Results & Conclusion**

* **Impact:** This project provided me with hands-on, low-level experience in building core ML components, demonstrating
  how **evolutionary computation** can be applied to solve dynamic physics and control challenges.
* **Outcome:** After training for 1000+ generations, the stickman successfully passed a  distance of ~50 meters, in human-like walking motion.
* **Future Work:** The joint restrictions should be adjusted prevent the stickmen from adopting a wide-legged gait, which provide a stable standing pose but is not a natural walking style.

---

_Century Private School Grade 8 Science Final Project - May 2021_

_By Andrii Bessarab_
