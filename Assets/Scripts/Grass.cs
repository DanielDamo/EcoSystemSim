using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Plant
{
    public override Material material()
    {
        return Resources.Load("Materials/Grass Patch", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Grass;
    }

    public override float posY()
    {
        return 3;
    }

    void Start()
    {
        bodyScale = 2;
        bodyScaleY = 1;

        maxNibbleCount = Mathf.FloorToInt(5 / Globals.harshness); 
        nutrition = 10 * Random.Range(0.7f, 1.3f);

        Setup();
    }
}
