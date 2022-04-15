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

    StarLogEntry MakeEntry(Entity entity)
    {
        GameObject obj = Instantiate(StarLogEntryPrefab);
        obj.name = entity.name;
        StarLogEntry entry = obj.GetComponent<StarLogEntry>();
        entry.SetText(entity.name);
        entry.entity = entity;
        entry.Init();

        if (entity is Star)
        {
            entry.SetIconColor(((Star)entity).color);
        }
        else if (entity is Planet)
        {
            entry.SetIconColor(((Planet)entity).color);
        }

        return entry;
    }
    void ProcessAddEntity(Entity entity)
    {
        if (entity is OrbitalBody || entity is Station)
        {
            StarLogEntry entry = MakeEntry(entity);
            AddEntry(entry);
        }

        foreach (Entity child in entity.children)
        {
            ProcessAddEntity(child);
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
