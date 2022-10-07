using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{ 
    public override Material material()
    {
        return Resources.Load("Materials/Fox", typeof(Material)) as Material;
    }
        
    public override Species species()
    {
        return Species.Fox;
    }      

    void Start()
    {
        gender = Random.Range(1, 3);
        bodyScale = 1;

        movSpeed = 10f * Random.Range(0.7f, 1.3f);
        fastMultiplier = 2;
        slowMultiplier = 0.3f;
        stamina = 6 * Random.Range(0.7f, 1.3f);
        staminaRecharge = 2 * Random.Range(0.7f, 1.3f);

        hungerLim = 60 * Random.Range(0.7f, 1.3f);
        thirstLim = 50 * Random.Range(0.7f, 1.3f);
        ageLim = 300 * Random.Range(0.7f, 1.3f);
        reproductionLim = 20 * Random.Range(0.7f, 1.3f);
        litterSize = 2;

        nutrition = 50;
        sightRange = 65 * Random.Range(0.7f, 1.3f);

        prey.Add(Species.Rabbit);
        prey.Add(Species.Mouse);
        prey.Add(Species.Carrot);

        predator.Add(Species.Owl);

        Setup();
    }
}
