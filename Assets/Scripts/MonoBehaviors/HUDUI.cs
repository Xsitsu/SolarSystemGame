using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    public GameObject button_interactables;
    public GameObject button_speedChange;
    public GameObject label_speedSublight;
    public GameObject label_speedWarp;
    public GameObject label_speedC;

    Text speedSublight;
    Text speedWarp;
    Text speedC;
    void Start()
    {
        Button interactables = button_interactables.GetComponent<Button>();
        if (interactables)
        {
            interactables.onClick.AddListener(ClickInteractables);
        }

        Button speedChange = button_speedChange.GetComponent<Button>();
        if (speedChange)
        {
            speedChange.onClick.AddListener(ClickSpeedChange);
        }

        speedSublight = label_speedSublight.GetComponent<Text>();
        speedWarp = label_speedWarp.GetComponent<Text>();
        speedC = label_speedC.GetComponent<Text>();
    }
    void Update()
    {
        GameObject character = PlayerManager.Instance.character;
        if (character != null)
        {
            SublightEngineMono sublightEngine = character.GetComponent<SublightEngineMono>();
            WarpEngineMono warpEngine = character.GetComponent<WarpEngineMono>();
            double _speedM = sublightEngine.SpeedOut;
            double _warpFactor = warpEngine.WarpFactorOut;
            double _speedC = warpEngine.speedCOut;

            speedSublight.text = System.Math.Round(_speedM, 1).ToString() + " m/s";
            speedWarp.text = "Warp " + System.Math.Round(_warpFactor, 1).ToString();
            speedC.text = System.Math.Round(_speedC, 1).ToString() + "c";
        }
    }
    void ClickInteractables()
    {
        InteractableManager.Instance.SetVisible(!InteractableManager.Instance.GetVisible());
    }
    void ClickSpeedChange()
    {
        StarSystemDisplay display = StarSystemDisplay.Instance;
        //UniverseDisplay display = UniverseDisplay.Instance;

        if (display.timeFactor == 1.0)
        {
            display.timeFactor = 10.0;
        }
        else if (display.timeFactor == 10.0)
        {
            display.timeFactor = 100.0;
        }
        else if (display.timeFactor == 100.0)
        {
            display.timeFactor = 1000.0;
        }
        else if (display.timeFactor == 1000.0)
        {
            display.timeFactor = 10000.0;
        }
        else if (display.timeFactor == 10000.0)
        {
            display.timeFactor = 100000.0;
        }
        else
        {
            display.timeFactor = 1.0;
        }
    }
}
