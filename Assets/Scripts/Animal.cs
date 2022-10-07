using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Entity
{
    //misc
    protected int gender { private get; set; }
    private float tolerance { get; set; }

    //movement
    private Vector3 targetPos { get; set; }
    protected float movSpeed { private get; set; }
    protected float fastMultiplier { private get; set; }
    protected float slowMultiplier { private get; set; }
    private float multiplier { get; set; } = 1f;
    protected float stamina { private get; set; }
    protected float staminaRecharge { private get; set; }

    //movement controllers
    private float counter { get; set; }
    private float huntCounter { get; set; }
    private float fleeCounter { get; set; }
    private bool isHunting { get; set; }
    private bool isFleeing { get; set; }
    private bool isResting { get; set; }

    //reproduction controllers
    private float reproductionNeed { get; set; }
    protected float reproductionLim { private get; set; }
    protected int litterSize { private get; set; }
    private float litterSizeRandMult { get; set; }

    //death controllers
    protected float hungerLim { private get; set; }
    protected float thirstLim { private get; set; }
    protected float ageLim { private get; set; }
    private float hunger { get; set; }
    private float thirst { get; set; }
    private float age { get; set; }

    //interaction
    protected float sightRange { private get; set; }
    private SphereCollider trigger { get; set; }
    protected List<Species> prey { get; set; } = new List<Species>();
    private List<GameObject> preyInRange { get; set; } = new List<GameObject>();
    protected List<Species> predator { get; set; } = new List<Species>();

    protected void Setup()
    {
        Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = true;
        this.gameObject.transform.localScale = new Vector3(bodyScale, bodyScale, bodyScale);
        this.GetComponent<MeshRenderer>().material = material();

        trigger = this.gameObject.AddComponent<SphereCollider>();
        trigger.radius = sightRange;
        trigger.isTrigger = true;
        Globals.popUpdate = true;
        tolerance = Random.Range(0.5f, 0.8f);
    }
    protected void Update()
    {
        hunger += Time.deltaTime * Globals.simSpeed * Globals.harshness;
        age += Time.deltaTime * Globals.simSpeed;
        thirst += Time.deltaTime * Globals.simSpeed * Globals.harshness;
        counter += Time.deltaTime * Globals.simSpeed;

        //death   
        if (age > ageLim || hunger > hungerLim || thirst > thirstLim)
        {
            if (age > ageLim) Globals.deathByAge += 1;
            else if (hunger > hungerLim) Globals.deathByStarvation += 1;
            else if (thirst > thirstLim) Globals.deathByThirst += 1;
            Destroy(this.gameObject);
            Globals.popUpdate = true;
        }

        if (age >= ageLim * 0.15 && age <= ageLim * 0.75) reproductionNeed += Time.deltaTime * Globals.simSpeed;

        //In case of any errors
        if (this.gameObject.transform.position.x < 0 || this.gameObject.transform.position.x > 300 || this.gameObject.transform.position.z < 0 || this.gameObject.transform.position.z > 300) Destroy(this.gameObject);
        if (this.gameObject.transform.position.y != 3.215) this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 3.215f, this.gameObject.transform.position.z);

        move();
    }

    //handles logic
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Animal>(out Animal animal))
        {
            if (predator.Contains(animal.species()) && animal.isHunting)
            {
                flee(other);
            }

            else if (prey.Contains(animal.species()) && hunger >= hungerLim * tolerance)
            {
                hunt(other, animal);
            }

            else if (animal.species() == species() && reproductionNeed >= reproductionLim && animal.gender != gender && animal.reproductionNeed >= animal.reproductionLim && animal.hunger < animal.hungerLim * 0.5f / Globals.harshness && hunger < hungerLim * 0.5f / Globals.harshness && animal.thirst < animal.thirstLim * 0.5f / Globals.harshness && thirst < thirstLim * 0.5f / Globals.harshness)
            {
                reproduce(other, animal);
            }
        }

        else if (other.TryGetComponent<Water>(out Water water))
        {
            if (thirst >= thirstLim * tolerance)
            {
                targetPos = other.transform.position;
            }
        }

        else if (other.TryGetComponent<Plant>(out Plant food) && prey.Contains(food.species()) && hunger >= hungerLim * tolerance)
        {
            forage(other, food);
        }
    }

    private void flee(Collider other)
    {
        Vector3 directionVector = this.gameObject.transform.position - other.transform.position;

        //sets targetPos to be in the general direction directly opposite the predator, but has some random movements
        targetPos = this.gameObject.transform.position + directionVector * 50;
        targetPos = new Vector3((targetPos.x - transform.position.x) * Random.Range(0.5f, 10), targetPos.y, (targetPos.z - transform.position.z) * Random.Range(0.5f, 10));
        isFleeing = true;
        fleeCounter += Time.deltaTime * Globals.simSpeed * Globals.harshness;

        if (fleeCounter >= stamina)
        {
            isFleeing = false;
            isResting = true;
        }

        if (sqrDistance(targetPos) >= sightRange * sightRange) isFleeing = false;
    }

    private void closestDistanceCalculator(Entity potentialPrey)
    {
        float closestdistance = 1000000;
        preyInRange.Add(potentialPrey.gameObject);

        foreach (GameObject prey in preyInRange)
        {
            if (prey != null && sqrDistance(prey.transform.position) < closestdistance)
            {
                closestdistance = sqrDistance(prey.transform.position);
                targetPos = prey.transform.position;
            }
        }
    }

    private void hunt(Collider other, Animal animal)
    {
        closestDistanceCalculator(animal);

        isHunting = true;
        huntCounter += Time.deltaTime * Globals.simSpeed * Globals.harshness;

        //gives up after stamina amount of time, "blinds" and slows the animal
        if (huntCounter >= stamina && isHunting)
        {
            preyInRange.Clear();
            isHunting = false;
            trigger.radius = 0;
            isResting = true;
        }

        //eat if the predator touches the prey
        if (sqrDistance(targetPos) <= 1)
        {
            preyInRange.Clear();
            Destroy(other.gameObject);
            Globals.deathByHunter += 1;
            Globals.popUpdate = true;
            hunger -= animal.nutrition;
            isHunting = false;
            huntCounter = 0;
        }
    }

    private void reproduce(Collider other, Animal animal)
    {
        targetPos = other.transform.position;
        animal.targetPos = this.transform.position;
        if (sqrDistance(targetPos) <= 2 && gender == 1)
        {
            litterSizeRandMult = Random.Range(0.3f, 2f);
            for (int i = 1; i <= Mathf.Round(litterSize * litterSizeRandMult / Globals.harshness); i++)
            {
                GameObject childBody = GameObject.CreatePrimitive(PrimitiveType.Sphere); ;
                Globals.componentAdder(animal, childBody);
                childBody.transform.position = this.transform.position;
            }
            reproductionNeed = 0;
            animal.reproductionNeed = 0;
        }
    }

    private void forage(Collider other, Plant food)
    {
        closestDistanceCalculator(food);

        isHunting = true;

        if (sqrDistance(targetPos) <= 7)
        {
            hunger -= food.nutrition;
            food.nibbleCount += 1;
            isHunting = false;
        }
    }
    private void move()
    {
        if (isFleeing || isHunting) fastMove();
        else if (isResting) resting();
        else wander();
    }

    private void resting()
    {
        multiplier = slowMultiplier;
        //restores animals stats once energy is recharged
        if (huntCounter != 0) huntCounter += Time.deltaTime * Globals.simSpeed;
        if (huntCounter - stamina >= staminaRecharge)
        {
            trigger.radius = sightRange;
            multiplier = 1;
            huntCounter = 0;
            isResting = false;
        }

        if (fleeCounter != 0) fleeCounter += Time.deltaTime * Globals.simSpeed;
        if (fleeCounter - stamina >= staminaRecharge)
        {
            multiplier = 1;
            fleeCounter = 0;
            isResting = false;
        }

        wander();
    }

    private void fastMove()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetPos, movSpeed * Time.deltaTime * fastMultiplier * Globals.simSpeed);
    }

    private void wander()
    {
        if (Random.Range(2, 9) < counter)
        {
            targetPos = new Vector3(Random.Range(0, 300), this.gameObject.transform.position.y, Random.Range(0, 300));
            counter = 0;
        }
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetPos, movSpeed * Time.deltaTime * multiplier * Globals.simSpeed);
    }

    private float sqrDistance(Vector3 tempTargetPos)
    {
        float xDistance = this.gameObject.transform.position.x - tempTargetPos.x;
        float zDistance = this.gameObject.transform.position.z - tempTargetPos.z;
        return (xDistance * xDistance + zDistance * zDistance);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Water")
        {
            thirst = 0;

            //makes them avoid contact with the water once not thirsty
            Vector3 directionVector = this.gameObject.transform.position - other.transform.position;
            targetPos = this.gameObject.transform.position + new Vector3(directionVector.x * Random.Range(0.5f, 10f), 0, directionVector.z * Random.Range(0.5f, 10));
        }
    }
}
