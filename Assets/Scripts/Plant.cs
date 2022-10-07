using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Entity
{   
    public int nibbleCount { get; set; }
    protected int maxNibbleCount { private get; set; }
    protected float bodyScaleY { private get; set; }

    protected void Setup()
    {
        this.gameObject.transform.localScale = new Vector3(bodyScale, bodyScaleY, bodyScale);
        this.GetComponent<MeshRenderer>().material = material();
    }

    protected void Update()
    {
        if (nibbleCount >= maxNibbleCount) eaten();
    }

    protected void eaten()
    {
        Destroy(gameObject);
        Globals.popUpdate = true;
    }
}
