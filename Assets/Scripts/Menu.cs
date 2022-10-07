using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Slider slider { get; set; }
    List<Slider> sliderList { get; set; } = new List<Slider>();
    List<Text> labels { get; set; } = new List<Text>();
    List<string> textStartPops { get; set; } = new List<string>();

    void Start()
    {
        slider = Resources.Load("Entities/Slider", typeof(Slider)) as Slider;
        Color fontColor = new Color(0, 1, 0);
        textStartPops.AddRange(new List<string>() { "Grass", "Carrot", "Grain", "Insect", "Mouse", "Rabbit", "Owl", "Hawk", "Fox", "Grass", "Carrot", "Grain" });

        //background
        GameObject menuBackground;
        RectTransform rTransform;
        menuBackground = new GameObject("MenuBackground");
        rTransform = menuBackground.AddComponent<RectTransform>();
        menuBackground.AddComponent<CanvasRenderer>();
        menuBackground.transform.SetParent(this.transform);
        menuBackground.transform.localPosition = new Vector2(0, 0);
        Image background = menuBackground.AddComponent<Image>();
        background.color = new Color(0.09f, 0.65f, 0.925f);
        rTransform.sizeDelta = new Vector2(1920, 1080); //program is best run on FHD 16:9 screens

        //Title
        Globals.textSetup("Title", "ECOSYSTEM SIMULATOR", -515, 350, 90, fontColor, this.transform);

        //sliders
        int x = -800;
        int y = 0;
        for (int i = 0; i <= 11; i++)
        {
            if(i == 9) 
            {
                y = -200;
                x = -200;
            }     
            sliderSetup(0, 50, 25, x, y);
            Globals.textSetup(textStartPops[i], textStartPops[i], 0, 10, 20, fontColor, sliderList[i].transform, labels);
            x += 200;
        }

        //maximum value for plant population is increased to 100 
        for (int i = 0; i < 3; i++)
        {
            sliderList[i + 9].maxValue = 100;
            sliderList[i + 9].value = 100;
            sliderList[i].maxValue = 100;
            sliderList[i].value = 50;
        }

        //max population text
        Globals.textSetup("maxPop", "Max Populations: ", -60, -120, 30, fontColor, this.transform);

        //start population text
        Globals.textSetup("startPop", "Start Populations: ", -60, 120, 30, fontColor, this.transform);

        //how to start text
        Globals.textSetup("start", "Press ENTER to start", -160, -440, 40, fontColor, this.transform);

        //harshness slider + text
        sliderSetup(50, 150, 100, 0, -340);
        Globals.textSetup("Harshness", "Harshness: ", -15, 0, 20, fontColor, sliderList[12].transform, labels);
    }

    void Update()
    {
        //Updating labels for sliders to reflect current selection
        for (int i = 0; i<= 11; i++)
        {
            labels[i].text = $"{textStartPops[i]}: {sliderList[i].value.ToString()}";
        }
        labels[12].text = $"Harshness: { (sliderList[12].value / 100).ToString()}x";

        //starts the simulation, locks in selection
        if (Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 0; i < 9; i++)
            {
                Globals.startPops[i] = Mathf.RoundToInt(sliderList[i].value);
            }
            for (int i = 9; i < 12; i++)
            {
                Globals.maxPops[i - 9] = Mathf.RoundToInt(sliderList[i].value);
            }
            Globals.harshness = Mathf.RoundToInt(sliderList[12].value) / 100f;
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    void sliderSetup(int minValue, int maxValue, int value, int x, int y)
    {
        var s = Instantiate(slider);
        s.transform.SetParent(this.transform);
        s.minValue = minValue;
        s.maxValue = maxValue;
        s.value = value;
        s.transform.localPosition = new Vector2(x, y);
        sliderList.Add(s);
    }
}
