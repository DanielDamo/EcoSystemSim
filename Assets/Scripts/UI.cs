using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; //need this for ui elements
using UnityEngine;

public class UI : MonoBehaviour
{
    private float count { get; set; }
    private float runTimeMin { get; set; }
    private float runTimeSec { get; set; }
    private float runTime { get; set; }
    private float simTimeMin { get; set; }
    private float simTimeSec { get; set; }
    private float simTime { get; set; }
    private List<Text> popsAndDeaths { get; set; } = new List<Text>();
    private List<Text> extras { get; set; } = new List<Text>();
    private List<Text> popLabels { get; set; } = new List<Text>();
    private bool UIToggle { get; set; } = true;
    private int callCount { get; set; }

    private GameObject graphBackground { get; set; }
    private RectTransform rTransform { get; set; }

    private Color orange { get; set; } = new Color(1f, 0.64f, 0.0f);

    private List<GameObject>[] dotList { get; set; } = new List<GameObject>[9];
    private List<Color> colourList { get; set; } = new List<Color>()
    {
        new Color(0, 0.4f, 0),  //grass
        new Color(1, 0.55f, 0),  //carrot
        new Color(0.86f, 1, 0),  //grain
        new Color(0.28f, 0.64f, 0.22f),  //grassHopper
        new Color(0.1f, 0.09f, 0.09f),  //mouse
        new Color(0.6f, 0.6f, 0.6f),  //rabbit
        new Color(0.66f, 0.38f, 0.08f),  //owl
        new Color(0.75f, 0.23f, 0.23f),  //hawk
        new Color(1, 0.59f, 0.18f),  //fox
    };

    private List<string> textString { get; set; } = new List<string>()
    {
        "Grass Pop",
        "Carrot Pop",
        "Grain Pop",
        "Grasshopper Pop",
        "Mouse Pop",
        "Rabbit Pop",
        "Owl Pop",
        "Hawk Pop", 
        "Fox Pop",
        "Plant Pop",
        "Animal Pop",
        "Total Pop",
        "Deaths due to age",
        "Deaths due to hunger",
        "Deaths due to thirst",
        "Deaths due to hunter"
    };

    void Start()
    {
        //initialising all values in the dotlist array
        for (int i = 0; i < 9; i++) dotList[i] = new List<GameObject>();

        //setting up extra UI elements
        Globals.textSetup("time", " ", 755, 480, 20, Color.green, this.transform, extras);
        Globals.textSetup("simTime", " ", 755, 455, 20, Color.green, this.transform, extras);
        Globals.textSetup("harshness", "Harshness: " + Globals.harshness, 755, -70, 20, Color.green, this.transform, extras);
        Globals.textSetup("tip", "Press the 'i' button to toggle the UI!", -110, -540, 20, Color.green, this.transform, extras);
        Globals.textSetup("speedControl", "Use + - to change speed. Current speed: 1x", -140, -560, 20, Color.green, this.transform, extras);
        Globals.textSetup("fps", " ", -890, 480, 20, Color.green, this.transform, extras);

        //Graph

        //graph background
        graphBackground = new GameObject("GraphBackground");
        graphBackground.AddComponent<CanvasRenderer>();
        rTransform = graphBackground.AddComponent<RectTransform>();
        Image background = graphBackground.AddComponent<Image>();
        graphBackground.transform.SetParent(this.transform);
        graphBackground.transform.localPosition = new Vector2(-565, -300);
        background.color = new Color(1, 1, 1, 0.7f);
        rTransform.sizeDelta = new Vector2(740, 420);

        //x axis
        GameObject xAxis = new GameObject("xAxis", typeof(Image));
        xAxis.transform.SetParent(graphBackground.transform);
        xAxis.GetComponent<RectTransform>().sizeDelta = new Vector2(740, 4);
        xAxis.GetComponent<Image>().color = Color.black;
        xAxis.transform.localPosition = new Vector2(0, -(rTransform.sizeDelta.y / 2) - 2);
        Globals.textSetup("xAxisLabel", "Time", 22, -54, 13, Color.black, xAxis.transform);

        //y axis
        GameObject yAxis = new GameObject("yAxis", typeof(Image));
        yAxis.transform.SetParent(graphBackground.transform);
        yAxis.GetComponent<RectTransform>().sizeDelta = new Vector2(4, 420);
        yAxis.GetComponent<Image>().color = Color.black;
        yAxis.transform.localPosition = new Vector2(-(rTransform.sizeDelta.x / 2) - 2, 0);
        Globals.textSetup("yAxisLabel", "Population", 30, -30, 13, Color.black, yAxis.transform, null , 90);

        //labels for each species
        for (int i = 0; i < 9; i++)
        {
            Globals.textSetup(i.ToString(), textString[i].Substring(0, textString[i].Length - 4), 0, 0, 10, colourList[i], graphBackground.transform, popLabels);
            popLabels[i].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            popLabels[i].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            if (i < 3 && Globals.maxPops[i] == 0) popLabels[i].gameObject.SetActive(false);
        }

        //max height of graph will be 100, and x axis will go to 10 minutes, 600 seconds.Very hard to make a rolling graph
        for (int i = 0; i < 9; i++)
        {
            graphMaker(0, (rTransform.sizeDelta.y * Globals.startPops[i]) / 105, colourList[i], dotList[i]);
        }

        //pop/death counters setup
        var y = 405;
        var counter = 0;
        foreach (string text in textString )
        {
            Globals.textSetup(textString[counter], " ", 755, y, 20 , Color.green, this.transform, popsAndDeaths);
            if (text == "Fox Pop" || text == "Total Pop") y -= 25;  //spacing
            y -= 25;
            counter++;
        }    
    }

    void Update()
    {
        if (count == 0) Globals.popUpdate = true;
        count += Time.deltaTime;

        // real time
        runTime += Time.deltaTime;
        runTimeMin = Mathf.Floor(runTime / 60);
        runTimeSec = Mathf.Floor(runTime - runTimeMin * 60);
        if (runTimeSec < 10 && runTimeMin < 10) extras[0].text = $"Elapsed time: 0{ runTimeMin.ToString()}:0{ runTimeSec.ToString()}";
        if (runTimeSec > 10 && runTimeMin < 10) extras[0].text = $"Elapsed time: 0{ runTimeMin.ToString()}:{ runTimeSec.ToString()}";
        if (runTimeSec > 10 && runTimeMin > 10) extras[0].text = $"Elapsed time: { runTimeMin.ToString()}:{runTimeSec.ToString()}";

        // sim time
        simTime += Time.deltaTime * Globals.simSpeed;
        simTimeMin = Mathf.Floor(simTime / 60);
        simTimeSec = Mathf.Floor(simTime - simTimeMin * 60);
        if (simTimeSec < 10 && simTimeMin < 10) extras[1].text = $"Simulated time: 0{ simTimeMin.ToString()}:0{simTimeSec.ToString()}";
        if (simTimeSec > 10 && simTimeMin < 10) extras[1].text = $"Simulated time: 0{ simTimeMin.ToString()}:{simTimeSec.ToString()}";
        if (simTimeSec > 10 && simTimeMin > 10) extras[1].text = $"Simulated time: { simTimeMin.ToString()}:{simTimeSec.ToString()}";

        //fps
        if (count > 0.1)
        {
            extras[5].text = (Mathf.Round(1 / Time.deltaTime)).ToString();
            count = 0;
        }

        //sim speed control
        if (Input.GetKeyDown(KeyCode.Equals) && Globals.simSpeed < 8)
        {
            Globals.simSpeed += 0.5f;
            extras[4].text = $"Use + - to change speed. Current speed: {Globals.simSpeed}x";
        }
        if (Input.GetKeyDown(KeyCode.Minus) && Globals.simSpeed > 0.5)
        {
            Globals.simSpeed -= 0.5f;
            extras[4].text = $"Use + - to change speed. Current speed: { Globals.simSpeed}x";
        }              

        //UI toggle
        if (Input.GetKeyDown(KeyCode.I))
        {          
            foreach (Text pop in popsAndDeaths) pop.gameObject.SetActive(!UIToggle);
            foreach (Text extra in extras) extra.gameObject.SetActive(!UIToggle);
            graphBackground.gameObject.SetActive(!UIToggle);
            extras[3].gameObject.SetActive(false); // tip, so once toggled off, wont reappear

            UIToggle = !UIToggle;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        //updating UI
        if (Globals.popUpdate)
        {
            Globals.popUpdate = false;

            int[] totalSpeciesPops = new int[9];
            totalSpeciesPops[0] = (FindObjectsOfType(typeof(Grass)) as Grass[]).Length;
            totalSpeciesPops[1] = (FindObjectsOfType(typeof(Carrot)) as Carrot[]).Length;
            totalSpeciesPops[2] = (FindObjectsOfType(typeof(Grain)) as Grain[]).Length;
            totalSpeciesPops[3] = (FindObjectsOfType(typeof(Grasshopper)) as Grasshopper[]).Length;
            totalSpeciesPops[4] = (FindObjectsOfType(typeof(Mouse)) as Mouse[]).Length;
            totalSpeciesPops[5] = (FindObjectsOfType(typeof(Rabbit)) as Rabbit[]).Length;
            totalSpeciesPops[6] = (FindObjectsOfType(typeof(Owl)) as Owl[]).Length;
            totalSpeciesPops[7] = (FindObjectsOfType(typeof(Hawk)) as Hawk[]).Length;
            totalSpeciesPops[8] = (FindObjectsOfType(typeof(Fox)) as Fox[]).Length;

            var plantsPop = (totalSpeciesPops[0] + totalSpeciesPops[1] + totalSpeciesPops[2]);
            var animalsPop = (totalSpeciesPops[3] + totalSpeciesPops[4] + totalSpeciesPops[5] + totalSpeciesPops[6] + totalSpeciesPops[7] + totalSpeciesPops[8]);
            var totalsPop = plantsPop + animalsPop;
            List<int> overallPopAndDeath = new List<int>() { plantsPop, animalsPop, totalsPop, Globals.deathByAge, Globals.deathByStarvation, Globals.deathByThirst, Globals.deathByHunter };

            Globals.grassPop = totalSpeciesPops[0];
            Globals.carrotPop = totalSpeciesPops[1];
            Globals.grainPop = totalSpeciesPops[2];

            var counter = 0;
            foreach (int pop in totalSpeciesPops)
            {
                //updating population counters
                popsAndDeaths[counter].text = $"{textString[counter]}: {totalSpeciesPops[counter].ToString()}";
                if (pop >= 30 && popsAndDeaths[counter].color != Color.green) popsAndDeaths[counter].color = Color.green;
                else if (pop < 30 && pop >= 20 && popsAndDeaths[counter].color != Color.yellow) popsAndDeaths[counter].color = Color.yellow;
                else if (pop < 20 && pop >= 10 && popsAndDeaths[counter].color != orange) popsAndDeaths[counter].color = orange;
                else if (pop < 10 && popsAndDeaths[counter].color != Color.red) popsAndDeaths[counter].color = Color.red;

                //Updating graph, max value on x axis is 600 simulated seconds
                if (simTime <= 600)
                {
                    popLabels[counter].transform.localPosition = new Vector2((rTransform.sizeDelta.x * simTime) / 600 - 320, (rTransform.sizeDelta.y * pop) / 105 - 250);
                    if (pop > 0 || counter < 3 && Globals.maxPops[counter] != 0) graphMaker((rTransform.sizeDelta.x * simTime) / 600, (rTransform.sizeDelta.y * pop) / 105, colourList[counter], dotList[counter]);
                    else callCount++;
                    if (pop == 0 && counter > 2) popLabels[counter].gameObject.SetActive(false);
                }
                counter++;
            }

            //updating death counters
            foreach (int popAndDeath in overallPopAndDeath)
            {
                popsAndDeaths[counter].text = $"{textString[counter]}: {overallPopAndDeath[counter - 9].ToString()}";
                counter++;
            }
        }
    }

    void graphMaker(float xPos, float yPos, Color color, List<GameObject> dotList)
    {
        callCount++;
        GameObject dot = new GameObject("Dot", typeof(Image));
        dot.transform.SetParent(graphBackground.GetComponent<RectTransform>());
        dot.GetComponent<Image>().color = color;
        dot.transform.localPosition = new Vector2(xPos, yPos);
        dot.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        dot.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        dot.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        dotList.Add(dot);
        if (callCount > 9)
        {
            graphLines(dotList[Mathf.FloorToInt(callCount / 9 - 1)], dotList[Mathf.FloorToInt(callCount / 9 - 1) - 1], color);
        }
    }

    void graphLines(GameObject dot1, GameObject dot2, Color color)
    {
        GameObject line = new GameObject("Line", typeof(Image));
        line.transform.SetParent(graphBackground.GetComponent<RectTransform>());
        line.GetComponent<Image>().color = color;
        line.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        line.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);

        Vector2 direction = (dot2.transform.localPosition - dot1.transform.localPosition).normalized;
        float distance = Vector2.Distance(dot1.transform.localPosition, dot2.transform.localPosition);

        line.GetComponent<RectTransform>().sizeDelta = new Vector2(distance, 1);
        line.transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI));
        line.transform.localPosition = new Vector2(dot1.transform.localPosition.x + (direction.x * distance * 0.5f), dot1.transform.localPosition.y + (direction.y * distance * 0.5f));

        Destroy(dot2.gameObject);
    }
}
