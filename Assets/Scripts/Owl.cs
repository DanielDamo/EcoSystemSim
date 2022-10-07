using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : Animal
{
    public override Material material()
    {
        return Resources.Load("Materials/Owl", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Owl;
    }

    void Start()
    {
        bodyScale = 1;

        gender = Random.Range(1, 3);

        movSpeed = 10f * Random.Range(0.7f, 1.3f);
        fastMultiplier = 2.5f;
        slowMultiplier = 0.3f;
        stamina = 7 * Random.Range(0.7f, 1.3f);
        staminaRecharge = 3f * Random.Range(0.7f, 1.3f);

        hungerLim = 50 * Random.Range(0.7f, 1.3f);
        thirstLim = 40 * Random.Range(0.7f, 1.3f);
        ageLim = 270 * Random.Range(0.7f, 1.3f);
        reproductionLim = 22 * Random.Range(0.7f, 1.3f);
        litterSize = 1;

        sightRange = 65 * Random.Range(0.7f, 1.3f);

        prey.Add(Species.Rabbit);
        prey.Add(Species.Grasshopper);
        prey.Add(Species.Mouse);

        Setup();
    }
}
