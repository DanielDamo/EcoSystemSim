using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    public override Material material()
    {
        return Resources.Load("Materials/Rabbit", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Rabbit;
    }

    void Start()
    {
        bodyScale = 0.8f;

        gender = Random.Range(1, 3);

        movSpeed = 9f * Random.Range(0.7f, 1.3f);
        fastMultiplier = 1.8f;
        slowMultiplier = 0.9f;
        stamina = 10 * Random.Range(0.7f, 1.3f);
        staminaRecharge = 5f * Random.Range(0.7f, 1.3f);

        hungerLim = 60 * Random.Range(0.7f, 1.3f);
        thirstLim = 50 * Random.Range(0.7f, 1.3f);
        ageLim = 220 * Random.Range(0.7f, 1.3f);
        reproductionLim = 12 * Random.Range(0.7f, 1.3f);
        litterSize = 3;

        nutrition = 40;
        sightRange = 50 * Random.Range(0.7f, 1.3f);

        prey.Add(Species.Carrot);
        prey.Add(Species.Grass);

        predator.Add(Species.Fox);
        predator.Add(Species.Owl);
        predator.Add(Species.Hawk);

        Setup();
    }
}
