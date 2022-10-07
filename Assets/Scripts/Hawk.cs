using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hawk : Animal
{
    public override Material material()
    {
        return Resources.Load("Materials/Hawk", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Hawk;
    }

    void Start()
    {
        bodyScale = 0.8f;

        gender = Random.Range(1, 3);

        movSpeed = 14f * Random.Range(0.7f, 1.3f);
        fastMultiplier = 4f;
        slowMultiplier = 0.2f;
        stamina = 3 * Random.Range(0.7f, 1.3f);
        staminaRecharge = 4f * Random.Range(0.7f, 1.3f);

        hungerLim = 50 * Random.Range(0.7f, 1.3f);
        thirstLim = 40 * Random.Range(0.7f, 1.3f);
        ageLim = 240 * Random.Range(0.7f, 1.3f);
        reproductionLim = 22 * Random.Range(0.7f, 1.3f);
        litterSize = 1;

        sightRange = 80 * Random.Range(0.7f, 1.3f);

        prey.Add(Species.Rabbit);
        prey.Add(Species.Grasshopper);
        prey.Add(Species.Mouse);

        Setup();
    }
}
