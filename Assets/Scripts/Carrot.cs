using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Plant
{
    public override Material material()
    {
        return Resources.Load("Materials/Carrot Patch", typeof(Material)) as Material;
    }

    public override Species species()
    {
        return Species.Carrot;
    }

    public override float posY()
    {
        return 2.75f;
    }

    void Start()
    {
        bodyScale = 1;
        bodyScaleY = 0.5f;

        maxNibbleCount = Mathf.FloorToInt(4 / Globals.harshness); 
        nutrition = 14 * Random.Range(0.7f, 1.3f);

        Setup();
    }
}
