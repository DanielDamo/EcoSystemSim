using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Globals
{
    public static int grassPop { get; set; }
    public static int carrotPop { get; set; }
    public static int grainPop { get; set; }
    public static int deathByAge { get; set; }
    public static int deathByStarvation { get; set; }
    public static int deathByThirst { get; set; }
    public static int deathByHunter { get; set; }
    public static int[] maxPops { get; set; } = new int[3];
    public static int[] startPops { get; set; } = new int[9];
    public static bool popUpdate { get; set; }
    public static float simSpeed { get; set; } = 1;
    public static float harshness { get; set; }


    public static void textSetup(string name, string text, int x, int y, int fontSize, Color colour, Transform parent, List<Text> list = null, int rotation = 0)
    {
        Text t;
        GameObject Object = new GameObject(name, typeof(RectTransform));
        t = Object.AddComponent<Text>();
        t.transform.SetParent(parent.transform);
        t.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        t.text = text;
        t.transform.localPosition = new Vector2(x, y);
        t.fontSize = fontSize;
        t.color = colour;
        t.fontStyle = FontStyle.Bold;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        if (list != null) list.Add(t);
        if (rotation != 0) Object.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, rotation);
    }

    public static void componentAdder(Entity entity, GameObject body)
    {
        if (entity.species() == Species.Grass) body.AddComponent<Grass>();
        else if (entity.species() == Species.Carrot) body.AddComponent<Carrot>();
        else if (entity.species() == Species.Grain) body.AddComponent<Grain>();
        else if (entity.species() == Species.Grasshopper) body.AddComponent<Grasshopper>();
        else if (entity.species() == Species.Mouse) body.AddComponent<Mouse>();
        else if (entity.species() == Species.Rabbit) body.AddComponent<Rabbit>();
        else if (entity.species() == Species.Owl) body.AddComponent<Owl>();
        else if (entity.species() == Species.Hawk) body.AddComponent<Hawk>();
        else if (entity.species() == Species.Fox) body.AddComponent<Fox>();
    }
}


public class Entity : MonoBehaviour
{
    protected float bodyScale { get; set; }
    public float nutrition { get; protected set; }
    virtual public Material material()
    {
        return null;
    }
    virtual public float posY()
    {
        return 5;
    }
    virtual public Species species()
    {
        //placeholder value, overwritten for every species
        return Species.Mouse; 
    }
}



public class EntitySpawning : MonoBehaviour
{
    private float grassSpawnCounter { get; set; }
    private float carrotSpawnCounter { get; set; }
    private float grainSpawnCounter { get; set; }
    private float grassSpawnTime { get; } = 3 * Globals.harshness;
    private float carrotSpawnTime { get; } = 6 * Globals.harshness;
    private float grainSpawnTime { get; } = 5 * Globals.harshness;
    private float grassRandMult { get; set; } = 1;
    private float carrotRandMult { get; set; } = 1;
    private float grainRandMult { get; set; } = 1;

    private List<Entity> EList { get; set; } = new List<Entity>();

    void Start()
    {
        EList.AddRange(new List<Entity>() {
            new Grass(),
            new Carrot(),
            new Grain(),
            new Grasshopper(),
            new Mouse(),
            new Rabbit(),
            new Owl(),
            new Hawk(),
            new Fox()
        }) ;

        //spawns random sized and random amount of water
        int waterAmount = Random.Range(8, 18);
        GameObject water = GameObject.CreatePrimitive(PrimitiveType.Cube); 
        water.GetComponent<MeshRenderer>().material = Resources.Load("Materials/Water", typeof(Material)) as Material;
        water.GetComponent<BoxCollider>().size = new Vector3(1, 50, 1);
        water.gameObject.tag = "Water";
        for (int i = 0; i<= waterAmount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(40, 260), 2.05f, Random.Range(40, 260));
            water.transform.localScale = new Vector3(Random.Range(10, 80), 1, Random.Range(20, 80));
            Instantiate(water, spawnPos, Quaternion.identity);
        }
        Destroy(water.gameObject);

        //instantiates starting population amount of all entities
        int count = 0;
        foreach (Entity entity in EList)
        {
            for (int i = 0; i < Globals.startPops[count]; i++)
            {
                randomPosInstantiator(entity,count);
            }
                count++;
        }
    }

    void Update()
    {
        grassSpawnCounter += Time.deltaTime * Globals.simSpeed;
        carrotSpawnCounter += Time.deltaTime * Globals.simSpeed;
        grainSpawnCounter += Time.deltaTime * Globals.simSpeed;

        if (grassSpawnCounter > grassSpawnTime * grassRandMult && Globals.grassPop < Globals.maxPops[0]) plantSpawn(EList[0]);  
        if (carrotSpawnCounter > carrotSpawnTime * carrotRandMult && Globals.carrotPop < Globals.maxPops[1]) plantSpawn(EList[1]);  
        if (grainSpawnCounter > grainSpawnTime * grainRandMult && Globals.grainPop < Globals.maxPops[2]) plantSpawn(EList[2]);  
    }

    void plantSpawn(Entity plant)
    {
        if (plant.species() == Species.Grass)
        {
            grassSpawnCounter = 0;
            grassRandMult = Random.Range(0.5f, 2.4f);
        }
        if (plant.species() == Species.Carrot)
        {
            carrotSpawnCounter = 0;
            carrotRandMult = Random.Range(0.5f, 2.4f);
        }
        if (plant.species() == Species.Grain)
        {
            grainSpawnCounter = 0;
            grainRandMult = Random.Range(0.5f, 2.4f);
        }
        randomPosInstantiator(plant, 0);
    }

    void randomPosInstantiator(Entity entity, int count = 0)
    {
        Vector3 spawnPos = new Vector3(Random.Range(20, 280), entity.posY(), Random.Range(20, 280));
        while (spawnCheckCollisions(spawnPos) != 0) spawnPos = new Vector3(Random.Range(0, 300), entity.posY(), Random.Range(0, 300));
        GameObject body;
        if (count <= 2)
        {
            body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        else 
        {
            body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        Globals.componentAdder(entity, body);
        body.transform.position = spawnPos;
        Globals.popUpdate = true;
    }

    private int spawnCheckCollisions(Vector3 spawnpos)
    {
        Collider[] spawntest = Physics.OverlapBox(spawnpos, new Vector3(1, 0, 1));
        return spawntest.Length;
    }

}
