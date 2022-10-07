using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasshopper : Animal
{
    public override Material material()
    {
        return Resources.Load("Materials/GrassHopper", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Grasshopper;
    }

    void Start()
    {
        bodyScale = 0.2f;
        gender = Random.Range(1, 3);

        movSpeed = 4f * Random.Range(0.7f, 1.3f);
        fastMultiplier = 2f;
        slowMultiplier = 0.8f;
        stamina = 5 * Random.Range(0.7f, 1.3f);
        staminaRecharge = 1.5f * Random.Range(0.7f, 1.3f);

        hungerLim = 30 * Random.Range(0.7f, 1.3f);
        thirstLim = 40 * Random.Range(0.7f, 1.3f);
        ageLim = 180 * Random.Range(0.7f, 1.3f);
        reproductionLim = 7 * Random.Range(0.7f, 1.3f);
        litterSize = 5;

        nutrition = 5;
        sightRange = 40 * Random.Range(0.7f, 1.3f);

        prey.Add(Species.Grain);
        prey.Add(Species.Grass);

        predator.Add(Species.Owl);
        predator.Add(Species.Hawk);

        Setup();
    }
}
