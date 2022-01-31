using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseHandler : MonoBehaviour
{
    Star _star = null;
    OrbitalGrid _startingGrid = null;
    static private UniverseHandler _instance;
	static public UniverseHandler Instance { get { return _instance; } }
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

    }

    public Star GetStarSystem()
    {
        Debug.Log("GetStarSystem()");
        if (_star == null)
        {
            Debug.Log("_star == null");
            GenerateStarSystem();
        }
        return _star;
    }
    
    public OrbitalGrid GetStartingGrid()
    {
        if (_star == null)
        {
            GenerateStarSystem();
        }
        return _startingGrid;
    }

    void GenerateStarSystem()
    {
        Debug.Log("Generate Star System");

        // _GenerateTest();
        _GenerateSol();
    }

    void _GenerateTest()
    {
        _star = new SystemGeneratorTest().Generate();
        _startingGrid = (OrbitalGrid)_star.children[0].children[0];
    }

    void _GenerateSol()
    {
        _star = new SystemGeneratorSol().Generate();
        _startingGrid = (OrbitalGrid)_star.children[2].children[1];
    }
}
