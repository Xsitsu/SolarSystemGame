using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    List<OrbitalGrid> _grids = new List<OrbitalGrid>();
    static private GridManager _instance;
	static public GridManager Instance { get { return _instance; } }
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
        
    }

    void Update()
    {
        foreach (OrbitalGrid grid in _grids)
        {
            if (grid.children.Count == 0)
            {
                _grids.Remove(grid);
            }
        }
    }

    public OrbitalGrid CreateOrbitalGrid(Entity entity, Vector3d relativePosition)
    {
        double radius = relativePosition.magnitude;

        double inclination = System.Math.Asin(relativePosition.y / radius);

        Vector3 unitCurrent = Vector3.Normalize(relativePosition.ToUnity());
        Vector3 unitBase = new Vector3(0, 0, -1);
        float len = (unitCurrent - unitBase).magnitude;

        double theta = 2 * (System.Math.Asin((len / 2)));

        double longOfAN = theta - 90;

        OrbitalGrid grid = new OrbitalGrid();
        grid.name = entity.name + " Grid";
        grid.SetOrbitData(0, relativePosition.magnitude, inclination, longOfAN, 90, StarSystemDisplay.Instance.CalculateCurrentTime());

        Debug.LogFormat("Created new grid: dist_to {0}, inclination {1}, longAN {2}", radius, inclination, longOfAN);

        entity.AddChild(grid);

        return grid;
    }
}
