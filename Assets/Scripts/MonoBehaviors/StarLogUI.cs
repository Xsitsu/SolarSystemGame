using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLogUI : MonoBehaviour
{
    public GameObject ListEntryFrame;
    public GameObject StarLogEntryPrefab;

    List<StarLogEntry> logEntries;

    void Start()
    {
        logEntries = new List<StarLogEntry>();

        Orbital orbital = new SystemGeneratorSol().Generate();
        DisplayStarSystem(orbital);
    }

    void Update()
    {
        
    }

    public void DisplayStarSystem(Orbital orbital)
    {
        ClearList();
        ProcessAddOrbital(orbital);
    }

    void ProcessAddOrbital(Orbital orbital)
    {
        //Debug.Log("ProcessAddOrbital for: " + orbital.name);

        StarLogEntry entry = Instantiate(StarLogEntryPrefab).GetComponent<StarLogEntry>();
        entry.SetText(orbital.name);

        if (orbital is Star)
        {
            entry.SetIconColor(((Star)orbital).color);
        }
        else if (orbital is Planet)
        {
            entry.SetIconColor(((Planet)orbital).color);
        }

        AddEntry(entry);

        if (orbital is OrbitalBody)
        {
            OrbitalBody ob = (OrbitalBody)orbital;
            foreach (Orbital child in ob.satellites)
            {
                ProcessAddOrbital(child);
            }
        }
    }

    void ClearList()
    {
        foreach (StarLogEntry entry in logEntries)
        {
            entry.gameObject.GetComponent<Transform>().SetParent(null);
        }
        
        logEntries = new List<StarLogEntry>();
    }

    void AddEntry(StarLogEntry entry)
    {
        entry.gameObject.GetComponent<Transform>().SetParent(ListEntryFrame.transform);

        int count = -logEntries.Count;

        RectTransform rect = entry.gameObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, count);
        rect.anchorMax = new Vector2(1, count + 1);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        logEntries.Add(entry);
    }
}
