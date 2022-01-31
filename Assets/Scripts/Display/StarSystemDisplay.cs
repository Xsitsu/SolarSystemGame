using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemDisplay : MonoBehaviour
{
    public GameObject nonePrefab;
    public GameObject starPrefab;
    public GameObject planetPrefab;
    public GameObject stationPrefab;

    public Entity anchor;
    public double timeFactor = 1.0;

    Star _star;
    OrbitalGrid _startingGrid;
    Hashtable entityMap = new Hashtable();

    static private StarSystemDisplay _instance;
	static public StarSystemDisplay Instance { get { return _instance; } }
    void Awake()
    {
        _instance = this;
    }
    void OnDestroy()
    {
        _instance = null;
    }

    void Start()
    {
        LoadStarSystem(new SystemGeneratorSol().Generate());
    }

    void Update()
    {
        Vector3d worldspacePosition = new Vector3d(0, 0, 0);
        Quaternion worldspaceRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        double current = Epoch.CurrentMilliseconds() / 1000.0;
        double useTime = current * timeFactor;
        PositionEntityParent(anchor, worldspacePosition, worldspaceRotation, useTime, null);
    }
    public Star GetStar()
    {
        return _star;
    }
    public void SetStructureAnchor(Structure newAnchor)
    {
        SetAnchor(newAnchor);
    }
    public OrbitalGrid GetStartingGrid()
    {
        return _startingGrid;
    }

    // public void SetNewOrbitalAnchor(GameObject gameObject)
    // {
    //     foreach (DictionaryEntry entry in orbitalMap)
    //     {
    //         if ((GameObject)(entry.Value) == gameObject)
    //         {
    //             if (entry.Key is OrbitalBody)
    //             {
    //                 OrbitalBody body = (OrbitalBody)entry.Key;

    //                 Debug.Log("Set new orbit anchor: " + body.name);

    //                 anchor.RemoveParent();
    //                 body.AddSatellite(anchor);
    //                 anchor.orbitRadius = body.radius * 1.4;

    //                 return;
    //             }
    //         }
    //     }
    // }

    public void LoadStarSystem(Star starSystem)
    {
        if (!StarSystemIsLoaded())
        {
            _star = starSystem;

            LoadEntity(_star);
            LoadDescendants(_star);

            //SetAnchor(_star);

            /*
            if (anchorObject != null)
            {
                ShipRenderer renderer = anchorObject.GetComponent<ShipRenderer>();
                if (renderer != null)
                {
                    renderer.SetLightSource(GetOrbitalObject(_star));
                }
            }
            */


            OrbitalBody earth = (OrbitalBody)_star.children[2];
            OrbitalGrid stationGrid = (OrbitalGrid)earth.children[1];
            _startingGrid = stationGrid;
        }

    }
    public void UnloadStarSystem()
    {
        if (StarSystemIsLoaded())
        {
            UnloadEntity(_star);
            UnloadDescendants(_star);

            //SetAnchor(null);

            _star = null;
            //anchor = null;

            /*
            if (anchorObject != null)
            {
                ShipRenderer renderer = anchorObject.GetComponent<ShipRenderer>();
                if (renderer != null)
                {
                    renderer.SetLightSource(GetOrbitalObject(null));
                }
            }
            */
        }
    }
    public bool StarSystemIsLoaded()
    {
        return (_star != null);
    }

    void LoadEntity(Entity entity)
    {
        if (!EntityIsLoaded(entity))
        {
            bool hasInteractable = false;

            GameObject go = null;
            if (entity is Planet)
            {
                Planet planet = (Planet)entity;
                go = Instantiate(planetPrefab);
                go.GetComponent<PlanetMono>().DisplayPlanet(planet);
                go.GetComponent<PlanetMono>().SetLightSource(GetEntityObject(_star));

                hasInteractable = true;
            }
            else if (entity is Star)
            {
                Star star = (Star)entity;
                go = Instantiate(starPrefab);
                go.GetComponent<StarMono>().DisplayStar(star);

                hasInteractable = true;
            }
            else if (entity is OrbitalGrid)
            {
                OrbitalGrid og = (OrbitalGrid)entity;
                go = Instantiate(nonePrefab);
                foreach (Entity en in og.children)
                {
                    LoadEntity(en);
                }
            }
            else if (entity is Station)
            {
                go = Instantiate(stationPrefab);

                hasInteractable = true;
            }
            else
            {
                go = Instantiate(nonePrefab);
            }

            if (go != null)
            {
                entityMap.Add(entity, go);
                go.transform.SetParent(transform);
                go.name = entity.name;

                Observable obs = go.GetComponent<Observable>();
                if (obs != null)
                {
                    obs.minZoom = (float)(entity.radiusM * 1.2);
                    obs.maxZoom = (float)(entity.radiusM * 4);
                    obs.defaultZoom = (float)(entity.radiusM * 1.5);
                }

                //if (!(orbital is OrbitalGrid))
                if (hasInteractable)
                {
                    InteractableManager.Instance.Register(go);
                    Interactable interactable = InteractableManager.Instance.GetInteractable(go);
                    if (interactable)
                    {
                        interactable.SetName(entity.name);
                        interactable.SetOffsetDistance(entity.radiusM);
                    }
                    else
                    {
                        Debug.Log("No interactable for: " + entity.name);
                    }
                }
            }
        }
    }
    void UnloadEntity(Entity entity)
    {
        if (EntityIsLoaded(entity))
        {
            if (entity is OrbitalGrid)
            {
                OrbitalGrid og = (OrbitalGrid)entity;
                foreach (Entity en in og.children)
                {
                    UnloadEntity(en);
                }
            }

            GameObject go = (GameObject)entityMap[entity];
            entityMap.Remove(entity);
            InteractableManager.Instance.Unregister(go);
            Destroy(go);
        }
    }
    bool EntityIsLoaded(Entity entity)
    {
        return entityMap.ContainsKey(entity);
    }
    GameObject GetEntityObject(Entity entity)
    {
        if (EntityIsLoaded(entity))
        {
            return (GameObject)entityMap[entity];
        }
        return null;
    }


    void SetAnchor(Structure newAnchor)
    {
        anchor = newAnchor;
    }

    void UnloadDescendants(Entity entity)
    {
        foreach (Entity child in entity.children)
        {
            UnloadEntity(child);
            UnloadDescendants(child);
        }
    }
    void LoadDescendants(Entity entity)
    {
        foreach (Entity child in entity.children)
        {
            LoadEntity(child);
            LoadDescendants(child);
        }
    }


    void PositionEntitySelf(Entity entity, Vector3d worldspacePosition, Quaternion worldspaceRotation, double currentTime)
    {
        if (EntityIsLoaded(entity))
        {
            GameObject obj = GetEntityObject(entity);
            obj.transform.localPosition = (worldspacePosition.ToUnity() / (float)Numbers.UnitsToMeters);
            obj.transform.rotation = worldspaceRotation;// * orbital.GetRotationalPeriodRotation(currentTime);
        }
    }
    void PositionEntityParent(Entity entity, Vector3d worldspacePosition, Quaternion worldspaceRotation, double currentTime, Entity fromChild)
    {
        PositionEntityChild(entity, worldspacePosition, worldspaceRotation, currentTime, fromChild);

        Entity parent = entity.parent;
        if (parent != null)
        {
            Vector3d localspacePosition = entity.CalculatePosition(currentTime);
            Quaternion localspaceRotation = entity.CalculateRotation(currentTime);

            Quaternion parentRotation = worldspaceRotation * Quaternion.Inverse(localspaceRotation);

            Vector3d worldspaceOffset = (parentRotation * localspacePosition.ToUnity()).ToUnityd();
            Vector3d parentPosition = worldspacePosition - worldspaceOffset;

            PositionEntityParent(parent, parentPosition, parentRotation, currentTime, entity);
        }
    }
    void PositionEntityChild(Entity entity, Vector3d worldspacePosition, Quaternion worldspaceRotation, double currentTime, Entity ignoreChild)
    {
        PositionEntitySelf(entity, worldspacePosition, worldspaceRotation, currentTime);

        foreach (Entity child in entity.children)
        {
            if (child != ignoreChild)
            {
                Vector3d localspacePosition = child.CalculatePosition(currentTime);
                Quaternion localspaceRotation = child.CalculateRotation(currentTime);

                Vector3d worldspaceOffset = (worldspaceRotation * localspacePosition.ToUnity()).ToUnityd();

                Vector3d childPosition = worldspacePosition + worldspaceOffset;
                Quaternion childRotation = worldspaceRotation * localspaceRotation;

                PositionEntityChild(child, childPosition, childRotation, currentTime, null);
            }

        }
    }
}
