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

        Entity entity = UniverseHandler.Instance.GetStarSystem();
        DisplayStarSystem(entity);
    }

    void Update()
    {
        
    }

    public void DisplayStarSystem(Entity entity)
    {
        ClearList();
        ProcessAddEntity(entity);
    }

    void ProcessAddEntity(Entity entity)
    {
        StarLogEntry entry = Instantiate(StarLogEntryPrefab).GetComponent<StarLogEntry>();
        entry.SetText(entity.name);

        if (entity is Star)
        {
            entry.SetIconColor(((Star)entity).color);
        }
        else if (entity is Planet)
        {
            entry.SetIconColor(((Planet)entity).color);
        }

        AddEntry(entry);

        if (entity is OrbitalBody)
        {
            OrbitalBody ob = (OrbitalBody)entity;
            foreach (Entity child in ob.children)
            {
                ProcessAddEntity(child);
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
