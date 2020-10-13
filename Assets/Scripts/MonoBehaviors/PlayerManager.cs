using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject character;
    static private PlayerManager _instance;
	static public PlayerManager Instance { get { return _instance; } }
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
        SolarSystemDisplay display = SolarSystemDisplay.Instance;
        if (display != null && character != null)
        {
            character.transform.SetParent(display.gameObject.transform);
        }
    }

    void Update()
    {
        
    }
}
