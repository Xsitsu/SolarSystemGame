using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    public GameObject button_interactables;
    public GameObject button_speedChange;
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
    }
    void Update()
    {
        
    }
    void ClickInteractables()
    {
        InteractableManager.Instance.SetVisible(!InteractableManager.Instance.GetVisible());
    }
    void ClickSpeedChange()
    {
        SolarSystemDisplay display = SolarSystemDisplay.Instance;
        if (display.timeFactor > 1.0)
        {
            display.timeFactor = 1.0;
        }
        else
        {
            display.timeFactor = 50000.0;
        }
    }
}
