using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : Animal
{
    public override Material material()
    {
        return Resources.Load("Materials/Mouse", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Mouse;
    }

    void Start()
    {
        bodyScale = 0.3f;

        gender = Random.Range(1, 3);

        movSpeed = 4f * Random.Range(0.7f, 1.3f);
        fastMultiplier = 2.5f;
        slowMultiplier = 0.8f;
        stamina = 8 * Random.Range(0.7f, 1.3f);
        staminaRecharge = 2.5f * Random.Range(0.7f, 1.3f);

        hungerLim = 50 * Random.Range(0.7f, 1.3f);
        thirstLim = 40 * Random.Range(0.7f, 1.3f);
        ageLim = 200 * Random.Range(0.7f, 1.3f);
        reproductionLim = 10 * Random.Range(0.7f, 1.3f);
        litterSize = 4;

        nutrition = 20;
        sightRange = 40 * Random.Range(0.7f, 1.3f);

        prey.Add(Species.Grain);

        predator.Add(Species.Fox);
        predator.Add(Species.Owl);
        predator.Add(Species.Hawk);

        Setup();
    }
}
