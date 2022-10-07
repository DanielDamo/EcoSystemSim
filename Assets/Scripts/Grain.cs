using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grain : Plant
{
    public override Material material()
    {
        return Resources.Load("Materials/Grain Patch", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Grain;
    }

    public override float posY()
    {
        return 3.5f;
    }

    void Start()
    {
        bodyScale = 2;
        bodyScaleY = 2;

        maxNibbleCount = Mathf.FloorToInt(3 / Globals.harshness);
        nutrition = 12 * Random.Range(0.7f, 1.3f);

        Setup();
    }  
}
